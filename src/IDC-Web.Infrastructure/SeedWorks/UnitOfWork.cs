using IDC.Domain.Repositories;
using IDC.Domain.SeedWorks;
using IDC.Infrastructure.Data;
using IDC.Infrastructure.Repositories;

namespace IDC.Infrastructure.SeedWorks;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        CompanyRepository = new CompanyRepository(context);
        _context = context;
    }

    public ICompanyRepository CompanyRepository { get; private set; }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
