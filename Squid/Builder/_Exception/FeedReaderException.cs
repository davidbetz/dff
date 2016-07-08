using System;
//+
namespace Squid
{
    [Serializable]
    public class FeedReaderException : ApplicationException
    {
        //- @Ctor -//
        public FeedReaderException() { }

        //- @Ctor -//
        public FeedReaderException(String message) : base(message) { }

        //- @Ctor -//
        public FeedReaderException(String message, Exception inner) : base(message, inner) { }

        //- #Ctor -//
        protected FeedReaderException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}