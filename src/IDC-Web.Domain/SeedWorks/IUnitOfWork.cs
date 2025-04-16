using IDC.Domain.Repositories;

namespace IDC.Domain.SeedWorks;

public interface IUnitOfWork
{
    ICompanyRepository CompanyRepository { get; }
    Task<int> CompleteAsync();
}
