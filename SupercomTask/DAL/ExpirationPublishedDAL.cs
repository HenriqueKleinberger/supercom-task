using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SupercomTask.BLL;
using SupercomTask.DAL.Interfaces;
using SupercomTask.Models;

namespace SupercomTask.DAL
{
    public class ExpirationPublishedDAL : BaseDAL, IExpirationPublishedDAL
    {
        private readonly ILogger<ExpirationPublishedDAL> _logger;

        public ExpirationPublishedDAL(SuperComTaskContext superComTaskContext, ILogger<ExpirationPublishedDAL> logger)
            : base(superComTaskContext)
        {
            _logger = logger;
        }
        
        public async Task<bool> hasBeenPublished(int cardId)
        {
            ExpirationPublished? expirationPublished = await _superComTaskContext.ExpirationPublished.Where(ep => ep.Card.CardId == cardId).FirstOrDefaultAsync();
            return expirationPublished != null;
        }

        public async Task publishExpirationCard(ExpirationPublished expirationPublished)
        {
            await _superComTaskContext.ExpirationPublished.AddAsync(expirationPublished);
            
        }
    }
}
