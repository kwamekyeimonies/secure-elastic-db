using AcceralytDevTest.common;
using AcceralytDevTest.dtos;
using AcceralytDevTest.models;
using AcceralytDevTest.repository;
using AcceralytDevTest.utils;

namespace AcceralytDevTest.services;

public class UserService:IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHandler _passwordHandler;
    private readonly ITokenService _tokenService;
    private ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository, IPasswordHandler passwordHandler, ITokenService tokenService, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _passwordHandler = passwordHandler;
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task<SignUpResponse> SignUp(SignUpRequest request)
    {
        try
        {
           var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
           if (existingUser != null)
           {
               _logger.LogError("Email {Email} already exists", request.Email);
               return new SignUpResponse { Message = "Email already exists" };
           }

           existingUser = await _userRepository.GetUserByPhoneNumberAsync(request.PhoneNumber);
           if (existingUser != null)
           {
               _logger.LogError("PhoneNumber {PhoneNumber} already exists", request.PhoneNumber);
               return new SignUpResponse { Message = "Phone number already exists" };
           }
           existingUser = await _userRepository.GetUserByPhoneNumberAsync(request.Username);
           if (existingUser != null)
           {
               _logger.LogError("Username {Username} already exists", request.Username);
               return new SignUpResponse { Message = "Username already exists" };
           }
           var hashedPassword = _passwordHandler.HashPassword(request.Password);
           
           var newUser = new User
           {
               Id = Guid.NewGuid(),
               Name = request.Name,
               Email = request.Email,
               Password = hashedPassword,
               Birthday = request.Birthday,
               Address = request.Address,
               City = request.City,
               PhoneNumber = request.PhoneNumber,
               Role = UserRoles.User,
               CreatedAt = DateTime.UtcNow,
               UpdatedAt = DateTime.UtcNow
           };
           
           var response = await _userRepository.CreateUserAsync(newUser);
           
           _logger.LogInformation("User {Username} added", request.Username);
           return new SignUpResponse { Message = "User added", User = newUser };

        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw new Exception($"Unable to sign up user {request.Username}");
        }
    }

    public async Task<SignInResponse> SignIn(SignInRequest request)
    {
        try
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
            if (existingUser == null)
            {
                _logger.LogWarning($"User with email {request.Email} not found.");
                return new SignInResponse
                {
                    Message = "Invalid email or password."
                };
            }

            var isPasswordValid = _passwordHandler.VerifyHashedPassword(request.Password, existingUser.Password);
            if (!isPasswordValid)
            {
                _logger.LogWarning($"Incorrect password for email {request.Email}.");
                return new SignInResponse
                {
                    Message = "Invalid email or password."
                };
            }

            var token = _tokenService.GenerateToken(existingUser);

            _logger.LogInformation($"User {request.Email} successfully signed in.");

            return new SignInResponse
            {
                UserId = existingUser.Id.ToString(),
                Message = "Sign-in successful.",
                AccessToken = token,
                Username = existingUser.UserName,
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw new Exception("Unable to login into account");
        }
    }
}