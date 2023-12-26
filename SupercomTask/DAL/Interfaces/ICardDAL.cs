using SupercomTask.Models;

namespace SupercomTask.DAL.Interfaces
{
    public interface ICardDAL
    {
        public Task<Card> InsertCard(Card card);
        public Task<Card> UpdateCard(Card card, int cardId);
        public Task DeleteCard(int cardId);
        public Task<Card> GetCard(int cardId);
        public Task<List<Card>> GetCards();
    }
}
