using AccommodationService.Model.Entity;
using AccommodationService.Model.Messages;
using AccommodationService.Repository.Contract;

namespace AccommodationService.Mapper.UserMapper;

public class UserDtoToUserMapper(IRepositoryManager repositoryManager) : BaseMapper<UserDto, User>
{
    public override async Task<User> Map(UserDto source)
    {
        var user = await repositoryManager.UserRepository.GetByUsernameAsync(source.Username);
        if (user is null)
            return new User
            {
                ExternalId = source.Id,
                Username = source.Username,
                Status = true
            };

        return user;
    }
}