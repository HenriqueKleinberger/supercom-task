namespace SupercomTask.Models
{
    public class ExpirationPublished
    {
        public int Id { get; set; }
        public Card Card { get; set; }
        public DateTime PublishedAt { get; set; }
    }
}
