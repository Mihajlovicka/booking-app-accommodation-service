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
    private readonly KafkaTopic _topicName;
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapperManager _mapperManager;

    public ConsumerService(
        ILogger<ConsumerService> logger,
        IOptions<ConsumerConfig> config,
        KafkaTopic topicName,
        IRepositoryManager repositoryManager,
        IMapperManager mapperManager)
    {
        _logger = logger;
        _topicName = topicName;
        _repositoryManager = repositoryManager;
        _mapperManager = mapperManager;
        _consumer = new ConsumerBuilder<Ignore, string>(config.Value).Build();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe(_topicName.ToString());

        Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(TimeSpan.FromSeconds(5));
                    if (consumeResult is null)
                        continue;

                    _logger.LogInformation($"Kafka message received: {consumeResult.Message.Value}");

                    var type = TopicTypeMap.Map.GetValueOrDefault(_topicName)
                               ?? throw new InvalidOperationException($"No type map found for topic {_topicName}");

                    var message = JsonConvert.DeserializeObject(consumeResult.Message.Value, type);

                    if (message is not UserDto userDto)
                    {
                        _logger.LogWarning("Message was not a UserDto. Skipping...");
                        continue;
                    }

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
