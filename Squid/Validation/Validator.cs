using System;
//+
namespace Squid.Validation
{
    internal static class Validator
    {
        //- ~EnsureIsNotNull -//
        internal static void EnsureIsNotNull(Object @object, String message)
        {
            if (@object == null)
            {
                {
                    FaultThrower.ThrowArgumentNullException(message);
                }
            }
        }

        //- ~IsNotZero -//
        internal static void IsNotZero(Int32 number, String message)
        {
            if (number == 0)
            {
                FaultThrower.ThrowArgumentException(message);
            }
        }

        //- ~IsNotBlank -//
        internal static void IsNotBlank(String data, String message)
        {
            if (String.IsNullOrEmpty(data))
            {
                FaultThrower.ThrowArgumentException(message);
            }
        }
    }
}