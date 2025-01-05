namespace AccommodationService.Model.Dto;

public class CreateAccommodationDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public AddressDto Address { get; set; }
    public IList<EquipmentDto> Equipment { get; set; }
    public int? MinNumberOfGuests { get; set; }
    public int? MaxNumberOfGuests { get; set; }
    public List<string> PictureUrls { get; set; } 
}