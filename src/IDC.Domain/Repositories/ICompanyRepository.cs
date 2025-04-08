using IDC.Domain.Data.Company;
using IDC.Domain.SeedWorks;

namespace IDC.Domain.Repositories;

public interface ICompanyRepository : IRepositoryBase<Company, Guid>
{
    Task<bool> HasCompany(string name);
}
