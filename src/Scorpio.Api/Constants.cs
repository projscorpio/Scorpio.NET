namespace Scorpio.Api
{
    public static class Constants
    {
        /// <summary>
        /// SignalR topics for messaging/RPC
        /// </summary>
        public static class Topics
        {
            /// <summary>
            /// Represents Ubiquiti SNMP data
            /// </summary>
            public static string Ubiquiti = "ubiquiti";

            /// <summary>
            /// Represents data gathered by on-board sensorics
            /// </summary>
            public static string Sensor = "sensor";

            /// <summary>
            /// Represents current rover position
            /// </summary>
            public static string GpsPosition = "gps";

            /// <summary>
            /// Compass readings
            /// </summary>
            public static string Compass = "compass";
        }
    }
}
