using System;

namespace Tek.Common
{
    public static class ExceptionExtension
    {
        public static string GetMessages(this Exception exception)
        {
            if (exception == null)
            {
                return string.Empty;
            }

            var messages = exception.Message;

            var currentException = exception.InnerException;

            var depth = 1;

            while (currentException != null)
            {
                messages += Environment.NewLine 
                    + new string('-', depth * 2)
                    + $" (Level {depth}) {currentException.Message}";

                currentException = currentException.InnerException;

                depth++;
            }

            return messages;
        }
    }
}
