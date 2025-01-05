using AccommodationService.Model.Dto;
using AccommodationService.Model.Entity;

namespace AccommodationService.Mapper;

public interface IMapperManager
{
    IBaseMapper<EquipmentDto, Equipment> EquipmentDtoToEquipmentMapper { get; }
    IBaseMapper<CreateAccommodationDto, Accommodation> CreateAccommodationDtoToAccommodationMapper { get; }
    IBaseMapper<AddressDto, Address> AddressDtoToAddressMapper { get; }
}