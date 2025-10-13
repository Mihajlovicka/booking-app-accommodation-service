using AccommodationService.Model.Dto;
using AccommodationService.Model.Entity;

namespace AccommodationService.Mapper.AccommodationMapper;

public class AccommodationToAccommodationDtoMapper(
    IBaseMapper<EquipmentDto, Equipment> equipmentDtoToEquipmentMapper,
    IBaseMapper<AddressDto, Address> addressDtoToAddressMapper
    ) : BaseMapper<Accommodation, AccommodationDto>
{
    public override async Task<AccommodationDto> Map(Accommodation source)
    {
        return new AccommodationDto()
        {
            Name = source.Name,
            Description = source.Description,
            MinNumberOfGuests = source.MinNumberOfGuests,
            MaxNumberOfGuests = source.MaxNumberOfGuests,
            Id = source.ExternalId.ToString(),
            PriceType = source.PriceType.ToString(),
            Address = addressDtoToAddressMapper.ReverseMap(source.Address),
            Equipments = source.Equipment.Select(equipmentDtoToEquipmentMapper.ReverseMap).ToList(),
            Pictures = source.Pictures.Select(picture => picture.Url).ToList(),
        };
    }
}