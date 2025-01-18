using AccommodationService.Repository.Contract;
using AccommodationService.Service.Contract;

namespace AccommodationService.Service.Implementation;

public class AccommodationService(
        IRepositoryManager repositoryManager
    ) : IAccommodationService
{
    public async Task Save(Accommodation accommodation)
    {
        if (!(await IsNameUniqueAsync(accommodation.Name))) throw new Exception("Accommodation name must be unique");
        await repositoryManager.AccommodationRepository.AddAsync(accommodation);
    }
    
    private async Task<bool> IsNameUniqueAsync(string name)
    {
        return !await repositoryManager.AccommodationRepository.ExistsAsync(a => a.Name == name);
    }

}