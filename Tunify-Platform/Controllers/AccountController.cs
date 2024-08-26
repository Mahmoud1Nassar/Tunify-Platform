using Microsoft.AspNetCore.Mvc;
using Tunify_Platform.Repositories.Interfaces;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccount _accountService;

    public AccountController(IAccount accountService)
    {
        _accountService = accountService;
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

        return Ok("User logged in successfully.");
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _accountService.LogoutAsync();
        return Ok("User logged out successfully.");
    }
}
