using Microsoft.EntityFrameworkCore;
using Tunify_Platform.Data;
using Tunify_Platform.Models;
using Tunify_Platform.Repositories.Services;
using Xunit;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TunifyPlatformTests
{
    public class PlaylistServiceTests
    {
        private TunifyDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<TunifyDbContext>()
                          .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Use a unique database for each test
                          .Options;

            var context = new TunifyDbContext(options);

            // Seed initial data if necessary
            context.Artists.Add(new Artist { Id = 1, Name = "Artist A" });
            context.Playlists.Add(new Playlist { Id = 1, Name = "Playlist A", UserId = 1 });
            context.Songs.Add(new Song { Id = 1, Title = "Song A", ArtistId = 1, AlbumId = 1 });

            context.SaveChanges();
            return context;
        }

        [Fact]
        public async Task AddSongToPlaylist_AddsSongSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new PlaylistRepository(context);

            // Act
            await service.AddSongToPlaylistAsync(1, 1);

            // Assert
            var playlist = await context.Playlists.Include(p => p.PlaylistSongs).ThenInclude(ps => ps.Song).FirstOrDefaultAsync(p => p.Id == 1);
            Assert.NotNull(playlist);
            Assert.Single(playlist.PlaylistSongs);
            Assert.Equal(1, playlist.PlaylistSongs.First().SongId);
        }

        [Fact]
        public async Task AddSongToArtist_AddsSongSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new ArtistRepository(context);

            // Act
            await service.AddSongToArtistAsync(1, 1);

            // Assert
            var song = await context.Songs.FirstOrDefaultAsync(s => s.Id == 1);
            Assert.NotNull(song);
            Assert.Equal(1, song.ArtistId);
        }
    }
}
