using Microsoft.AspNetCore.Identity;

public interface IAccount
{
    Task<bool> RegisterUserAsync(RegisterDto registerDto);
    Task<bool> LoginUserAsync(LoginDto loginDto);
    Task LogoutAsync();
    Task<string> GenerateJwtToken(IdentityUser user);
}
