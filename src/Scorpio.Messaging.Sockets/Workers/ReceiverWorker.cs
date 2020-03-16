using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Scorpio.Messaging.Sockets.Workers
{
    internal sealed class ReceiverWorker : WorkerBase
    {
        internal event EventHandler<PacketReceivedEventArgs> PacketReceived;
        internal event EventHandler<FaultExceptionEventArgs> ReceiverNetworkFault;

        protected override int WorkerSleepTime => 5;
        internal int MaxPacketLength { get; set; } = 1000;
        
        private readonly byte[] _data;

        public ReceiverWorker(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            _data = new byte[MaxPacketLength];
        }

        protected override void DoWork()
        {
            if (!NetworkStream.CanRead)
                return;
            int length = 0;

            try
            {
                // Receive header first - 4 bytes indicating total packet length
                length = ReceiveHeader();

                // Receive actual packet
                ReceivePayload(length);

                // Invoke event
                var eventArgs = new PacketReceivedEventArgs(_data, length);
                PacketReceived?.Invoke(this, eventArgs);
            }
            catch (ArgumentOutOfRangeException)
            {
                Logger.LogWarning("Received message, length bytes error (invalid protocol), len: " + length);
            }
            catch (IOException ex) when (ex.InnerException is SocketException innerEx && innerEx.SocketErrorCode == SocketError.TimedOut)
            {
                // Receive timeout
                // Swallow
            }
            catch (IOException ex)
            {
                // Critical fault - probably need to restart connection
                var eventArgs = new FaultExceptionEventArgs(ex);
                ReceiverNetworkFault?.Invoke(this, eventArgs);
            }
        }

        private int ReceiveHeader()
        {
            var header = new byte[4];

            int receivedSize = 0;
            while (receivedSize < 4)
            {
                receivedSize += NetworkStream.Read(header, receivedSize, 4 - receivedSize);
            }

            // Convert to integer with correct endianness
            int headerLengthInt = BitConverter.ToInt32(header, 0);
            return headerLengthInt;

            // TODO: discuss if we should use it
            //int length = IPAddress.NetworkToHostOrder(headerLengthInt);
            //return length;
        }

        private void ReceivePayload(int length)
        {
            int receivedSize = 0;

            while (receivedSize < length)
            {
                receivedSize += NetworkStream.Read(_data, receivedSize, length - receivedSize);
            }
        }
    }
}
