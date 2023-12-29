using SupercomTask.Models;

namespace SupercomTask.DAL.Interfaces
{
    public interface IExpirationPublishedDAL : IBaseDAL
    {
        public Task<Boolean> hasBeenPublished(int cardId);
        public Task publishExpirationCard(ExpirationPublished expirationPublished);
    }
}
