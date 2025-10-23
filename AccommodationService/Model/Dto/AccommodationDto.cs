namespace AccommodationService.Model.Dto;

public class AccommodationDto
{
    public string Id { get; set; } //external
    public string Name { get; set; }
    public string Description { get; set; }
    public string Owner { get; set; }
    public int? MinNumberOfGuests { get; set; }
    public int? MaxNumberOfGuests { get; set; }
    public AddressDto Address { get; set; }
    public List<EquipmentDto> Equipments { get; set; }
    public List<string> Pictures { get; set; }
    public string? PriceType { get; set; }
}