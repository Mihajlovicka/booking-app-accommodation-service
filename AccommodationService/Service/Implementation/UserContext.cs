using AccommodationService.Service.Contract;

namespace AccommodationService.Service.Implementation;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public string? Name => httpContextAccessor.HttpContext?.User?.Identity?.Name;
}
