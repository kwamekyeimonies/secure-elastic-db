using AcceralytDevTest.dtos;
using AcceralytDevTest.services;
using Microsoft.AspNetCore.Mvc;

namespace AcceralytDevTest.controllers;

[ApiController]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;

    public UserController(ILogger<UserController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUpController([FromBody] SignUpRequest signUpRequest)
    {
        try
        {
            var response = await _userService.SignUp(signUpRequest);
            return Ok(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw new Exception("Something went wrong");
        }
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> SignInController([FromBody] SignInRequest signInRequest)
    {
        try
        {
            var response = await _userService.SignIn(signInRequest);
            return Ok(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw new Exception("Something went wrong");
        }
    }
}