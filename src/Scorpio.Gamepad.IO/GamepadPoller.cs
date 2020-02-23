using J2i.Net.XInputWrapper;
using Scorpio.Gamepad.IO.Args;
using Scorpio.Gamepad.Models;
using System;

namespace Scorpio.Gamepad.IO
{
    public class GamepadPoller : IGamepadPoller
    {
        public event EventHandler<GamepadEventArgs> GamepadStateChanged;
        public event EventHandler<bool> AChanged;
        public event EventHandler<bool> BChanged;
        public event EventHandler<bool> XChanged;
        public event EventHandler<bool> YChanged;
        public event EventHandler<bool> BackChanged;
        public event EventHandler<bool> StartChanged;
        public event EventHandler<bool> DPadDownChanged;
        public event EventHandler<bool> DPadUpChanged;
        public event EventHandler<bool> DPadLeftChanged;
        public event EventHandler<bool> DPadRightChanged;

        public bool IsConnected => _controller?.IsConnected ?? false;

        private readonly XboxController _controller;
        private GamepadModel _gamepadState;
        private GamepadModel _previousState;

        public GamepadPoller(int controllerIndex) : this(controllerIndex, 50) { }

        public GamepadPoller(int controllerIndex, int updateFrequency)
        {
            _controller = XboxController.RetrieveController(controllerIndex);
            XboxController.UpdateFrequency = updateFrequency;
            StartPolling();
        }

        private void StartPolling()
        {
            _controller.StateChanged += StateChanged;
            XboxController.StartPolling();
        }

        private void StopPolling()
        {
            _controller.StateChanged -= StateChanged;
            XboxController.StopPolling();
        }

        public void Vibrate()
        {
            _controller?.Vibrate(1.0, 1.0, TimeSpan.FromSeconds(0.8));
        }

        public GamepadModel GetState() => _gamepadState;

        private void StateChanged(object sender, XboxControllerStateChangedEventArgs e)
        {
            _gamepadState = Map(e.CurrentInputState);

            var args = new GamepadEventArgs { Gamepad = _gamepadState };

            // Fire event
            GamepadStateChanged?.Invoke(this, args);
            ProcessButtonsChangeEvents(_gamepadState, _previousState);

            _previousState = _gamepadState;
        }

        private void ProcessButtonsChangeEvents(GamepadModel state, GamepadModel previousState)
        {
            if (previousState is null) return;

            if (previousState.IsAPressed != state.IsAPressed) AChanged?.Invoke(this, state.IsAPressed);
            if (previousState.IsBPressed != state.IsAPressed) BChanged?.Invoke(this, state.IsBPressed);
            if (previousState.IsXPressed != state.IsXPressed) XChanged?.Invoke(this, state.IsXPressed);
            if (previousState.IsYPressed != state.IsYPressed) YChanged?.Invoke(this, state.IsYPressed);

            if (previousState.IsBackPressed != state.IsBackPressed) BackChanged?.Invoke(this, state.IsBackPressed);
            if (previousState.IsStartPressed != state.IsStartPressed) StartChanged?.Invoke(this, state.IsStartPressed);

            if (previousState.DPad.IsDownPressed != state.DPad.IsDownPressed)
                DPadDownChanged?.Invoke(this, state.DPad.IsDownPressed);

            if (previousState.DPad.IsUpPressed != state.DPad.IsUpPressed)
                DPadUpChanged?.Invoke(this, state.DPad.IsUpPressed);

            if (previousState.DPad.IsLeftPressed != state.DPad.IsLeftPressed)
                DPadLeftChanged?.Invoke(this, state.DPad.IsLeftPressed);

            if (previousState.DPad.IsRightPressed != state.DPad.IsRightPressed)
                DPadRightChanged?.Invoke(this, state.DPad.IsRightPressed);
        }

        private static GamepadModel Map(XInputState state)
        {
            return new GamepadModel
            {
                LeftThumbStick = new ThumbStickModel
                {
                    Vertical = state.Gamepad.sThumbLY,
                    Horizontal = state.Gamepad.sThumbLX
                },
                RightThumbStick = new ThumbStickModel
                {
                    Horizontal = state.Gamepad.sThumbRX,
                    Vertical = state.Gamepad.sThumbRY
                },
                LeftTrigger = state.Gamepad.bLeftTrigger,
                RightTrigger = state.Gamepad.bRightTrigger,
                IsLeftTriggerButtonPressed = GetButtonState(state, ButtonFlags.XINPUT_GAMEPAD_LEFT_SHOULDER),
                IsRightTriggerButtonPressed = GetButtonState(state, ButtonFlags.XINPUT_GAMEPAD_RIGHT_SHOULDER),
                IsAPressed = GetButtonState(state, ButtonFlags.XINPUT_GAMEPAD_A),
                IsBPressed = GetButtonState(state, ButtonFlags.XINPUT_GAMEPAD_B),
                IsXPressed = GetButtonState(state, ButtonFlags.XINPUT_GAMEPAD_X),
                IsYPressed = GetButtonState(state, ButtonFlags.XINPUT_GAMEPAD_Y),
                IsBackPressed = GetButtonState(state, ButtonFlags.XINPUT_GAMEPAD_BACK),
                IsStartPressed = GetButtonState(state, ButtonFlags.XINPUT_GAMEPAD_START),
                DPad = new DPadModel
                {
                    IsDownPressed = GetButtonState(state, ButtonFlags.XINPUT_GAMEPAD_DPAD_DOWN),
                    IsUpPressed = GetButtonState(state, ButtonFlags.XINPUT_GAMEPAD_DPAD_UP),
                    IsLeftPressed = GetButtonState(state, ButtonFlags.XINPUT_GAMEPAD_DPAD_LEFT),
                    IsRightPressed = GetButtonState(state, ButtonFlags.XINPUT_GAMEPAD_DPAD_RIGHT),
                }
            };
        }

        private static bool GetButtonState(XInputState state, ButtonFlags button)
        {
            return state.Gamepad.IsButtonPressed((int) button);
        }

        public void Dispose()
        {
            StopPolling();
        }
    }
}
