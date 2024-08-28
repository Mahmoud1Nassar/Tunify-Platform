using Microsoft.AspNetCore.Identity;
using Tunify_Platform.Repositories.Interfaces;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class IdentityAccountService : IAccount
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IConfiguration _configuration;
    public IdentityAccountService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    public async Task<bool> RegisterUserAsync(RegisterDto registerDto)
    {
        var user = new IdentityUser { UserName = registerDto.Username, Email = registerDto.Email };
        var result = await _userManager.CreateAsync(user, registerDto.Password);
        return result.Succeeded;
    }

    public async Task<bool> LoginUserAsync(LoginDto loginDto)
    {
        var result = await _signInManager.PasswordSignInAsync(loginDto.Username, loginDto.Password, loginDto.RememberMe, false);
        return result.Succeeded;
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<string> GenerateJwtToken(IdentityUser user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);
        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
