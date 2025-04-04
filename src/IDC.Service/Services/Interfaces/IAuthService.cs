using IDC.Application.Dto.Auth;

namespace IDC.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthenticatedResult> Login(LoginRequest request);
        Task<AuthenticatedResult> RefreshToken(TokenRequest request);
    }
}
