using System;
using System.Collections.Generic;
using System.Text;

namespace IoPinController.Utils
{
    public interface IIoPinControllerLogger
    {
        bool IsLoggingErrors { get; set; }
        bool IsLoggingInfo { get; set; }

        void LogError(string message);
        void LogInfo(string message);
    }
}
