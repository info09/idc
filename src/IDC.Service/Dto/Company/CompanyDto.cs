using IDC.Domain.Data.Company;

namespace IDC.Application.Dto.Company;

public class CompanyDto
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string Address { get; set; } = default!;
    public CompanyType CompanyType { get; set; } = CompanyType.None;
}
