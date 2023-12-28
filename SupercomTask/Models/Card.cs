namespace SupercomTask.Models
{
    public class Card
    {
        public int CardId { get; set; }

        public String Title { get; set; }

        public String Description { get; set; }

        public DateTime Deadline { get; set; }

        public DateTime CreatedAt { get; set; }

        public Status Status { get; set; }
    }
}
