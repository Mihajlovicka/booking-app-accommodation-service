using AccommodationService.Mapper;
using AccommodationService.Model.Dto;
using AccommodationService.Model.Messages;
using AccommodationService.Repository.Contract;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AccommodationService.Service.MessagingService;

public class ConsumerService : BackgroundService
{
    private readonly ILogger<ConsumerService> _logger;
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly IEnumerable<KafkaTopic> _topics;
    private readonly IServiceScopeFactory _scopeFactory;

    public ConsumerService(
        ILogger<ConsumerService> logger,
        IOptions<ConsumerConfig> config,
        IEnumerable<KafkaTopic> topics,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _topics = topics;
        _scopeFactory = scopeFactory;
        _consumer = new ConsumerBuilder<Ignore, string>(config.Value).Build();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe(_topics.Select(t => t.ToString()).ToList());

        Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();

                    var _repositoryManager = scope.ServiceProvider.GetRequiredService<IRepositoryManager>();
                    var _mapperManager = scope.ServiceProvider.GetRequiredService<IMapperManager>();

                    var consumeResult = _consumer.Consume(TimeSpan.FromSeconds(5));
                    if (consumeResult is null)
                        continue;

                    var topic = consumeResult.Topic;
                    _logger.LogInformation($"Kafka message received on topic {topic}: {consumeResult.Message.Value}");
                    
                    var topicEnum = Enum.Parse<KafkaTopic>(topic);
                    var type = TopicTypeMap.Map.GetValueOrDefault(topicEnum)
                               ?? throw new InvalidOperationException($"No type map found for topic {topic}");

                    var message = JsonConvert.DeserializeObject(consumeResult.Message.Value, type);

                    switch (message)
                    {
                        case UserDto userDto:
                            {
                                if (topic == KafkaTopic.UserCreated.ToString())
                                {
                                    if (!Enum.TryParse<UserRole>(userDto.Role, true, out var role))
                                    {
                                        _logger.LogWarning($"Invalid role '{userDto.Role}' received. Skipping user '{userDto.Username}'.");
                                        continue;
                                    }

                                    if (role != UserRole.HOST)
                                    {
                                        _logger.LogInformation($"Skipping user '{userDto.Username}' with role '{role}'. Only HOST is allowed.");
                                        continue;
                                    }

                                    var userEntity = await _mapperManager.UserDtoToUserMapper.Map(userDto);
                                    await _repositoryManager.UserRepository.AddAsync(userEntity);
                                    _logger.LogInformation($"✅ HOST user '{userDto.Username}' saved successfully!");
                                }
                                else if (topic == KafkaTopic.DeleteUser.ToString())
                                {
                                    var user = await _repositoryManager.UserRepository.GetByUsernameAsync(userDto.Username);
                                    await _repositoryManager.AccommodationRepository.DeleteAllByOwner(user.Id);
                                    await _repositoryManager.UserRepository.DeleteAsync(user.Id);
                                    _logger.LogInformation($"User '{userDto.Username}' delete in BookingService.");
                                }
                                break;

                            }
                        default:
                            _logger.LogWarning($"Unknown message type received for topic {topic}");
                            break;
                    }
                }
                catch (OperationCanceledException)
                {
                    // shutdown signal, safe to ignore
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while consuming Kafka message");
                }
            }
        }, stoppingToken);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _consumer.Close();
        _consumer.Dispose();
        base.Dispose();
    }
}
