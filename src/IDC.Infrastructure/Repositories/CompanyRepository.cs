using IDC.Domain.Data.Company;
using IDC.Domain.Repositories;
using IDC.Infrastructure.Data;
using IDC.Infrastructure.SeedWorks;
using Microsoft.EntityFrameworkCore;

namespace IDC.Infrastructure.Repositories;

public class CompanyRepository : RepositoryBase<Company, Guid>, ICompanyRepository
{
    public CompanyRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task<bool> HasCompany(string name)
    {
        // Check company already exists
        var isHasCompany = _context.Companies.AnyAsync(c => c.Name == name);
        return isHasCompany;
    }
}
