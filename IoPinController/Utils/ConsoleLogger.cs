using System;
using System.Collections.Generic;
using System.Text;

namespace IoPinController.Utils
{
    public class ConsoleLogger : IIoPinControllerLogger
    {
        public bool IsLoggingErrors { get; set; }
        public bool IsLoggingInfo { get; set; }

        public void LogError(string message)
        {
            if (IsLoggingErrors)
            {
                Console.WriteLine("Error: " + message);
            }
        }

        public void LogInfo(string message)
        {
            if (IsLoggingInfo)
            {
                Console.WriteLine("Info: " + message);
            }
        }
    }
}
