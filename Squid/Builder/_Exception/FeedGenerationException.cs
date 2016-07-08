using System;
//+
namespace Squid
{
    [Serializable]
    public class FeedGenerationException : ApplicationException
    {
        //- @Ctor -//
        public FeedGenerationException() { }

        //- @Ctor -//
        public FeedGenerationException(String message) : base(message) { }

        //- @Ctor -//
        public FeedGenerationException(String message, Exception inner) : base(message, inner) { }

        //- #Ctor -//
        protected FeedGenerationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}