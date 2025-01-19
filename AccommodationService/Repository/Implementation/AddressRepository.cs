using AccommodationService.Data;
using AccommodationService.Model.Entity;
using AccommodationService.Repository.Contract;
using Microsoft.EntityFrameworkCore;

namespace AccommodationService.Repository.Implementation;

public class AddressRepository(AppDbContext appDbContext) : IAddressRepository
{
    public async Task<Address?> GetByIdAsync(int id) => await appDbContext.Addresses.FindAsync(id);
    
    public async Task<Address?> GetByProperties(Address address)
    {
        return await appDbContext.Addresses
            .Where(a => a.StreetNumber == address.StreetNumber &&
                        a.StreetName == address.StreetName &&
                        a.City == address.City &&
                        a.PostNumber == address.PostNumber &&
                        a.Country == address.Country)
            .FirstOrDefaultAsync();
    }
}