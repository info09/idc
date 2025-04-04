using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IDC.Domain.Data.Identity;

[Table("AppUsers")]
public class AppUser : IdentityUser<Guid>
{
    [Required]
    [MaxLength(100)]
    public required string FirstName { get; set; }

    [Required]
    [MaxLength(100)]
    public required string LastName { get; set; }
    public bool IsActive { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public DateTime DateCreated { get; set; }
    [MaxLength(500)]
    public string? Avatar { get; set; }
    public DateTime? VipStartDate { get; set; }
    public DateTime? VipExpireDate { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public Guid CompanyId { get; set; }
    public string GetFullName()
    {
        return this.FirstName + " " + this.LastName;
    }
}
