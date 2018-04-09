using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using IoPinController.FileUtils;
using IoPinController.Utils;

namespace IoPinController.PinControllers.Linux
{
    public class LinuxOutputPin : OutputPin
    {
        private const string OutputDirectionValue = "out";
        private const string OutputLowValue = "0";
        private const string OutputHighValue = "1";
        private const string UnexportFilePath = "/sys/class/gpio/unexport";

        private readonly string _outputModeFilePath;

        public LinuxOutputPin(int number, IAsyncFileUtil fileUtils, IIoPinControllerLogger logger) : base(number, fileUtils, logger)
        {
            _outputModeFilePath = $"/sys/class/gpio/gpio{this.NumberText}/value";
        }

        protected override void Initialize()
        {
            //First check if the pin has already been exported
            if (FileUtils.DirectoryExists($"/sys/class/gpio/gpio{this.NumberText}"))
            {
                Logger.LogInfo($"GPIO {NumberText} already being exported. Will not export it.");
            }
            else
            {
                var exportFilePath = "/sys/class/gpio/export";
                FileUtils.AppendText(exportFilePath, NumberText);
            }

            var directionFilePath = $"/sys/class/gpio/gpio{this.NumberText}/direction";
            FileUtils.AppendText(directionFilePath, OutputDirectionValue);
        }

        protected override async Task OnDisposeAsync()
        {
            await SetOutputModeAsync(OutputModeType.Low);
            await UnexportPinAsync();
        }

        protected override async Task OnSetOutputModeAsync(OutputModeType outputMode)
        {
            var outputValue = OutputMode == OutputModeType.Low ? OutputLowValue : OutputHighValue;
            await FileUtils.AppendTextAsync(_outputModeFilePath, outputValue);
        }

        private async Task UnexportPinAsync()
        {
            await FileUtils.AppendTextAsync(UnexportFilePath, NumberText);
        }
    }
}
