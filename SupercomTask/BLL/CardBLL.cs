using SupercomTask.BLL.Interfaces;
using SupercomTask.Controllers;
using SupercomTask.DAL;
using SupercomTask.DAL.Interfaces;
using SupercomTask.DTO;
using SupercomTask.Exceptions;
using SupercomTask.Mappers;
using SupercomTask.Models;
using SupercomTask.Utils.Time.Interfaces;

namespace SupercomTask.BLL
{
    public class CardBLL : ICardBLL
    {
        private readonly ILogger<CardBLL> _logger;
        private readonly ICardDAL _cardDAL;
        private readonly IStatusDAL _statusDAL;
        private readonly ITimeHelper _timeHelper;


        public CardBLL(ILogger<CardBLL> logger, ICardDAL cardDAL, IStatusDAL statusDAL, ITimeHelper timeHelper)
        {
            _logger = logger;
            _cardDAL = cardDAL;
            _statusDAL = statusDAL;
            _timeHelper = timeHelper;
        }
        public Task DeleteCard(int cardId)
        {
            throw new NotImplementedException();
        }

        public Task<CardDTO> GetCard(int cardId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CardDTO>> GetCards()
        {
            List<Card> cards = await _cardDAL.GetCards();
            return cards.ConvertAll(c => c.ToCardDTO());
        }

        public async Task<CardDTO> InsertCard(CardDTO cardDTO)
        {
            Status status = await _statusDAL.GetStatusByName(cardDTO.Status);
            ValidateStatus(status);
            Card card = cardDTO.ToCard(status);

            cardDTO.CreatedAt = _timeHelper.Now();

            Card insertedCard = await _cardDAL.InsertCard(card);

            return insertedCard.ToCardDTO();
        }

        public Task<CardDTO> UpdateCard(CardDTO cardDTO, int cardId)
        {
            throw new NotImplementedException();
        }

        private void ValidateStatus(Status status)
        {
            
            if (status == null)
            {
                throw new InvalidStatusException();
            }
        }
    }
}
