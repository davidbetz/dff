using System;
using System.ServiceModel;
using Squid.Builder;
//+
namespace Squid
{
    internal static class FaultThrower
    {
        //- $Throw -//
        private static void Throw<T>(T exception) where T : Exception
        {
            throw new FaultException<T>(exception, exception.Message);
        }

        //- ~ThrowArgumentException -//
        internal static void ThrowArgumentException(String message)
        {
            Throw<ArgumentException>(new ArgumentException(message));
        }

        //- ~ThrowArgumentNullException -//
        internal static void ThrowArgumentNullException(String message)
        {
            Throw<ArgumentNullException>(new ArgumentNullException(message));
        }

        //- ~ThrowFeedGenerationException -//
        internal static void ThrowFeedGenerationException(String message)
        {
            Throw<FeedGenerationException>(new FeedGenerationException(message));
        }

        //- ~ThrowInvalidOperationException -//
        internal static void ThrowInvalidOperationException(String message)
        {
            Throw<InvalidOperationException>(new InvalidOperationException(message));
        }

        //- ~ThrowSecurityException -//
        internal static void ThrowSecurityException(String message)
        {
            Throw<System.Security.SecurityException>(new System.Security.SecurityException(message));
        }
    }
}