namespace Tunify_Platform.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }

        // Navigation property
        public ICollection<User> Users { get; set; }
    }
}
