using AcceralytDevTest.models;

namespace AcceralytDevTest.dtos;

public class SignUpResponse
{
    public User User { get; set; }
    public string Message { get; set; }
}