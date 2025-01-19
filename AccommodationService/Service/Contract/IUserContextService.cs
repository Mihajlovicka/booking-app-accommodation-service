using AccommodationService.Model.Entity;

namespace AccommodationService.Service.Contract;

public interface IUserContextService
{
    Task<User> GetCurrentUserAsync();
}