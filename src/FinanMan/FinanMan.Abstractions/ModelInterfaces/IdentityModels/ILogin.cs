namespace FinanMan.Abstractions.ModelInterfaces.IdentityModels;

public interface ILogin
{
    string? Username { get; set; }
    string? Password { get; set; }
    bool RemeberMe { get; set; }

}
