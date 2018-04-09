using System;
using System.Threading.Tasks;
using IoPinController.FileUtils;
using IoPinController.Utils;

namespace IoPinController
{
    public abstract class Pin : IDisposable
    {
        protected Pin(int number, IAsyncFileUtil fileUtils, IIoPinControllerLogger logger)
        {
            Number = number;
            NumberText = number.ToString();
            FileUtils = fileUtils;
            Logger = logger;
            Initialize();
        }

        public int Number { get; }
        public string NumberText { get; }
        public IAsyncFileUtil FileUtils { get; }
        public IIoPinControllerLogger Logger { get; }
        public abstract PinDirectionType PinDirection { get; }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                Task.Run(OnDisposeAsync);
            }
        }

        protected abstract Task OnDisposeAsync();
        protected abstract void Initialize();
    }
}
