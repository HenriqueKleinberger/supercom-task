using SupercomTask.Models;

namespace SupercomTask.DTO
{
    public class CardDTO
    {
        public int Id { get; set; }
        public String Title { get; set; }

        public String Description { get; set; }

        public DateTime DeadLine { get; set; }

        public DateTime CreatedAt { get; set; }

        public String Status { get; set; }
    }
}
