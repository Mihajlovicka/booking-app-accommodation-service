using AccommodationService.Model.Dto;

namespace AccommodationService.Service.Contract;

public interface IAccommodationService
{
    Task Save(Accommodation accommodation, string name);
    Task<IEnumerable<AccommodationDto>> GetAllByUser();
    Task UpdatePriceType(string accommodationId, UpdatePriceTypeDto priceType);
    Task<AccommodationDto> GetById(string accommodationId);
}