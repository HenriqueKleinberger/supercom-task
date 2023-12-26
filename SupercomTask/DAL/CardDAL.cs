using SupercomTask.DAL.Interfaces;
using SupercomTask.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;

namespace SupercomTask.DAL
{
    public class CardDAL : ICardDAL
    {
        private readonly ILogger<CardDAL> _logger;
        private readonly SuperComTaskContext _superComTaskContext;

        public CardDAL(ILogger<CardDAL> logger, SuperComTaskContext superComTaskContext)
        {
            _logger = logger;
            _superComTaskContext = superComTaskContext;
        }
        public async Task DeleteCard(int cardId)
        {
            throw new NotImplementedException();
        }

        public async Task<Card> GetCard(int cardId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Card>> GetCards()
        {
            return await _superComTaskContext.Cards
                .Include(c => c.Status)
                .OrderBy(c => c.CardId)
                .ToListAsync();
        }

        public async Task<Card> InsertCard(Card card)
        {
            EntityEntry<Card> entry = await _superComTaskContext.Cards.AddAsync(card);
            await _superComTaskContext.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<Card> UpdateCard(Card card, int cardId)
        {
            throw new NotImplementedException();
        }
    }
}
