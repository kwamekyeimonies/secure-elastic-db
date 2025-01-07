using AcceralytDevTest.models;

namespace AcceralytDevTest.repository;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> GetUserByIdAsync(Guid id);
    Task<User> GetUserByEmailAsync(string email);
    Task<User> GetUserByUserNameAsync(string username);
    Task<User> CreateUserAsync(User user);
    Task<User> GetUserByPhoneNumberAsync(string phoneNumber);
    Task<User> UpdateUserAsync(User user);
    Task<string> DeleteUserAsync(User user);
    
}