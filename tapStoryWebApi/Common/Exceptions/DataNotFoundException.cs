using System;

namespace tapStoryWebApi.Common.Exceptions
{
    public class DataNotFoundException: ApplicationException
    {
        public DataNotFoundException(string message) : base(message)
        {
            
        }
    }
}