using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using IoPinController.FileUtils;
using IoPinController.Utils;

namespace IoPinController.PinControllers.Linux
{
    public class LinuxInputPin : InputPin
    {
        private const string InputDirectionValue = "in";
        private const string ExportFilePath = "/sys/class/gpio/unexport";


        private readonly string _inputValueFilePath;

        public LinuxInputPin(int number, IAsyncFileUtil fileUtils, IIoPinControllerLogger logger) : base(number, fileUtils, logger)
        {
            var inputFileDirectory = $"/sys/class/gpio/gpio{NumberText}";
            var fileName = "value";
            _inputValueFilePath = Path.Combine(inputFileDirectory, fileName);
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
            FileUtils.AppendText(directionFilePath, InputDirectionValue);
        }

        protected override async Task OnDisposeAsync()
        {
            await UnexportPinAsync();
        }

        public override async Task<bool> GetInputValueAsync()
        {
            //There should only be one character in the file, so we can save some time here
            var inputValue = await FileUtils.ReadFirstCharacterAsync(_inputValueFilePath);

            //0 means no signal, something else means there's a signal of some kind
            switch (inputValue)
            {
                case '0':
                    return false;
                case '1':
                    return true;
                default:
                    throw new InvalidDataException($"Invalid input pin value read with character: {inputValue}");
            }
        }

        private async Task UnexportPinAsync()
        {
            await FileUtils.AppendTextAsync(ExportFilePath, NumberText);
        }
    }
}
