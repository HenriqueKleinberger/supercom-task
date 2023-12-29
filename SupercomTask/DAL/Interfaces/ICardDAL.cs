using SupercomTask.DTO;
using SupercomTask.Models;

namespace SupercomTask.DAL.Interfaces
{
    public interface ICardDAL : IBaseDAL
    {
        public Task<Card> InsertCard(Card card);
        public Task<Card> UpdateCard(Card card, CardDTO cardDTO);
        public Task DeleteCard(int cardId);
        public Task<Card?> GetCard(int cardId);
        public Task<List<Card>> GetCards();

        Task<List<Card>> GetExpiredUndoneCards();
    }
}
