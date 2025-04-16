using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IDC.Domain.Data.Identity;

[Table("AppRoles")]
public class AppRole : IdentityRole<Guid>
{
    [Required]
    [MaxLength(200)]
    public required string DisplayName { get; set; }
}
