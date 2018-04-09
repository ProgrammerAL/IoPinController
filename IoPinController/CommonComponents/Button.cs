using System;
using System.Collections.Generic;
using System.Text;

namespace IoPinController.CommonComponents
{
    public class Button : IDisposable
    {
        private ButtonStateType _state;

        public Button(InputPin inputPin)
        {
            InputPin = inputPin;
            InputPin.InputValueChanged += InputPin_InputValueChanged;
            UpdateButtonState();
        }

        public event Action<Button, ButtonStateType> StateChanged;

        public InputPin InputPin { get; }

        public ButtonStateType State
        {
            get { return _state; }
            private set
            {
                if (_state != value)
                {
                    _state = value;
                    StateChanged?.Invoke(this, _state);
                }
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            InputPin.InputValueChanged -= InputPin_InputValueChanged;
        }
        
        private void InputPin_InputValueChanged(InputPin sender, bool value)
        {
            UpdateButtonState();
        }

        private void UpdateButtonState()
        {
            State = InputPin.CurrentValue ? ButtonStateType.NotPressed : ButtonStateType.Pressed;
        }
    }
}
