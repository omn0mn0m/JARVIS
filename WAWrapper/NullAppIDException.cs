using System;
using System.Runtime.Serialization;

namespace WAWrapper
{
    /// <summary>
    /// Description of NullAppIDException
    /// </summary>
    public class NullAppIDException : Exception, ISerializable
    {
        public NullAppIDException()
        {
        }

        public NullAppIDException(string message)
            : base(message)
        {
        }

        public NullAppIDException(string message, NullAppIDException innerException)
            : base(message, innerException)
        {
        }

        // This constructor is needed for serialization.
        protected NullAppIDException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}