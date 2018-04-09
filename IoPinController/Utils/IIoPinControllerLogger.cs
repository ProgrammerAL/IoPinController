using System;
using System.Collections.Generic;
using System.Text;

namespace IoPinController.Utils
{
    public interface IIoPinControllerLogger
    {
        bool IsLoggingErrors { get; set; }
        bool IsLoggingInfo { get; set; }

        void LogError(Func<string> messageFunc);
        void LogInfo(Func<string> messageFunc);
    }
}
