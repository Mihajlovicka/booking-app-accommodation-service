using AccommodationService.Model.Entity;
using AccommodationService.Service.Contract;
using Microsoft.AspNetCore.Identity;

namespace AccommodationService.Service.Implementation;

public class UserContextService(
    UserManager<User> userManager,
    IHttpContextAccessor httpContextAccessor
    ) : IUserContextService
{
    public async Task<User> GetCurrentUserAsync()
    {
        var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext!.User);
        if (user == null)
            throw new UnauthorizedAccessException("User not found");
        return user;
    }
}