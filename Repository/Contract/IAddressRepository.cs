using AccommodationService.Model.Entity;

namespace AccommodationService.Repository.Contract;

public interface IAddressRepository
{
    Task<Address?> GetByIdAsync(int id);
    Task<Address?> GetByProperties(Address address);
}