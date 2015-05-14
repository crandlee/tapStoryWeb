using System;

namespace tapStoryWebApi.Exceptions
{
    public class DataNotFoundException: ApplicationException
    {
        public DataNotFoundException(string message) : base(message)
        {
            
        }
    }
}