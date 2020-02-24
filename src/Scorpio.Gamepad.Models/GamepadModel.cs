namespace Scorpio.Gamepad.Models
{
    /// <summary>
    /// Represents state of gamepad controls
    /// </summary>
    public class GamepadModel
    {
        public ThumbStickModel LeftThumbStick { get; set; }
        public ThumbStickModel RightThumbStick { get; set; }
        public bool IsLeftThumbStickPressed { get; set; }
        public bool IsRightThumbStickPressed { get; set; }
        public byte LeftTrigger { get; set; }
        public byte RightTrigger { get; set; }
        public bool IsLeftTriggerButtonPressed { get; set; }
        public bool IsRightTriggerButtonPressed { get; set; }
        public DPadModel DPad { get; set; }
        public bool IsAPressed { get; set; }
        public bool IsBPressed { get; set; }
        public bool IsXPressed { get; set; }
        public bool IsYPressed { get; set; }
        public bool IsBackPressed { get; set; }
        public bool IsStartPressed { get; set; }
    }
}
