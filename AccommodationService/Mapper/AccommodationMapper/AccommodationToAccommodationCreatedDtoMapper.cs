using AccommodationService.Model.Messages;

namespace AccommodationService.Mapper.AccommodationMapper;

public class AccommodationToAccommodationCreatedDtoMapper(
    ) : BaseMapper<Accommodation, AccommodationCreatedDto>
{
    public override async Task<AccommodationCreatedDto> Map(Accommodation source)
    {
        return new AccommodationCreatedDto()
        {
            Id = source.ExternalId,
            PriceType = source.PriceType,
            Owner = source.Owner.Username
        };
    }
}