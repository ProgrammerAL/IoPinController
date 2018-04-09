using System;
using System.Threading.Tasks;
using IoPinController;
using IoPinController.CommonComponents;
using IoPinController.FileUtils;
using IoPinController.PinControllers.Linux;
using IoPinController.Utils;

namespace IoPinControllerPractice
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Using same example as what's described at: 
            // http://hertaville.com/introduction-to-accessing-the-raspberry-pis-gpio-in-c.html

            var taskSchedulerUtility = new TaskSchedulerUtility();
            var fileUtils = new AsyncFileUtil();
            var consoleLogger = new ConsoleLogger
            {
                IsLoggingErrors = true,
                IsLoggingInfo = true
            };

            using (var controller = new LinuxPinController(fileUtils, consoleLogger, taskSchedulerUtility))
            {
                var buttonInputPin = controller.GetOrCreateInputPin(17);
                var button = new Button(buttonInputPin);
                button.StateChanged += Button_StateChanged;

                try
                {
                    var ledPin = controller.GetOrCreateOutputPin(4);
                    controller.StartContinuouslyCheckingInputPins();

                    for (int i = 0; i < 1000; i++)
                    {
                        //LED stuff stopped worked for some reason. Not sure why. Might need to make sure file is closed first?
                        if (i % 2 == 0)
                        {
                            await ledPin.SetOutputModeAsync(OutputModeType.High);
                        }
                        else
                        {
                            await ledPin.SetOutputModeAsync(OutputModeType.Low);
                        }

                        System.Threading.Thread.Sleep(500);
                    }

                    Console.WriteLine("Done...");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Caught Exception: {ex.Message}");
                    Console.WriteLine(Environment.NewLine);
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private static void Button_StateChanged(Button sender, ButtonStateType state)
        {
            Console.WriteLine($"Button value: {state}");
        }
    }
}
