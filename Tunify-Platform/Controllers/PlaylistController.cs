using Microsoft.AspNetCore.Mvc;
using Tunify_Platform.Repositories.Interfaces;
using Tunify_Platform.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Tunify_Platform.Controllers
{
    [Authorize] // Secures all actions in this controller
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistController : ControllerBase
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly ISongRepository _songRepository;

        public PlaylistController(IPlaylistRepository playlistRepository, ISongRepository songRepository)
        {
            _playlistRepository = playlistRepository;
            _songRepository = songRepository;
        }

        [Authorize]
        [HttpPost("{playlistId}/songs/{songId}")]
        public async Task<IActionResult> AddSongToPlaylist(int playlistId, int songId)
        {
            var playlist = await _playlistRepository.GetPlaylistByIdAsync(playlistId);
            if (playlist == null) return NotFound();

            var song = await _songRepository.GetSongByIdAsync(songId);
            if (song == null) return NotFound();

            playlist.PlaylistSongs.Add(new PlaylistSongs { PlaylistId = playlistId, SongId = songId });
            await _playlistRepository.UpdatePlaylistAsync(playlist);

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{playlistId}")]
        public async Task<IActionResult> DeletePlaylist(int playlistId)
        {
            var playlist = await _playlistRepository.GetPlaylistByIdAsync(playlistId);
            if (playlist == null) return NotFound();

            await _playlistRepository.DeletePlaylistAsync(playlistId);
            return NoContent();
        }

        [HttpGet("{playlistId}/songs")]
        public async Task<IActionResult> GetSongsForPlaylist(int playlistId)
        {
            var playlist = await _playlistRepository.GetPlaylistByIdAsync(playlistId);
            if (playlist == null) return NotFound();

            var songs = playlist.PlaylistSongs.Select(ps => ps.Song).ToList();
            return Ok(songs);
        }
    }
}
