using AccommodationService.Model.Dto;
using AccommodationService.Model.Entity;

namespace AccommodationService.Mapper.AccommodationMapper;

public class CreateAccommodationDtoToAccommodationMapper(
    IBaseMapper<EquipmentDto, Equipment> equipmentDtoToEquipmentMapper,
    IBaseMapper<AddressDto, Address> addressDtoToAddressMapper
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
            Pictures = source.PictureUrls.Select(url => new Picture { Url = url }).ToList(),
        };
    }
}