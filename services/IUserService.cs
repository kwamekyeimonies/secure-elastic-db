using AcceralytDevTest.dtos;

namespace AcceralytDevTest.services;

public interface IUserService
{
    Task<SignUpResponse> SignUp(SignUpRequest request);
    Task<SignInResponse> SignIn(SignInRequest request);
}