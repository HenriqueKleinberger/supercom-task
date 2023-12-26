using SupercomTask.Constants;

namespace SupercomTask.Exceptions
{
    public class InvalidStatusException : Exception
    {
        public InvalidStatusException()
            : base(ErrorMessages.INVALID_STATUS)
        {
        }
    }
}
