using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Text;
using IoPinController.FileUtils;
using IoPinController.Utils;

namespace IoPinController.PinControllers.Linux
{
    public class LinuxPinController : PinController<LinuxInputPin, LinuxOutputPin>
    {
        public LinuxPinController(IAsyncFileUtil fileUtils, IIoPinControllerLogger logger, ITaskSchedulerUtility taskSchedulerUtility)
            : base((pinNumber) => new LinuxInputPin(pinNumber, fileUtils, logger),
                  (pinNumber) => new LinuxOutputPin(pinNumber, fileUtils, logger),
                  taskSchedulerUtility)
        {
        }

        protected override void DisposePins(IEnumerable<LinuxInputPin> inputPins, IEnumerable<LinuxOutputPin> outputPins)
        {
            foreach (var pin in inputPins)
            {
                pin.Dispose();
            }

            foreach (var pin in outputPins)
            {
                pin.Dispose();
            }
        }
    }
}
