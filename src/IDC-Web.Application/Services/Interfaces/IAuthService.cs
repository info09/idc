using IDC.Application.Dto.Auth;
using IDC.Shared.SeedWorks;

namespace IDC.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResult<AuthenticatedResult>> Login(LoginRequest request);
        Task<ApiResult<AuthenticatedResult>> RefreshToken(TokenRequest request);
    }
}
