using AcceralytDevTest.common;
using AcceralytDevTest.datasources;
using AcceralytDevTest.models;
using Microsoft.EntityFrameworkCore;

namespace AcceralytDevTest.repository;

public class UserRepository : IUserRepository
{
    private readonly PostgreSqldbContext _postgreSqldbContext;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(PostgreSqldbContext postgreSqldbContext, ILogger<UserRepository> logger)
    {
        _postgreSqldbContext = postgreSqldbContext;
        _logger = logger;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all users from PostgreSqlDB");
            return await Task.FromResult(_postgreSqldbContext.Users.ToList());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw new Exception($"Failed to fetch all users from PostgreSqlDB");
        }
    }

    public async Task<User> GetUserByIdAsync(Guid id)
    {
        try
        {
            _logger.LogInformation("Fetching user from PostgreSqlDB");
            var user = await Task.FromResult(_postgreSqldbContext.Users.FirstOrDefault(u=>u.Id == id));
            if (user == null)
            {
                _logger.LogInformation($"User with id {id} not found");
                throw new KeyNotFoundException("User with id {id} not found");
            }
            
            return user;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw new Exception($"Failed to fetch user with id {id} from PostgreSqlDB");
        }
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        try
        {
            _logger.LogInformation("Fetching user from PostgreSqlDB");
            var user = await Task.FromResult(_postgreSqldbContext.Users.FirstOrDefault(u=>u.Email == email));
            if (user == null)
            {
                _logger.LogInformation($"User with email {email} not found");
                throw new KeyNotFoundException("User with email {id} not found");
            }
            
            return user;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw new Exception($"Failed to fetch user with id {email} from PostgreSqlDB");
        }
    }
    
    public async Task<User> GetUserByUserNameAsync(string username)
    {
        try
        {
            _logger.LogInformation("Fetching user from PostgreSqlDB");
            var user = await Task.FromResult(_postgreSqldbContext.Users.FirstOrDefault(u=>u.UserName == username));
            if (user == null)
            {
                _logger.LogInformation($"User with username {username} not found");
                throw new KeyNotFoundException("User with username {username} not found");
            }
            
            return user;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw new Exception($"Failed to fetch user with username {username} from PostgreSqlDB");
        }
    }

    public async Task<User> CreateUserAsync(User user)
    {
        try
        {
            _logger.LogInformation("Creating user from PostgreSqlDB");
            user.Id = Guid.NewGuid();
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;
            if (string.IsNullOrWhiteSpace(user.Role))
            {
                user.Role = UserRoles.User;
            }
            var result = await _postgreSqldbContext.Users.AddAsync(user);
            return result.Entity;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw new Exception($"Failed to create user from PostgreSqlDB");
        }
    }

    public async Task<User> GetUserByPhoneNumberAsync(string phoneNumber)
    {
        try
        {
            _logger.LogInformation("Fetching user from PostgreSqlDB");
            var user = await Task.FromResult(_postgreSqldbContext.Users.FirstOrDefault(u=>u.PhoneNumber == phoneNumber));
            if (user == null)
            {
                _logger.LogInformation($"User with phoneNumber {phoneNumber} not found");
                throw new KeyNotFoundException($"User with phoneNumber {phoneNumber} not found");
            }
            
            return user;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw new Exception($"Failed to fetch user with phoneNumber {phoneNumber} from PostgreSqlDB");
        }
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        try
        {
            _logger.LogInformation("Updating user from PostgreSqlDB");
            var existingUser = await _postgreSqldbContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (existingUser == null)
            {
                _logger.LogInformation($"User with id {user.Id} not found");
                throw new KeyNotFoundException("User with id {user.Id} not found");
            }
            
            foreach (var property in typeof(User).GetProperties())
            {
                var newValue = property.GetValue(user);
                if (newValue != null && !string.IsNullOrWhiteSpace(newValue.ToString()))
                {
                    property.SetValue(existingUser, newValue);
                }
            }

            existingUser.UpdatedAt = DateTime.UtcNow;
            await _postgreSqldbContext.SaveChangesAsync();
            
            return existingUser;
            
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw new Exception($"Failed to update user from PostgreSqlDB");
        }
    }

    public async Task<string> DeleteUserAsync(User userPayload)
    {
        try
        {
            _logger.LogInformation("Deleting user from PostgreSqlDB");
            var user =await  _postgreSqldbContext.Users.FirstOrDefaultAsync(u => u.Id == userPayload.Id);
            if (user == null)
            {
                _logger.LogInformation($"User with id {userPayload.Id} not found");
                throw new KeyNotFoundException($"User with id {userPayload.Id} not found");
            }
            _postgreSqldbContext.Users.Remove(user);
            await _postgreSqldbContext.SaveChangesAsync();
            
            return "user account deleted successfully";
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw new Exception("Failed to delete user from PostgreSqlDB");
        }
    }
}