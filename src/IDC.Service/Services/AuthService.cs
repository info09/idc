using IDC.Application.Dto.Auth;
using IDC.Application.Services.Interfaces;
using IDC.Domain.Data.Identity;
using IDC.Domain.Exceptions;
using IDC.Shared.Constants;
using IDC.Shared.SeedWorks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IDC.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly RoleManager<AppRole> _roleManager;

        public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _roleManager = roleManager;
        }

        public async Task<ApiResult<AuthenticatedResult>> Login(LoginRequest request)
        {
            if(request == null)
                return new ApiErrorResult<AuthenticatedResult>(500, "Invalid Request");

            var user = await _userManager.Users.FirstOrDefaultAsync(i => i.UserName == request.UserName);
            if (user == null || user.IsActive == false || user.LockoutEnabled) throw new IDCException("Đăng nhập không đúng");

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, true);
            if (!result.Succeeded) throw new IDCException("Đăng nhập không đúng");

            var roles = await _userManager.GetRolesAsync(user);
            //var permissions = await this.GetPermissionsByUserIdAsync(user.Id.ToString());
            var claims = new[]
            {
                    new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                    new Claim(UserClaims.Id, user.Id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.UserName!),
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(UserClaims.FirstName, user.FirstName),
                    new Claim(UserClaims.Roles, string.Join(";", roles)),
                    //new Claim(UserClaims.Permissions, JsonSerializer.Serialize(permissions)),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(UserClaims.Avatar, !string.IsNullOrEmpty(user?.Avatar) ? user.Avatar : "")
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user!.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(30);

            await _userManager.UpdateAsync(user);

            return new ApiSuccessResult<AuthenticatedResult>(200, new AuthenticatedResult()
            {
                Token = accessToken,
                RefreshToken = refreshToken
            });
        }

        public async Task<ApiResult<AuthenticatedResult>> RefreshToken(TokenRequest request)
        {
            if (request == null) 
                throw new IDCException("Invalid request");

            var principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);

            if (principal == null || principal.Identity == null || principal.Identity.Name == null)
                throw new IDCException("Invalid token");

            var user = await _userManager.Users.FirstOrDefaultAsync(i => i.UserName == principal.Identity.Name);

            if (request.RefreshToken != user?.RefreshToken)
                throw new IDCException("Invalid token");

            if (user.RefreshTokenExpiryTime < DateTime.UtcNow)
                throw new IDCException("Invalid token");

            if (user == null || user.IsActive == false || user.LockoutEnabled)
                throw new IDCException("Invalid token");

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(30);

            await _userManager.UpdateAsync(user);
            return new ApiSuccessResult<AuthenticatedResult>(200, new AuthenticatedResult()
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
    }
}
