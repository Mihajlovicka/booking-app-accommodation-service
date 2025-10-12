using AccommodationService.Data;
using AccommodationService.Model.Entity;
using AccommodationService.Repository.Contract;
using Microsoft.EntityFrameworkCore;

namespace AccommodationService.Repository.Implementation;

public class UserRepository(AppDbContext context) : CrudRepository<User>(context), IUserRepository
{
    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Username.Equals(username));
    }
}