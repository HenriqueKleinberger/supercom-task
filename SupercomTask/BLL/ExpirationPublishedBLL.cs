using SupercomTask.BLL.Interfaces;
using SupercomTask.DAL.Interfaces;
using SupercomTask.DTO;
using SupercomTask.Models;
namespace SupercomTask.BLL
{
    public class ExpirationPublishedBLL : IExpirationPublishedBLL
    {
        private readonly ILogger<ExpirationPublishedBLL> _logger;
        private readonly ICardDAL _cardDAL;
        private readonly IExpirationPublishedDAL _expirationPublishedDAL;


        public ExpirationPublishedBLL(ILogger<ExpirationPublishedBLL> logger, ICardDAL cardDAL, IExpirationPublishedDAL expirationPublishedDAL)
        {
            _logger = logger;
            _cardDAL = cardDAL;
            _expirationPublishedDAL = expirationPublishedDAL;
        }

        public async Task PublishExpiration(CardDTO cardDTO)
        {
            Card? card = await _cardDAL.GetCard(cardDTO.Id);
            if(card != null)
            {
                bool alreadyPublished = await _expirationPublishedDAL.hasBeenPublished(card.CardId);
                if(!alreadyPublished)
                {
                    ExpirationPublished expirationPublished = new ExpirationPublished()
                    {
                        Card = card,
                        PublishedAt = DateTime.UtcNow
                    };
                    await _expirationPublishedDAL.publishExpirationCard(expirationPublished);
                    _logger.LogInformation($"Hi your Task is due {card.Title}");
                    await _expirationPublishedDAL.SaveChangesAsync();
                } else
                {
                    _logger.LogInformation($"Message already published to LOG: {card.Title}");
                }
            }
        }
    }
}
