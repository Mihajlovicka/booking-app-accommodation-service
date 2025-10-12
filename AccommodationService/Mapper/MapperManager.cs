using AccommodationService.Model.Dto;
using AccommodationService.Model.Entity;
using AccommodationService.Model.Messages;

namespace AccommodationService.Mapper;

public class MapperManager(
    IBaseMapper<EquipmentDto, Equipment> equipmentDtoToEquipmentMapper,
    IBaseMapper<CreateAccommodationDto, Accommodation> createAccommodationDtoToAccommodationMapper,
    IBaseMapper<AddressDto, Address> addressDtoToAddressMapper,
    IBaseMapper<UserDto, User> userDtoToUserMapper) : IMapperManager
{
    public IBaseMapper<EquipmentDto, Equipment> EquipmentDtoToEquipmentMapper { get; } =
        equipmentDtoToEquipmentMapper;

    public IBaseMapper<CreateAccommodationDto, Accommodation> CreateAccommodationDtoToAccommodationMapper { get; } =
        createAccommodationDtoToAccommodationMapper;

    public IBaseMapper<AddressDto, Address> AddressDtoToAddressMapper { get; } = addressDtoToAddressMapper;
    public IBaseMapper<UserDto, User> UserDtoToUserMapper { get; } = userDtoToUserMapper;
}