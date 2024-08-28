using Microsoft.AspNetCore.Mvc;
using Tunify_Platform.Repositories.Interfaces;
using Tunify_Platform.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Tunify_Platform.Controllers
{
    [Authorize(Roles = "Admin")] // Only Admins can modify or delete users
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [Authorize] // All authenticated users can view users
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(users);
        }

        [Authorize] // All authenticated users can view a user by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _userRepository.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            await _userRepository.UpdateUserAsync(user);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userRepository.DeleteUserAsync(id);
            return NoContent();
        }
    }
}
