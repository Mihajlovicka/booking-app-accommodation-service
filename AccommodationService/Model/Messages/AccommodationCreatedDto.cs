using AccommodationService.Model.Dto;
using AccommodationService.Model.Entity;

namespace AccommodationService.Model.Messages;

public class AccommodationCreatedDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public PriceType PriceType { get; set; }
    public AddressDto Address { get; set; }
    public string Owner { get; set; }
    public int? MinNumberOfGuests { get; set; }
    public int? MaxNumberOfGuests { get; set; }
    public List<string> Pictures { get; set; }
}