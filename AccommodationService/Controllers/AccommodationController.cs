using AccommodationService.Mapper;
using AccommodationService.Model.Dto;
using AccommodationService.Service.Contract;
using Microsoft.AspNetCore.Mvc;

namespace AccommodationService.Controllers;

[Route("api/accommodations")]
[ApiController]
public class AccommodationController(
    IEquipmentService equipmentService,
    IAccommodationService accommodationService,
    IMapperManager mapperManager
    ) : ControllerBase
{
    [HttpGet("equipment")]
    public async Task<IActionResult> GetAllEquipment()
    {
        try
        {
            var response = await equipmentService.GetAll();
            return Ok(response);
        }
        catch (Exception exception)
        {
            var errorResponse = new
            {
                exception.Message
            };

            return StatusCode(500, errorResponse);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Save([FromBody] CreateAccommodationDto accommodation)
    {
        try
        {
            await accommodationService.Save(
                await mapperManager.CreateAccommodationDtoToAccommodationMapper.Map(accommodation));

            return Ok();
        }
        catch (Exception exception)
        {
             var errorResponse = new
            {
                exception.Message
            };

            return StatusCode(500, errorResponse);
        }
    }
}