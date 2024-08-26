public interface IAccount
{
    Task<bool> RegisterUserAsync(RegisterDto registerDto);
    Task<bool> LoginUserAsync(LoginDto loginDto);
    Task LogoutAsync();
}
