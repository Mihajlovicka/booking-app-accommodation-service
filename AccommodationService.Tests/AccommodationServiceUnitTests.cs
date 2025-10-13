using System.Linq.Expressions;
using AccommodationService.Mapper;
using AccommodationService.Model.Dto;
using AccommodationService.Model.Entity;
using AccommodationService.Model.Messages;
using AccommodationService.Repository.Contract;
using AccommodationService.Service.Contract;
using AccommodationService.Service.MessagingService;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

[TestFixture]
public class AccommodationServiceUnitTests
{
    private Mock<IRepositoryManager> repoManagerMock;
    private Mock<IUserRepository> userRepoMock;
    private Mock<IMapperManager> mapperManagerMock;
     private Mock<IAccommodationRepository> accRepoMock;
    private Mock<ProducerService> mockProducerService;
    private Mock<IUserContext> mockUserContextService;
    private AccommodationService.Service.Implementation.AccommodationService service;

    [SetUp]
    public void Setup()
    {
        repoManagerMock = new Mock<IRepositoryManager>();
        userRepoMock = new Mock<IUserRepository>();
        accRepoMock = new Mock<IAccommodationRepository>();
        mapperManagerMock = new Mock<IMapperManager>();


        var producerConfig = new ProducerConfig { BootstrapServers = "localhost:9092" };
        var mockKafkaConfig = Mock.Of<IOptions<ProducerConfig>>(options =>
            options.Value == producerConfig
        );

        mockProducerService = new Mock<ProducerService>(
            new Mock<ILogger<ProducerService>>().Object,
            mockKafkaConfig
        );
        mockUserContextService = new Mock<IUserContext>();

        repoManagerMock.Setup(r => r.UserRepository).Returns(userRepoMock.Object);
        repoManagerMock.Setup(r => r.AccommodationRepository).Returns(accRepoMock.Object);

        service = new AccommodationService.Service.Implementation.AccommodationService(
            repoManagerMock.Object,
            mockUserContextService.Object,
            mapperManagerMock.Object,
            mockProducerService.Object
        );

        mockProducerService.Setup(p => p.ProduceAsync(It.IsAny<string>(), It.IsAny<object>()));
    }

    [Test]
    public void Save_ShouldThrowException_WhenNameIsNotUnique()
    {
        var accommodation = new Accommodation { Name = "TestAcc" };
        accRepoMock.Setup(r => r.ExistsAsync(a => a.Name == "TestAcc")).ReturnsAsync(true);

        Assert.ThrowsAsync<Exception>(() => service.Save(accommodation, "john"));
    }
    
    [Test]
    public void Save_ShouldThrowException_WhenUserNotFound()
    {
        // Arrange
        var accommodation = new Accommodation { Name = "TestAcc" };
        accRepoMock.Setup(r => r.ExistsAsync(a => a.Name == "TestAcc")).ReturnsAsync(false); // name unique
        userRepoMock.Setup(r => r.GetByUsernameAsync("john")).ReturnsAsync((User?)null);

        // Act & Assert
        Assert.ThrowsAsync<Exception>(() => service.Save(accommodation, "john"));
    }

    [Test]
    public async Task Save_ShouldCallAddAsync_WhenValid()
    {
        // Arrange
        var accommodation = new Accommodation { Name = "TestAcc" };
        accRepoMock.Setup(r => r.ExistsAsync(a => a.Name == "TestAcc")).ReturnsAsync(false);
        userRepoMock.Setup(r => r.GetByUsernameAsync("john"))
            .ReturnsAsync(new User { Id = 1, Username = "john" });

         var accomodationDto = new AccommodationCreatedDto { Id = Guid.Empty };
        mapperManagerMock.Setup(r => r.AccommodationToAccommodationCreatedDtoMapper.Map(accommodation))
            .ReturnsAsync(accomodationDto);


        // Act
        await service.Save(accommodation, "john");

        // Assert
        accRepoMock.Verify(r => r.AddAsync(It.IsAny<Accommodation>()), Times.Once);
    }
    
    [Test]
    public async Task Save_ShouldSetOwnerIdCorrectly()
    {
        // Arrange
        var user = new User { Id = 1, Username = "john" };
        var accommodation = new Accommodation { Name = "TestAcc" };
        var accomodationDto = new AccommodationCreatedDto { Id = Guid.Empty };

        accRepoMock.Setup(r => r.ExistsAsync(a => a.Name == "TestAcc")).ReturnsAsync(false);
        userRepoMock.Setup(r => r.GetByUsernameAsync("john")).ReturnsAsync(user);
        mapperManagerMock.Setup(r => r.AccommodationToAccommodationCreatedDtoMapper.Map(accommodation))
            .ReturnsAsync(accomodationDto);
        // Act
        await service.Save(accommodation, "john");

        // Assert
        Assert.That(accommodation.OwnerId, Is.EqualTo(user.Id));
    }

    [Test]
    public async Task Save_ShouldCheckNameUniqueness()
    {
        // Arrange
        var accommodation = new Accommodation { Name = "TestAcc" };
        accRepoMock.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Accommodation, bool>>>()))
            .ReturnsAsync(false);
        userRepoMock.Setup(r => r.GetByUsernameAsync("john"))
            .ReturnsAsync(new User { Id = 1 });

        mapperManagerMock.Setup(r => r.AccommodationToAccommodationCreatedDtoMapper.Map(accommodation))
            .ReturnsAsync((AccommodationCreatedDto?)null);

        // Act
        await service.Save(accommodation, "john");

        // Assert
        accRepoMock.Verify(r => r.ExistsAsync(It.IsAny<Expression<Func<Accommodation, bool>>>()), Times.Once);
    }

    [Test]
    public void Save_ShouldThrowException_WhenNameIsEmpty()
    {
        var accommodation = new Accommodation { Name = "" };

        Assert.ThrowsAsync<Exception>(() => service.Save(accommodation, "john"));
    }
    
    [Test]
    public async Task GetAllByUser_ReturnsMappedDtos_WhenUserExists()
    {
        var user = new User { Id = 1, Username = "testuser" };
        var accommodations = new List<Accommodation>
        {
            new Accommodation { Id = 1, OwnerId = 1, Owner = user, ExternalId = Guid.NewGuid() },
            new Accommodation { Id = 2, OwnerId = 1, Owner = user, ExternalId = Guid.NewGuid() }
        };

        mockUserContextService.Setup(u => u.Name).Returns("testuser");
        repoManagerMock.Setup(r => r.UserRepository.GetByUsernameAsync("testuser"))
                    .ReturnsAsync(user);
        repoManagerMock.Setup(r => r.AccommodationRepository.GetAllByOwnerIdAsync(user.Id))
                    .ReturnsAsync(accommodations);

        mapperManagerMock.Setup(m => m.AccommodationToAccommodationDtoMapper.Map(It.IsAny<Accommodation>()))
                    .ReturnsAsync((Accommodation a) => new AccommodationDto { Id = a.ExternalId.ToString(), Owner = a.Owner.Username });

        var result = await service.GetAllByUser();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(2));
    }

    [Test]
    public void GetAllByUser_ThrowsException_WhenUserNotFound()
    {
        mockUserContextService.Setup(u => u.Name).Returns("unknown");
        repoManagerMock.Setup(r => r.UserRepository.GetByUsernameAsync("unknown"))
                    .ReturnsAsync((User)null);

        Assert.ThrowsAsync<Exception>(async () => await service.GetAllByUser());
    }

        [Test]
    public async Task UpdatePriceType_UpdatesAndProduces_WhenAccommodationExists()
    {
        var user = new User { Id = 1, Username = "testuser" };
        var accommodation = new Accommodation { ExternalId = Guid.NewGuid(), PriceType = PriceType.PerGuest, Owner = user};
        var dto = new UpdatePriceTypeDto { PriceType = "PerUnit" };

        repoManagerMock.Setup(r => r.AccommodationRepository.GetByExternalIdAsync("abc123"))
                    .ReturnsAsync(accommodation);
        mapperManagerMock.Setup(m => m.AccommodationToAccommodationCreatedDtoMapper.Map(accommodation))
                    .ReturnsAsync(new AccommodationCreatedDto { Id = accommodation.ExternalId, Owner = accommodation.Owner.Username, PriceType = PriceType.PerUnit });


        // Act
        await service.UpdatePriceType("abc123", dto);

        // Assert
        Assert.That(PriceType.PerUnit, Is.EqualTo(accommodation.PriceType));
        repoManagerMock.Verify(r => r.AccommodationRepository.UpdateAsync(accommodation), Times.Once);
    }

    [Test]
    public void UpdatePriceType_ThrowsKeyNotFound_WhenAccommodationDoesNotExist()
    {
        // Arrange
        repoManagerMock.Setup(r => r.AccommodationRepository.GetByExternalIdAsync("unknown"))
                    .ReturnsAsync((Accommodation)null);
        var dto = new UpdatePriceTypeDto { PriceType = "PerUnit" };

        // Act & Assert
        Assert.ThrowsAsync<KeyNotFoundException>(async () => await service.UpdatePriceType("unknown", dto));
    }
}