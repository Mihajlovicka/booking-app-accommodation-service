using AccommodationService.Model.Entity;

namespace AccommodationService.Model.Messages;

public class AccommodationCreatedDto
{
    public Guid Id { get; set; }
    public PriceType PriceType { get; set; }
    public string Owner { get; set; }
}