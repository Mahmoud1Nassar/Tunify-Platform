using Microsoft.AspNetCore.Mvc;
using Tunify_Platform.Repositories.Interfaces;
using Tunify_Platform.Models;
using System.Threading.Tasks;

namespace Tunify_Platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistController : ControllerBase
    {
        private readonly IPlaylistRepository _playlistRepository;

        public PlaylistController(IPlaylistRepository playlistRepository)
        {
            _playlistRepository = playlistRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetPlaylists()
        {
            var playlists = await _playlistRepository.GetAllPlaylistsAsync();
            return Ok(playlists);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlaylist(int id)
        {
            var playlist = await _playlistRepository.GetPlaylistByIdAsync(id);
            if (playlist == null)
            {
                return NotFound();
            }
            return Ok(playlist);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlaylist([FromBody] Playlist playlist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _playlistRepository.AddPlaylistAsync(playlist);
            return CreatedAtAction(nameof(GetPlaylist), new { id = playlist.Id }, playlist);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlaylist(int id, [FromBody] Playlist playlist)
        {
            if (id != playlist.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingPlaylist = await _playlistRepository.GetPlaylistByIdAsync(id);
            if (existingPlaylist == null)
            {
                return NotFound();
            }

            await _playlistRepository.UpdatePlaylistAsync(playlist);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlaylist(int id)
        {
            var playlist = await _playlistRepository.GetPlaylistByIdAsync(id);
            if (playlist == null)
            {
                return NotFound();
            }

            await _playlistRepository.DeletePlaylistAsync(id);
            return NoContent();
        }
    }
}
