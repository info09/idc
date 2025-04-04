namespace IDC.Domain.SeedWorks;

public interface IUnitOfWork
{
    Task<int> CompleteAsync();
}
