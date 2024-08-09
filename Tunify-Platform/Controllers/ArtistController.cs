using Microsoft.AspNetCore.Mvc;
using Tunify_Platform.Repositories.Interfaces;
using Tunify_Platform.Models;
using System.Threading.Tasks;

namespace Tunify_Platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistController : ControllerBase
    {
        private readonly IArtistRepository _artistRepository;

        public ArtistController(IArtistRepository artistRepository)
        {
            _artistRepository = artistRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetArtists()
        {
            var artists = await _artistRepository.GetAllArtistsAsync();
            return Ok(artists);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetArtist(int id)
        {
            var artist = await _artistRepository.GetArtistByIdAsync(id);
            if (artist == null)
            {
                return NotFound();
            }
            return Ok(artist);
        }

        [HttpPost]
        public async Task<IActionResult> CreateArtist([FromBody] Artist artist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _artistRepository.AddArtistAsync(artist);
            return CreatedAtAction(nameof(GetArtist), new { id = artist.Id }, artist);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateArtist(int id, [FromBody] Artist artist)
        {
            if (id != artist.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingArtist = await _artistRepository.GetArtistByIdAsync(id);
            if (existingArtist == null)
            {
                return NotFound();
            }

            await _artistRepository.UpdateArtistAsync(artist);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtist(int id)
        {
            var artist = await _artistRepository.GetArtistByIdAsync(id);
            if (artist == null)
            {
                return NotFound();
            }

            await _artistRepository.DeleteArtistAsync(id);
            return NoContent();
        }
    }
}
