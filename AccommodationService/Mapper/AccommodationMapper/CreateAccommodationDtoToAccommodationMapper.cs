using AccommodationService.Model.Dto;
using AccommodationService.Model.Entity;
using AccommodationService.Service.Contract;

namespace AccommodationService.Mapper.AccommodationMapper;

public class CreateAccommodationDtoToAccommodationMapper(
    IBaseMapper<EquipmentDto, Equipment> equipmentDtoToEquipmentMapper,
    IBaseMapper<AddressDto, Address> addressDtoToAddressMapper,
    IUserContextService userContextService
    ) : BaseMapper<CreateAccommodationDto, Accommodation>
{
    public override async Task<Accommodation> Map(CreateAccommodationDto source)
    {
        return new Accommodation()
        {
            Name = source.Name,
            Description = source.Description,
            MinNumberOfGuests = source.MinNumberOfGuests,
            MaxNumberOfGuests = source.MaxNumberOfGuests,
            Active = true,
            Address = await addressDtoToAddressMapper.Map(source.Address),
            Equipment = (await Task.WhenAll(
                source.Equipment.Select(equipmentDtoToEquipmentMapper.Map)
            )).ToList(),
            //pics
            Pictures = source.PictureUrls.Select(url => new Picture { Url = url }).ToList(),
            Owner = await userContextService.GetCurrentUserAsync()
        };
    }
}