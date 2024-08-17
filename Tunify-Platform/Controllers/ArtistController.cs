using Microsoft.AspNetCore.Mvc;
using Tunify_Platform.Repositories.Interfaces;
using Tunify_Platform.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Tunify_Platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistController : ControllerBase
    {
        private readonly IArtistRepository _artistRepository;
        private readonly ISongRepository _songRepository;

        public ArtistController(IArtistRepository artistRepository, ISongRepository songRepository)
        {
            _artistRepository = artistRepository;
            _songRepository = songRepository;
        }

        [HttpPost("{artistId}/songs/{songId}")]
        public async Task<IActionResult> AddSongToArtist(int artistId, int songId)
        {
            var artist = await _artistRepository.GetArtistByIdAsync(artistId);
            if (artist == null) return NotFound();

            var song = await _songRepository.GetSongByIdAsync(songId);
            if (song == null) return NotFound();

            song.ArtistId = artistId;
            await _songRepository.UpdateSongAsync(song);

            return Ok();
        }

        [HttpGet("{artistId}/songs")]
        public async Task<IActionResult> GetSongsByArtist(int artistId)
        {
            var artist = await _artistRepository.GetArtistByIdAsync(artistId);
            if (artist == null) return NotFound();

            var songs = artist.Songs.ToList();
            return Ok(songs);
        }
    }
}
