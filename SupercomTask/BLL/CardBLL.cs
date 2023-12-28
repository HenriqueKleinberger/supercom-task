using SupercomTask.BLL.Interfaces;
using SupercomTask.Constants;
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
        public async Task DeleteCard(int cardId)
        {
            await _cardDAL.DeleteCard(cardId);
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
            card.CreatedAt = _timeHelper.Now();

            Card insertedCard = await _cardDAL.InsertCard(card);

            return insertedCard.ToCardDTO();
        }

        public async Task<CardDTO> UpdateCard(CardDTO cardDTO, int cardId)
        {
            Status? status = await _statusDAL.GetStatusByName(cardDTO.Status);
            ValidateStatus(status);
            
            Card? cardToUpdate = await _cardDAL.GetCard(cardId);
            ValidateCard(cardToUpdate);
            ValidateUpdateDeadline(cardToUpdate, cardDTO);
            cardToUpdate.Status = status;
            
            Card cardUpdated = await _cardDAL.UpdateCard(cardToUpdate, cardDTO);

            return cardUpdated.ToCardDTO();
        }

        public async Task<List<CardDTO>> GetExpiredUndoneCards()
        {
            List<Card> cards = await _cardDAL.GetExpiredUndoneCards();
            return cards.ConvertAll(c => c.ToCardDTO());
        }

        private void ValidateStatus(Status? status)
        {
            
            if (status == null)
            {
                throw new ValidationException(ErrorMessages.INVALID_STATUS);
            }
        }

        private void ValidateCard(Card? card)
        {
            if (card == null)
            {
                throw new ValidationException(ErrorMessages.CARD_NOT_FOUND);

            }
        }

        private void ValidateUpdateDeadline(Card card, CardDTO cardDTO)
        {
            if (card.CreatedAt.Date > cardDTO.Deadline.Date)
            {
                throw new ValidationException(ErrorMessages.DEADLINE_VALIDATION);

            }
        }
    }
}
