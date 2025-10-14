using AccommodationService.Mapper;
using AccommodationService.Model.Dto;
using AccommodationService.Model.Entity;
using AccommodationService.Repository.Contract;
using AccommodationService.Service.Contract;
using AccommodationService.Service.MessagingService;

namespace AccommodationService.Service.Implementation;

public class AccommodationService(
        IRepositoryManager repositoryManager,
        IUserContext userContext,
        IMapperManager mapper,
        ProducerService producerService
    ) : IAccommodationService
{
    public async Task Save(Accommodation accommodation, string name)
    {
        if (!(await IsNameUniqueAsync(accommodation.Name))) throw new Exception("Accommodation name must be unique");
        var user = await repositoryManager.UserRepository.GetByUsernameAsync(name);
        if (user is null)
        {
            throw new Exception("User not found.");
        }

        accommodation.OwnerId = user.Id;

        await repositoryManager.AccommodationRepository.AddAsync(accommodation);
        var accommodationDto = await mapper.AccommodationToAccommodationCreatedDtoMapper.Map(accommodation);
        _ = producerService.ProduceAsync(KafkaTopic.AccommodationCreated.ToString(), accommodationDto);
    }

    private async Task<bool> IsNameUniqueAsync(string name)
    {
        return !await repositoryManager.AccommodationRepository.ExistsAsync(a => a.Name == name);
    }

    public async Task<IEnumerable<AccommodationDto>> GetAllByUser()
    {
        var user = await repositoryManager.UserRepository.GetByUsernameAsync(userContext.Name);
        if (user is null)
        {
            throw new Exception("User not found.");
        }

        var accommodations = await repositoryManager.AccommodationRepository.GetAllByOwnerIdAsync(user.Id);
        var mapped = await Task.WhenAll(accommodations.Select(a => mapper.AccommodationToAccommodationDtoMapper.Map(a)));
        return mapped;
    }

    public async Task UpdatePriceType(string accommodationId, UpdatePriceTypeDto dto)
    {
        var accommodation = await repositoryManager.AccommodationRepository.GetByExternalIdAsync(accommodationId);
        if (accommodation == null)
            throw new KeyNotFoundException("Accommodation not found.");

        accommodation.PriceType = Enum.Parse<PriceType>(dto.PriceType, ignoreCase: true);
        await repositoryManager.AccommodationRepository.UpdateAsync(accommodation);
        var accommodationDto = mapper.AccommodationToAccommodationCreatedDtoMapper.Map(accommodation);
        _ = producerService.ProduceAsync(KafkaTopic.AccommodationCreated.ToString(), accommodationDto);
    }

    public async Task<AccommodationDto> GetById(string accommodationId)
    {
        var accommodation = await repositoryManager.AccommodationRepository.GetByExternalIdAsync(accommodationId);

        if (accommodation is null) throw new Exception("Accommodation does not exist!");

        return await mapper.AccommodationToAccommodationDtoMapper.Map(accommodation);
    }
}