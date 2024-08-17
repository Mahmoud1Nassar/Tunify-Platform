using Tunify_Platform.Models;
using Tunify_Platform.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tunify_Platform.Data;

namespace Tunify_Platform.Repositories.Services
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly TunifyDbContext _context;

        public PlaylistRepository(TunifyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Playlist>> GetAllPlaylistsAsync()
        {
            return await _context.Playlists.Include(p => p.PlaylistSongs).ThenInclude(ps => ps.Song).ToListAsync();
        }

        public async Task<Playlist> GetPlaylistByIdAsync(int id)
        {
            return await _context.Playlists.Include(p => p.PlaylistSongs).ThenInclude(ps => ps.Song).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddPlaylistAsync(Playlist playlist)
        {
            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePlaylistAsync(Playlist playlist)
        {
            _context.Playlists.Update(playlist);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePlaylistAsync(int id)
        {
            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist != null)
            {
                _context.Playlists.Remove(playlist);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddSongToPlaylistAsync(int playlistId, int songId)
        {
            var playlist = await _context.Playlists.Include(p => p.PlaylistSongs).FirstOrDefaultAsync(p => p.Id == playlistId);
            if (playlist != null)
            {
                playlist.PlaylistSongs.Add(new PlaylistSongs { PlaylistId = playlistId, SongId = songId });
                await _context.SaveChangesAsync();
            }
        }
    }
}
