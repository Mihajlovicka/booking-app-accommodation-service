using System.Linq.Expressions;
using AccommodationService.Model.Entity;
using AccommodationService.Repository.Contract;
using Moq;

[TestFixture]
public class AccommodationServiceUnitTests
{
    private Mock<IRepositoryManager> repoManagerMock;
    private Mock<IUserRepository> userRepoMock;
    private Mock<IAccommodationRepository> accRepoMock;
    private AccommodationService.Service.Implementation.AccommodationService service;

    [SetUp]
    public void Setup()
    {
        repoManagerMock = new Mock<IRepositoryManager>();
        userRepoMock = new Mock<IUserRepository>();
        accRepoMock = new Mock<IAccommodationRepository>();

        repoManagerMock.Setup(r => r.UserRepository).Returns(userRepoMock.Object);
        repoManagerMock.Setup(r => r.AccommodationRepository).Returns(accRepoMock.Object);

        service = new AccommodationService.Service.Implementation.AccommodationService(repoManagerMock.Object);
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

        accRepoMock.Setup(r => r.ExistsAsync(a => a.Name == "TestAcc")).ReturnsAsync(false);
        userRepoMock.Setup(r => r.GetByUsernameAsync("john")).ReturnsAsync(user);

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
}