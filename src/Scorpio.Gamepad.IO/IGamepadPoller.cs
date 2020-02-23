using System;
using Scorpio.Gamepad.IO.Args;
using Scorpio.Gamepad.Models;

namespace Scorpio.Gamepad.IO
{
    public interface IGamepadPoller : IDisposable
    {
        event EventHandler<GamepadEventArgs> GamepadStateChanged;
        event EventHandler<bool> AChanged;
        event EventHandler<bool> BChanged;
        event EventHandler<bool> XChanged;
        event EventHandler<bool> YChanged;
        event EventHandler<bool> BackChanged;
        event EventHandler<bool> StartChanged;
        event EventHandler<bool> DPadDownChanged;
        event EventHandler<bool> DPadUpChanged;
        event EventHandler<bool> DPadLeftChanged;
        event EventHandler<bool> DPadRightChanged;

        GamepadModel GetState();
        bool IsConnected { get; }
        void Vibrate();
    }
}