using SupercomTask.DTO;
using SupercomTask.Models;

namespace SupercomTask.BLL.Interfaces
{
    public interface ICardBLL
    {
        public Task<CardDTO> InsertCard(CardDTO cardDTO);
        public Task<CardDTO> UpdateCard(CardDTO cardDTO, int cardId);
        public Task DeleteCard(int cardId);
        public Task<CardDTO> GetCard(int cardId);
        public Task<List<CardDTO>> GetCards();
    }
}
