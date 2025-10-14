using AccommodationService.Mapper;
using AccommodationService.Model.Dto;
using AccommodationService.Service.Contract;
using AccommodationService.Service.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccommodationService.Controllers;

[Authorize]
[Route("api/accommodations")]
[ApiController]
public class AccommodationController(
    IEquipmentService equipmentService,
    IAccommodationService accommodationService,
    IMapperManager mapperManager,
    IUserContext userContext
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

    [Authorize(Roles = "HOST")]
    [HttpPost]
    public async Task<IActionResult> Save([FromBody] CreateAccommodationDto accommodation)
    {
        try
        {

            await accommodationService.Save(
                await mapperManager.CreateAccommodationDtoToAccommodationMapper.Map(accommodation), userContext.Name);

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

    [Authorize(Roles = "HOST")]
    [HttpGet("")]
    public async Task<IActionResult> GetAllByUser()
    {
        return Ok(await accommodationService.GetAllByUser());
    }

    [Authorize(Roles = "HOST")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var accommodations = await accommodationService.GetAllByUser();
        var accommodation = accommodations.FirstOrDefault(a => a.Id == id);
        return Ok(accommodation);
    }

    [Authorize(Roles = "HOST")]
    [HttpPatch("{id}/price-type")]
    public async Task<IActionResult> UpdatePriceType(string id, [FromBody] UpdatePriceTypeDto dto)
    {
        await accommodationService.UpdatePriceType(id, dto);
        return NoContent();
    }
}