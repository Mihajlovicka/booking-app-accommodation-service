namespace AccommodationService.Service.Contract;

public interface IAccommodationService
{
    Task Save(Accommodation accommodation);
}