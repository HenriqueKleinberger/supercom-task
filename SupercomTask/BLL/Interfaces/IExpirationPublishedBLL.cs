using SupercomTask.DTO;

namespace SupercomTask.BLL.Interfaces
{
    public interface IExpirationPublishedBLL
    {
        public Task PublishExpiration(CardDTO cardDTO);
    }
}
