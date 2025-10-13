using AccommodationService.Mapper.AccommodationMapper;
using AccommodationService.Model.Dto;
using AccommodationService.Model.Entity;
using AccommodationService.Model.Messages;

namespace AccommodationService.Mapper;

public class MapperManager(
    IBaseMapper<EquipmentDto, Equipment> equipmentDtoToEquipmentMapper,
    IBaseMapper<CreateAccommodationDto, Accommodation> createAccommodationDtoToAccommodationMapper,
    IBaseMapper<AddressDto, Address> addressDtoToAddressMapper,
    IBaseMapper<UserDto, User> userDtoToUserMapper,
    IBaseMapper<Accommodation, AccommodationCreatedDto> AccommodationToAccommodationCreatedDtoMapper) : IMapperManager
{
    public IBaseMapper<EquipmentDto, Equipment> EquipmentDtoToEquipmentMapper { get; } =
        equipmentDtoToEquipmentMapper;

    public IBaseMapper<CreateAccommodationDto, Accommodation> CreateAccommodationDtoToAccommodationMapper { get; } =
        createAccommodationDtoToAccommodationMapper;

    public IBaseMapper<AddressDto, Address> AddressDtoToAddressMapper { get; } = addressDtoToAddressMapper;
    public IBaseMapper<UserDto, User> UserDtoToUserMapper { get; } = userDtoToUserMapper;
    public IBaseMapper<Accommodation, AccommodationDto> AccommodationToAccommodationDtoMapper { get; } =
        new AccommodationToAccommodationDtoMapper(equipmentDtoToEquipmentMapper, addressDtoToAddressMapper);
    public IBaseMapper<Accommodation, AccommodationCreatedDto> AccommodationToAccommodationCreatedDtoMapper { get; } =
        AccommodationToAccommodationCreatedDtoMapper;
}