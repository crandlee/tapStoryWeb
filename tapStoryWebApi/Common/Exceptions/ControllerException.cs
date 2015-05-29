using System;

namespace tapStoryWebApi.Common.Exceptions
{
    public class ControllerException: Exception
    {
        public ControllerException()
        {
        }

        public ControllerException(string message) : base(message)
        {
            
        }

        public ControllerException(string message, Exception innerException) : base(message, innerException)
        {
            
        }

    }
}