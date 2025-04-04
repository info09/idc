using IDC.Domain.Data.Company;
using IDC.Domain.Repositories;
using IDC.Infrastructure.Data;
using IDC.Infrastructure.SeedWorks;

namespace IDC.Infrastructure.Repositories;

public class CompanyRepository : RepositoryBase<Company, Guid>, ICompanyRepository
{
    public CompanyRepository(ApplicationDbContext context) : base(context)
    {
    }
}
