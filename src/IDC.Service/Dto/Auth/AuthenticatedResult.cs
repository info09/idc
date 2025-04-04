namespace IDC.Application.Dto.Auth;

public class AuthenticatedResult
{
    public required string Token { get; set; }
    public required string RefreshToken { get; set; }
}
