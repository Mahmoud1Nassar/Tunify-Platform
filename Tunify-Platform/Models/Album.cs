﻿namespace Tunify_Platform.Models
{
    public class Album
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ArtistId { get; set; }
        public Artist Artist { get; set; }

        public ICollection<Song> Songs { get; set; }
    }
}
