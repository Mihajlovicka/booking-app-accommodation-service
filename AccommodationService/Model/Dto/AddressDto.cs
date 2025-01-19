namespace AccommodationService.Model.Dto;

public class AddressDto
{
    public int? Id { get; set; }
    public string StreetNumber { get; set; }
    public string StreetName { get; set; }
    public string City { get; set; }
    public string PostNumber { get; set; }
    public string Country { get; set; }
}