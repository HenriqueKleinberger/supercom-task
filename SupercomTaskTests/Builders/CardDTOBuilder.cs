using SupercomTask.DTO;
using SupercomTask.Models;

namespace SupercomTaskTests.Builders
{
    internal class CardDTOBuilder
    {
        private CardDTO _cardDTO;
        
        public CardDTOBuilder()
        {
            _cardDTO = new CardDTO();
            _cardDTO.DeadLine = DateTime.Now;
            _cardDTO.Status = "To Do";
            _cardDTO.CreatedAt = DateTime.Now;
            _cardDTO.Description = "Card Description";
            _cardDTO.Title = "Card Title";
        }

        public CardDTOBuilder WithTitle(string title)
        {
            _cardDTO.Title = title;
            return this;
        }

        public CardDTOBuilder WithStatus(string status)
        {
            _cardDTO.Status = status;
            return this;
        }

        public CardDTOBuilder WithDescription(string description)
        {
            _cardDTO.Description = description;
            return this;
        }

        public CardDTOBuilder WithDeadLine(DateTime deadline)
        {
            _cardDTO.DeadLine = deadline;
            return this;
        }

        public CardDTO Build()
        {
            return _cardDTO;
        }
    }
}
