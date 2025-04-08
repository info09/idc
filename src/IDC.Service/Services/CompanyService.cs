using IDC.Application.Dto.Company;
using IDC.Application.Services.Interfaces;
using IDC.Domain.Data.Company;
using IDC.Domain.Data.Identity;
using IDC.Domain.Exceptions;
using IDC.Domain.SeedWorks;
using IDC.Shared.SeedWorks;
using Microsoft.AspNetCore.Identity;

namespace IDC.Application.Services;

public class CompanyService : ICompanyService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;

    public CompanyService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task<ApiResult<bool>> CreateCompany(CreateCompanyRequest request)
    {
        // Check if company already exists
        var existingCompany = await _unitOfWork.CompanyRepository.HasCompany(request.Name);
        if (existingCompany)
        {
            throw new IDCException("Company with this email already exists.");
        }
        var company = new Company()
        {
            Name = request.Name,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Address = request.Address,
            CompanyType = CompanyType.Trial,
        };
        _unitOfWork.CompanyRepository.Add(company);

        // Create User
        //var user = new AppUser()
        //{
        //    FirstName = "Admin",
        //    LastName = company.Name,
        //    Email = request.Email,
        //    NormalizedEmail = request.Email.ToUpper(),
        //    UserName = request.Email,
        //    DateCreated = DateTime.UtcNow,
        //    CompanyId = company.Id,
        //    IsActive = true
        //};
        //user.PasswordHash = new PasswordHasher<AppUser>().HashPassword(user, "Admin@123$");

        //await _userManager.CreateAsync(user, "Admin@123$");

        await _unitOfWork.CompleteAsync();

        return new ApiSuccessResult<bool>(200, true);
    }
}
