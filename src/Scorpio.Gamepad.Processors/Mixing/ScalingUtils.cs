namespace Scorpio.Gamepad.Processors.Mixing
{
    public class ScalingUtils
    {
        public static float ByteToFloat(byte input)
        {
            return input / 255.0f;
        }

        public static float ShortToFloat(short input)
        {
            return input / 32767.0f;
        }

        /// <summary>
        /// Returns number bounded by param.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="boundary"></param>
        /// <returns></returns>
        public static float SymmetricalConstrain(float input, float boundary)
        {
            if (input > boundary) input = boundary;
            if (input < -boundary) input = -boundary;
            return input;
        }

        /// <summary>
        /// Returns number bounded by param.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="boundary"></param>
        /// <returns></returns>
        public static int SymmetricalConstrain(int input, int boundary)
        {
            if (input > boundary) input = boundary;
            if (input < -boundary) input = -boundary;
            return input;
        }

        public static short ConstrainNonnegative(short input, short constrain)
        {
            if (input < 0) input = 0;
            if (input > constrain) input = constrain;

            return input;
        }
    }
}