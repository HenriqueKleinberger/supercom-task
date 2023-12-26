using SupercomTask.DTO;
using SupercomTask.Models;

namespace SupercomTask.Mappers
{
    public static class CardDTOMapper
    {
        public static Card ToCard(this CardDTO cardDTO, Status status)
        {
            
            Card card = new Card()
            {
                CreatedAt = cardDTO.CreatedAt,
                DeadLine = cardDTO.DeadLine,
                Description = cardDTO.Description,
                Title = cardDTO.Title,
                Status = status
            };
            return card;
            
        }
    }

}
