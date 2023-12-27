using SupercomTask.Models;

namespace SupercomTask.DAL.Interfaces
{
    public interface IStatusDAL
    {
        public Task<Status?> GetStatusByName(String name);
        public Task<List<Status>> GetAllStatus();
    }
}
