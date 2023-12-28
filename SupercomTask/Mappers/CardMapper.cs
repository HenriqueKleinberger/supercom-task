using SupercomTask.DTO;
using SupercomTask.Models;

namespace SupercomTask.Mappers
{
    public static class CardMapper
    {
        public static CardDTO ToCardDTO(this Card card)
        {
            
            CardDTO carDTO = new CardDTO()
            {
                Id = card.CardId,
                CreatedAt = card.CreatedAt,
                Deadline = card.Deadline,
                Description = card.Description,
                Title = card.Title,
                Status = card.Status.Name
            };
            return carDTO;
        }
    }

}
