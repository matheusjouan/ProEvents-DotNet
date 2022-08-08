using ProEvents.Domain.Identity;

namespace ProEvents.Infra.Interface;

public interface IUserRepository : IBaseRepository<User>
{
    Task<IEnumerable<User>> GetUsersAsync();
    Task<User> GetUserByIdAsync(int id);
    Task<User> GetUserByUsernameAsync(string username);
}
