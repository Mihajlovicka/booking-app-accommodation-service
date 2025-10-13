using AccommodationService.Model.Dto;
using AccommodationService.Model.Entity;
using AccommodationService.Model.Messages;

namespace AccommodationService.Mapper;

public interface IMapperManager
{
    IBaseMapper<EquipmentDto, Equipment> EquipmentDtoToEquipmentMapper { get; }
    IBaseMapper<CreateAccommodationDto, Accommodation> CreateAccommodationDtoToAccommodationMapper { get; }
    IBaseMapper<AddressDto, Address> AddressDtoToAddressMapper { get; }
    IBaseMapper<UserDto, User> UserDtoToUserMapper { get; }
    IBaseMapper<Accommodation, AccommodationDto> AccommodationToAccommodationDtoMapper { get; }
    IBaseMapper<Accommodation, AccommodationCreatedDto> AccommodationToAccommodationCreatedDtoMapper  { get; }
}