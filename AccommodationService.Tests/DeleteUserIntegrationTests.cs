using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using AccommodationService.Data;
using AccommodationService.Model.Dto;
using AccommodationService.Model.Entity;
using AccommodationService.Service.MessagingService;
using NUnit.Framework;
using AccommodationService.Model.Messages;
using Microsoft.Extensions.Configuration;
using Confluent.Kafka;
using System.Net.Http.Headers;

namespace AccommodationService.Tests;

[TestFixture]
[Category("Integration")]
public class DeleteUserIntegrationTests
{
    private HttpClient _client;
    private CustomWebApplicationFactory _factory;
    private string KafkaBroker = "localhost:9092";

    private Guid uuid = Guid.NewGuid();

    [OneTimeSetUp]
    public async Task Setup()
    {
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient();

        var config = _factory.Services.GetRequiredService<IConfiguration>();
        KafkaBroker = config.GetValue<string>("KafkaConfig:Producer:BootstrapServers");


    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [Test]
    public async Task GetUser_NoUser()
    {
        var user = new UserDto { Id = uuid, Username = "host@example.com", Role = "HOST" };

        await SetupDbData();
        await ProduceKafkaMessage<UserDto>(KafkaTopic.DeleteUser.ToString(), user);

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        Assert.That(db.Accommodations.Count() == 0);
        Assert.That(db.Users.Count() == 0);
    }


   private async Task SetupDbData()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var user = new User { Id = 0, ExternalId = Guid.NewGuid(), Status = true, Username = "host@example.com" };
        db.Users.Add(user);
        db.SaveChanges();

        var address = new Address
        {
            City = "city",
            Country = "country",
            PostNumber = "post num",
            StreetName = "StreetName",
            StreetNumber = "StreetNumber"
        };

        var accommodation = new Accommodation { Id = 0, ExternalId = Guid.NewGuid(), OwnerId = 0, Owner = user, Name = "name", Description = "desc", Address = address };
        db.Accommodations.Add(accommodation);
        db.SaveChanges();
    }

    private async Task ProduceKafkaMessage<T>(string topic, T message)
    {
        var config = new ProducerConfig { BootstrapServers = KafkaBroker };
        using var producer = new ProducerBuilder<Null, string>(config).Build();
        var json = JsonSerializer.Serialize(message);
        await producer.ProduceAsync(topic, new Message<Null, string> { Value = json });
        producer.Flush(TimeSpan.FromSeconds(5));
    }

}
