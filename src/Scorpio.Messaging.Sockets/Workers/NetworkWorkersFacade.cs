using Microsoft.Extensions.Logging;
using System;
using System.Net.Sockets;

namespace Scorpio.Messaging.Sockets.Workers
{
    internal class NetworkWorkersFacade
    {
        internal WorkerStatus SenderStatus => _senderWorker?.Status ?? WorkerStatus.Stopped;
        internal WorkerStatus ReceiverStatus => _receiverWorker?.Status ?? WorkerStatus.Stopped;

        internal event EventHandler<PacketReceivedEventArgs> PacketReceived;
        internal event EventHandler<FaultExceptionEventArgs> NetworkWorkerFaulted;
        internal NetworkStream NetworkStream { get; set; }

        protected ILogger<NetworkWorkersFacade> Logger { get; set; }
 
        private readonly SenderWorker _senderWorker;
        private readonly ReceiverWorker _receiverWorker;

        public NetworkWorkersFacade(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<NetworkWorkersFacade>();

            _senderWorker = new SenderWorker(loggerFactory)
            {
                NetworkStream = NetworkStream
            };
            _senderWorker.SenderNetworkFault += _senderWorker_SenderNetworkFault;

            _receiverWorker = new ReceiverWorker(loggerFactory)
            {
                NetworkStream = NetworkStream
            };
            _receiverWorker.PacketReceived += _receiverWorker_PacketReceived;
            _receiverWorker.ReceiverNetworkFault += _receiverWorker_ReceiverNetworkFault;
        }

        public void Enqueue(byte[] data) => _senderWorker?.Enqueue(data);

        private void _receiverWorker_ReceiverNetworkFault(object sender, FaultExceptionEventArgs args)
            => InvokeNetworkFaultedEvent(sender, args);

        private void _senderWorker_SenderNetworkFault(object sender, FaultExceptionEventArgs args)
            => InvokeNetworkFaultedEvent(sender, args);

        protected virtual void InvokeNetworkFaultedEvent(object sender, FaultExceptionEventArgs args)
            => NetworkWorkerFaulted?.Invoke(sender, args);

        private void _receiverWorker_PacketReceived(object sender, PacketReceivedEventArgs args)
            => PacketReceived?.Invoke(sender, args);

        public void Start()
        {
            _senderWorker.Start();
            _senderWorker.NetworkStream = NetworkStream;
            _receiverWorker.Start();
            _receiverWorker.NetworkStream = NetworkStream;
        }

        public void Stop()
        {
            _senderWorker.Stop();
            _receiverWorker.Stop();
        }
    }
}
