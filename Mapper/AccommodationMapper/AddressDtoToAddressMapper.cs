using AccommodationService.Model.Dto;
using AccommodationService.Model.Entity;
using AccommodationService.Repository.Contract;

namespace AccommodationService.Mapper.AccommodationMapper;

public class AddressDtoToAddressMapper(
    IRepositoryManager repositoryManager
    )
    : BaseMapper<AddressDto, Address>
{
    public override async Task<Address> Map(AddressDto source)
    {
        var address = new Address();
        if(source.Id is not null)
        {
            address = await repositoryManager.AddressRepository.GetByIdAsync((int)source.Id);
        }
        
        UpdateEntityFromDto(source, address!);

        var existingAddress = await repositoryManager.AddressRepository.GetByProperties(address!);

        return existingAddress ?? address!;
    }

    private static void UpdateEntityFromDto(AddressDto source, Address destination)
    {
        destination.StreetNumber = source.StreetNumber;
        destination.StreetName = source.StreetName;
        destination.City = source.City;
        destination.Country = source.Country;
        destination.PostNumber = source.PostNumber;
    }
}