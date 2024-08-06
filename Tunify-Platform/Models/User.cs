namespace Tunify_Platform.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        // Foreign key for Subscription
        public int SubscriptionId { get; set; }
        public Subscription Subscription { get; set; }

        // Navigation property
        public ICollection<Playlist> Playlists { get; set; }
    }
}
