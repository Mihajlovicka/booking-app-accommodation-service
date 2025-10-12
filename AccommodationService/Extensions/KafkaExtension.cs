using AccommodationService.Mapper;
using AccommodationService.Repository.Contract;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using AccommodationService.Service.MessagingService;

namespace AccommodationService.Extensions;

public static class KafkaExtensions
{
    public static IServiceCollection AddKafkaServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Configure Producer
        services.Configure<ProducerConfig>(configuration.GetSection("KafkaConfig:Producer"));
        services.AddSingleton<ProducerService>();

        // Configure Consumer
        services.Configure<ConsumerConfig>(configuration.GetSection("KafkaConfig:Consumer"));
        services.AddHostedService(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<ConsumerService>>();
            var consumerConfig = provider.GetRequiredService<IOptions<ConsumerConfig>>();
            var repositoryManager = provider.GetRequiredService<IRepositoryManager>();
            var mapperManager = provider.GetRequiredService<IMapperManager>(); 
            var topic = KafkaTopic.UserCreated;

            return new ConsumerService(logger, consumerConfig, topic, repositoryManager, mapperManager);
        });

        return services;
    }
}
