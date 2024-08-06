using Microsoft.EntityFrameworkCore;
using Tunify_Platform.Models;
using System;

namespace Tunify_Platform.Data
{
    public class TunifyDbContext : DbContext
    {
        public TunifyDbContext(DbContextOptions<TunifyDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<PlaylistSongs> PlaylistSongs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Composite key for PlaylistSongs
            modelBuilder.Entity<PlaylistSongs>()
                .HasKey(ps => new { ps.PlaylistId, ps.SongId });

            // Relationships
            modelBuilder.Entity<PlaylistSongs>()
                .HasOne(ps => ps.Playlist)
                .WithMany(p => p.PlaylistSongs)
                .HasForeignKey(ps => ps.PlaylistId);

            modelBuilder.Entity<PlaylistSongs>()
                .HasOne(ps => ps.Song)
                .WithMany(s => s.PlaylistSongs)
                .HasForeignKey(ps => ps.SongId);

            // Specify precision for the Price property
            modelBuilder.Entity<Subscription>()
                .Property(s => s.Price)
                .HasPrecision(18, 2); // Adjust precision and scale as needed

            // Modify foreign key constraints to avoid cascading deletes
            modelBuilder.Entity<Song>()
                .HasOne(s => s.Album)
                .WithMany(a => a.Songs)
                .HasForeignKey(s => s.AlbumId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Song>()
                .HasOne(s => s.Artist)
                .WithMany(a => a.Songs)
                .HasForeignKey(s => s.ArtistId)
                .OnDelete(DeleteBehavior.Restrict);

            // Define the relationship between User and Subscription
            modelBuilder.Entity<User>()
                .HasOne(u => u.Subscription)
                .WithMany(s => s.Users)
                .HasForeignKey(u => u.SubscriptionId);

            // Seeding initial data
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "John Doe", Email = "john.doe@example.com", SubscriptionId = 1 }
            );

            modelBuilder.Entity<Artist>().HasData(
                new Artist { Id = 1, Name = "Artist A" }
            );

            modelBuilder.Entity<Album>().HasData(
                new Album { Id = 1, Title = "Album A", ArtistId = 1 }
            );

            modelBuilder.Entity<Song>().HasData(
                new Song { Id = 1, Title = "Song A", ArtistId = 1, AlbumId = 1, Duration = new TimeSpan(0, 3, 45) }
            );

            modelBuilder.Entity<Playlist>().HasData(
                new Playlist { Id = 1, Name = "My Playlist", UserId = 1 }
            );

            modelBuilder.Entity<PlaylistSongs>().HasData(
                new PlaylistSongs { PlaylistId = 1, SongId = 1 }
            );

            modelBuilder.Entity<Subscription>().HasData(
                new Subscription { Id = 1, Type = "Free", Price = 0.00M },
                new Subscription { Id = 2, Type = "Premium", Price = 9.99M }
            );
        }
    }
}
