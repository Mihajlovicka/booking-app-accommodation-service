using AccommodationService.Model.Entity;

namespace AccommodationService.Repository.Contract;

public interface IUserRepository : ICrudRepository<User>
{
    Task<User?> GetByUsernameAsync(string username);
}