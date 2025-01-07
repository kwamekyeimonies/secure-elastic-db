using AcceralytDevTest.models;

namespace AcceralytDevTest.utils;

public interface ITokenService
{
    string GenerateToken(User user);

}