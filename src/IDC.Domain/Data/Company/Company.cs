namespace IDC.Domain.Data.Company;

public class Company
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string Address { get; set; } = default!;
    public CompanyType CompanyType { get; set; } = CompanyType.None;
    public DateTime DateCreated { get; set; }
}

public enum CompanyType
{
    None = 0,
    Trial = 1,
    Basic = 2,
    Pro = 3
}
