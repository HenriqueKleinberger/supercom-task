using Microsoft.EntityFrameworkCore;
using SupercomTask.BLL;
using SupercomTask.DAL.Interfaces;
using SupercomTask.Models;

namespace SupercomTask.DAL
{
    public class StatusDAL : IStatusDAL
    {
        private readonly ILogger<StatusDAL> _logger;
        private readonly SuperComTaskContext _superComTaskContext;

        public StatusDAL(ILogger<StatusDAL> logger, SuperComTaskContext superComTaskContext)
        {
            _logger = logger;
            _superComTaskContext = superComTaskContext;
        }
        public async Task<List<Status>> GetAllStatus()
        {
            List<Status> status = await _superComTaskContext.Status.ToListAsync();
            return status;
        }

        public async Task<Status> GetStatusByName(string name)
        {
            Status? status = await _superComTaskContext.Status.Where(s => s.Name == name).FirstOrDefaultAsync();
            return status;
        }
    }
}
