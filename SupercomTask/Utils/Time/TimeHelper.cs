using SupercomTask.Utils.Time.Interfaces;

namespace SupercomTask.Utils.Time
{
    public class TimeHelper : ITimeHelper
    {
        public DateTime Now()
        {
            return DateTime.Now;
        }
    }
}
