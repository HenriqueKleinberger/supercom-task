using SupercomTask.DAL.Interfaces;
using SupercomTask.Models;
namespace SupercomTask.DAL
{
    public class BaseDAL : IBaseDAL
    {
        protected readonly SuperComTaskContext _superComTaskContext;

        public BaseDAL(SuperComTaskContext superComTaskContext)
        {
            _superComTaskContext = superComTaskContext;
        }

        public async Task SaveChangesAsync()
        {
            await _superComTaskContext.SaveChangesAsync();
        }
    }
}
