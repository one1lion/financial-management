using FinanMan.Abstractions.ModelInterfaces.IdentityModels;
using System.ComponentModel.DataAnnotations;

namespace FinanMan.Shared.IdentityModels;

public class LoginModel : ILogin
{
    [Required]
    public string? Username { get; set; }
    [Required]
    public string? Password { get; set; }
    public bool RemeberMe { get; set; }
}
