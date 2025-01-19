using AccommodationService.Model.Dto;
using AccommodationService.Model.Entity;

namespace AccommodationService.Mapper;

public class MapperManager(
    IBaseMapper<EquipmentDto, Equipment> equipmentDtoToEquipmentMapper,
    IBaseMapper<CreateAccommodationDto, Accommodation> createAccommodationDtoToAccommodationMapper,
    IBaseMapper<AddressDto, Address> addressDtoToAddressMapper
    ) : IMapperManager
{
    public IBaseMapper<EquipmentDto, Equipment> EquipmentDtoToEquipmentMapper { get; } =
        equipmentDtoToEquipmentMapper;

    public IBaseMapper<CreateAccommodationDto, Accommodation> CreateAccommodationDtoToAccommodationMapper { get; } =
        createAccommodationDtoToAccommodationMapper;

    public IBaseMapper<AddressDto, Address> AddressDtoToAddressMapper { get; } = addressDtoToAddressMapper;
}