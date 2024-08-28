using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tunify_Platform.Repositories.Interfaces;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccount _accountService;
    private readonly UserManager<IdentityUser> _userManager;

    public AccountController(IAccount accountService, UserManager<IdentityUser> userManager)
    {
        _accountService = accountService;
        _userManager = userManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _accountService.RegisterUserAsync(registerDto);

        if (!result)
            return BadRequest("User registration failed.");

        return Ok("User registered successfully.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _accountService.LoginUserAsync(loginDto);

        if (!result)
            return Unauthorized("Invalid login attempt.");

        // Find the user by username
        var user = await _userManager.FindByNameAsync(loginDto.Username);
        if (user == null)
        {
            return Unauthorized("Invalid login attempt.");
        }

        // Generate the JWT token
        var token = await _accountService.GenerateJwtToken(user);

        return Ok(new { Token = token });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _accountService.LogoutAsync();
        return Ok("User logged out successfully.");
    }
}
