using System;

namespace Implementation.Exceptions
{
    /// <summary>
    /// Custom exceptions for this project
    /// </summary>
    public class EtlException : Exception
    {
        public EtlException(string errorMessage) : base(string.Format("Message: {0}", errorMessage))
        {
        }

        public EtlException(Type exceptiontype, string errorMessage)
            : base(string.Format("{0} exception. Message: {1}", exceptiontype.Name, errorMessage))
        {
        }
    }
}