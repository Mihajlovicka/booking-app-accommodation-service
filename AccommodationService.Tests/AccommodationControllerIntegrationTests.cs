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
public class AccommodationControllerIntegrationTests
{
    private HttpClient _client;
    private CustomWebApplicationFactory _factory;
    private string KafkaBroker = "localhost:9092";

    [OneTimeSetUp]
    public async Task Setup()
    {
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient();

        await SetupDbData();

        var config = _factory.Services.GetRequiredService<IConfiguration>();
        KafkaBroker = config.GetValue<string>("KafkaConfig:Producer:BootstrapServers");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");


    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [Test]
    public async Task GetAllByUser_WithHostToken_ReturnsOkAndData()
    {


        var response = await _client.GetAsync("/api/accommodations");
        response.EnsureSuccessStatusCode();

        var accommodations = await response.Content.ReadFromJsonAsync<IEnumerable<AccommodationDto>>();
        Assert.That(accommodations, Is.Not.Null);
        Assert.That(accommodations, Is.Not.Empty);
    }

    [Test]
    public async Task UpdatePriceType_WithValidData_ReturnsNoContent()
    {

        // Get first accommodation id
        var responseGet = await _client.GetAsync("/api/accommodations");
        responseGet.EnsureSuccessStatusCode();
        var accommodations = await responseGet.Content.ReadFromJsonAsync<IEnumerable<AccommodationDto>>();
        var firstAccommodation = accommodations.First();

        var updateDto = new UpdatePriceTypeDto { PriceType = "PerGuest" };

        var responsePatch = await _client.PatchAsJsonAsync(
            $"/api/accommodations/{firstAccommodation.Id}/price-type",
            updateDto
        );

        Assert.That(System.Net.HttpStatusCode.NoContent, Is.EqualTo(responsePatch.StatusCode));
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

    private async Task<string> ConsumeKafkaMessage(string topic)
    {
        var config = new ConsumerConfig
            {
                BootstrapServers = KafkaBroker,
                GroupId = $"test-group-{Guid.NewGuid()}",
                AutoOffsetReset = AutoOffsetReset.Earliest,
            };

        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe(topic);

        var timeout = TimeSpan.FromSeconds(10);
        var start = DateTime.Now;

        try
        {
            while (DateTime.Now - start < timeout)
            {
                var result = consumer.Consume(100);
                if (result != null && !string.IsNullOrWhiteSpace(result.Message?.Value))
                {
                    return result.Message.Value;
                }
            }
        }
        finally
        {
            consumer.Close();
        }
        return null; // Timeout if no message is received
    }

}
