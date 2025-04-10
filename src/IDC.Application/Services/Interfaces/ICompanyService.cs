using IDC.Application.Dto.Company;
using IDC.Shared.SeedWorks;

namespace IDC.Application.Services.Interfaces;

public interface ICompanyService
{
    Task<ApiResult<bool>> CreateCompany(CreateCompanyRequest request);
    Task<ApiResult<List<CompanyDto>>> GetCompanys();
}
