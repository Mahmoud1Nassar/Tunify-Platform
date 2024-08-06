namespace Tunify_Platform.Models
{
    public class PlaylistSongs
    {
        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; }

        public int SongId { get; set; }
        public Song Song { get; set; }
    }
}
