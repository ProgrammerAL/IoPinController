using System;
using System.Collections.Generic;
using System.Text;

namespace IoPinController.Utils
{
    public class ConsoleLogger : IIoPinControllerLogger
    {
        public bool IsLoggingErrors { get; set; }
        public bool IsLoggingInfo { get; set; }

        public void LogError(Func<string> messageFunc)
        {
            if (IsLoggingErrors)
            {
                Console.WriteLine("Error: " + messageFunc());
            }
        }

        public void LogInfo(Func<string> messageFunc)
        {
            if (IsLoggingInfo)
            {
                Console.WriteLine("Info: " + messageFunc());
            }
        }
    }
}
