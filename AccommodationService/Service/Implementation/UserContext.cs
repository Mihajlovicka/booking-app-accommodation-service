using System.Security.Claims;
using AccommodationService.Service.Contract;

namespace AccommodationService.Service.Implementation;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public string? Name => httpContextAccessor.HttpContext?.User?.Claims
        .FirstOrDefault(c => c.Type == "name")?.Value ?? httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
}
