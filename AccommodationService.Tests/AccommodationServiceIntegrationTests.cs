using AccommodationService.Controllers;
using AccommodationService.Model.Dto;
using AccommodationService.Service.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using AccommodationService.Mapper;
using AccommodationService.Model.Entity;

namespace AccommodationService.Tests
{
    public class AccommodationControllerTests
    {
        private Mock<IEquipmentService> _mockEquipmentService;
        private Mock<IAccommodationService> _mockAccommodationService;
        private Mock<IMapperManager> _mockMapperManager;
        private Mock<IUserContext> _userContext;
        private AccommodationController _controller;

        [SetUp]
        public void Setup()
        {
            _mockEquipmentService = new Mock<IEquipmentService>();
            _mockAccommodationService = new Mock<IAccommodationService>();
            _mockMapperManager = new Mock<IMapperManager>();
            _userContext = new Mock<IUserContext>();

            _controller = new AccommodationController(
                _mockEquipmentService.Object,
                _mockAccommodationService.Object,
                _mockMapperManager.Object,
                _userContext.Object
            );

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _controller.HttpContext.Items["name"] = "testuser";
        }

        [Test]
        public async Task SaveAccommodation_FullDto_ReturnsOk()
        {
            var dto = new CreateAccommodationDto
            {
                Name = "Test Apartment",
                Description = "Beautiful apartment in city center",
                MinNumberOfGuests = 1,
                MaxNumberOfGuests = 4,
                Address = new AddressDto
                {
                    City = "Belgrade",
                    Country = "Serbia",
                    StreetName = "Kralja Petra",
                    StreetNumber = "12",
                    PostNumber = "11000"
                },
                Equipment = new List<EquipmentDto>
                {
                    new() { Name = "WiFi" },
                    new() { Name = "TV" }
                },
                PictureUrls = []
            };

            var mappedEntity = new Accommodation
            {
                Name = dto.Name,
                Description = dto.Description,
                MinNumberOfGuests = dto.MinNumberOfGuests,
                MaxNumberOfGuests = dto.MaxNumberOfGuests,
                Address = new Address
                {
                    City = dto.Address.City,
                    Country = dto.Address.Country,
                    StreetName = dto.Address.StreetName,
                    StreetNumber = dto.Address.StreetNumber,
                    PostNumber = dto.Address.PostNumber
                },
                Equipment = dto.Equipment
                    .Select(e => new Equipment { Name = e.Name })
                    .ToList()
            };

            var mapperMock = new Mock<IBaseMapper<CreateAccommodationDto, Accommodation>>();
            mapperMock.Setup(m => m.Map(dto)).ReturnsAsync(mappedEntity);
            _mockMapperManager.Setup(x => x.CreateAccommodationDtoToAccommodationMapper).Returns(mapperMock.Object);

            _userContext.Setup(u => u.Name).Returns("testuser");

            _mockAccommodationService.Setup(x => x.Save(mappedEntity, "testuser"))
                .Returns(Task.CompletedTask);

            var result = await _controller.Save(dto);

            if (result is OkResult okResult && !Equals(okResult.StatusCode, 200))
            {
                NUnit.Framework.Assert.Fail($"Expected 200 but was {okResult.StatusCode}");
            }

            _mockAccommodationService.Verify(x => x.Save(mappedEntity, "testuser"), Times.Once);
        }

        [Test]
        public async Task SaveAccommodation_MapperThrows_ReturnsInternalServerError()
        {
            var dto = new CreateAccommodationDto { Name = "Test Apartment" };

            var mapperMock = new Mock<IBaseMapper<CreateAccommodationDto, Accommodation>>();
            mapperMock.Setup(m => m.Map(dto)).ThrowsAsync(new System.Exception("Mapper error"));
            _mockMapperManager.Setup(x => x.CreateAccommodationDtoToAccommodationMapper).Returns(mapperMock.Object);

            var result = await _controller.Save(dto);

            var objectResult = result as ObjectResult;
            if (objectResult != null && !Equals(objectResult.StatusCode, 500))
                NUnit.Framework.Assert.Fail($"Expected 500 but was {objectResult.StatusCode}");

            var message = objectResult?.Value?.GetType().GetProperty("Message")?.GetValue(objectResult.Value)
                ?.ToString();

            if (!Equals(message, "Mapper error"))
            {
                NUnit.Framework.Assert.Fail($"Expected message 'Mapper error' but was '{message}'");
            }

        }

        [Test]
        public async Task SaveAccommodation_ServiceThrows_ReturnsInternalServerError()
        {
            var dto = new CreateAccommodationDto { Name = "Test Apartment" };
            var mappedEntity = new Accommodation { Name = dto.Name };

            var mapperMock = new Mock<IBaseMapper<CreateAccommodationDto, Accommodation>>();
            mapperMock.Setup(m => m.Map(dto)).ReturnsAsync(mappedEntity);
            _mockMapperManager.Setup(x => x.CreateAccommodationDtoToAccommodationMapper).Returns(mapperMock.Object);

            _mockAccommodationService.Setup(x => x.Save(mappedEntity, "testuser"))
                .ThrowsAsync(new System.Exception("Service error"));

            _userContext.Setup(u => u.Name).Returns("testuser");

            var result = await _controller.Save(dto);

            var objectResult = result as ObjectResult;
            if (objectResult != null && !Equals(objectResult.StatusCode, 500))
                Assert.Fail($"Expected 500 but was {objectResult.StatusCode}");

            var message = objectResult?.Value?.GetType().GetProperty("Message")?.GetValue(objectResult.Value)
                ?.ToString();

            if (!Equals(message, "Service error"))
                Assert.Fail($"Expected message 'Service error' but was '{message}'");
        }

        [Test]
        public async Task GetAllEquipment_ReturnsOkWithData()
        {
            var equipmentList = new List<Equipment>
            {
                new() { Id = 11, Name = "WiFi" },
                new() { Id = 12, Name = "TV" }
            };

            _mockEquipmentService.Setup(x => x.GetAll()).ReturnsAsync(equipmentList);

            var result = await _controller.GetAllEquipment();

            if (result is not OkObjectResult okResultTemp || okResultTemp.StatusCode != 200)
            {
                Assert.Fail($"Expected 200 but was {(result as ObjectResult)?.StatusCode}");
                return;
            }

            if (okResultTemp.Value is not IEnumerable<Equipment> returnedList)
            {
                Assert.Fail("Expected a list of equipment but got null");
            }
        }
        
        [Test]
        public async Task SaveAccommodation_NullDto_ReturnsInternalServerError()
        {
            CreateAccommodationDto? dto = null;

            var result = await _controller.Save(dto);

            if (result is not ObjectResult objectResultTemp || objectResultTemp.StatusCode != 500)
            {
                Assert.Fail($"Expected 500 but was {(result as ObjectResult)?.StatusCode}");
                return;
            }

            var message = objectResultTemp.Value?.GetType().GetProperty("Message")?.GetValue(objectResultTemp.Value)?.ToString();
            if (string.IsNullOrEmpty(message))
                Assert.Fail("Expected error message but got null or empty");
        }
    }
}
