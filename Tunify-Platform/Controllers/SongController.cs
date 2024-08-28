using Microsoft.AspNetCore.Mvc;
using Tunify_Platform.Repositories.Interfaces;
using Tunify_Platform.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Tunify_Platform.Controllers
{
    [Authorize] // Secures all actions in this controller
    [Route("api/[controller]")]
    [ApiController]
    public class SongController : ControllerBase
    {
        private readonly ISongRepository _songRepository;

        public SongController(ISongRepository songRepository)
        {
            _songRepository = songRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetSongs()
        {
            var songs = await _songRepository.GetAllSongsAsync();
            return Ok(songs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSong(int id)
        {
            var song = await _songRepository.GetSongByIdAsync(id);
            if (song == null)
            {
                return NotFound();
            }
            return Ok(song);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateSong([FromBody] Song song)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _songRepository.AddSongAsync(song);
            return CreatedAtAction(nameof(GetSong), new { id = song.Id }, song);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSong(int id, [FromBody] Song song)
        {
            if (id != song.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingSong = await _songRepository.GetSongByIdAsync(id);
            if (existingSong == null)
            {
                return NotFound();
            }

            await _songRepository.UpdateSongAsync(song);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSong(int id)
        {
            var song = await _songRepository.GetSongByIdAsync(id);
            if (song == null)
            {
                return NotFound();
            }

            await _songRepository.DeleteSongAsync(id);
            return NoContent();
        }
    }
}
