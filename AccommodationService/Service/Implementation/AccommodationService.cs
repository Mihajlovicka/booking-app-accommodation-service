using AccommodationService.Repository.Contract;
using AccommodationService.Service.Contract;

namespace AccommodationService.Service.Implementation;

public class AccommodationService(
        IRepositoryManager repositoryManager
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
    }
    
    private async Task<bool> IsNameUniqueAsync(string name)
    {
        return !await repositoryManager.AccommodationRepository.ExistsAsync(a => a.Name == name);
    }

}