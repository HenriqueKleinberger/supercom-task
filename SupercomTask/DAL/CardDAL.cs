using SupercomTask.DAL.Interfaces;
using SupercomTask.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using SupercomTask.DTO;
using System.Net.NetworkInformation;

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
            Card? cardToDelete = await _superComTaskContext.Cards.FindAsync(cardId);
            if (cardToDelete != null)
            {
                _superComTaskContext.Cards.Remove(cardToDelete);
                await _superComTaskContext.SaveChangesAsync();
            }
        }

        public async Task<Card?> GetCard(int cardId)
        {
            Card? card = await _superComTaskContext.Cards
                .Include(c => c.Status)
                .Where(c => c.CardId == cardId)
                .FirstOrDefaultAsync();
            return card;
        }

        public async Task<List<Card>> GetCards()
        {
            return await _superComTaskContext.Cards
                .Include(c => c.Status)
                .OrderBy(c => c.CardId)
                .ToListAsync();
        }

        public async Task<List<Card>> GetExpiredUndoneCards()
        {
            return await _superComTaskContext.Cards
                .Include(c => c.Status)
                .Where(c => c.Deadline.Date <  DateTime.UtcNow.Date && c.Status.Name != "Done")
                .OrderBy(c => c.CardId)
                .ToListAsync();
        }

        public async Task<Card> InsertCard(Card card)
        {
            EntityEntry<Card> entry = await _superComTaskContext.Cards.AddAsync(card);
            await _superComTaskContext.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<Card> UpdateCard(Card card, CardDTO cardDTO)
        {
            card.Title = cardDTO.Title;
            card.Description = cardDTO.Description;
            card.Deadline = cardDTO.Deadline;

            await _superComTaskContext.SaveChangesAsync();
            return card;
        }
    }
}
