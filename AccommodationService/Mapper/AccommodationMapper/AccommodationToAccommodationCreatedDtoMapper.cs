using AccommodationService.Model.Dto;
using AccommodationService.Model.Entity;
using AccommodationService.Model.Messages;

namespace AccommodationService.Mapper.AccommodationMapper;

public class AccommodationToAccommodationCreatedDtoMapper(
    IBaseMapper<AddressDto, Address> addressDtoToAddressMapper
    ) : BaseMapper<Accommodation, AccommodationCreatedDto>
{
    public override async Task<AccommodationCreatedDto> Map(Accommodation source)
    {
        return new AccommodationCreatedDto()
        {
            Id = source.ExternalId,
            Name = source.Name,
            PriceType = source.PriceType,
            Owner = source.Owner.Username,
            Address = addressDtoToAddressMapper.ReverseMap(source.Address),
            MinNumberOfGuests = source.MinNumberOfGuests,
            MaxNumberOfGuests = source.MaxNumberOfGuests,
            Pictures = source.Pictures.Select(picture => picture.Url).ToList(),
        };
    }
}