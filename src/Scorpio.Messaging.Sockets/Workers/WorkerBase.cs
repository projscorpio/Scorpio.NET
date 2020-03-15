using Microsoft.Extensions.Logging;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Scorpio.Messaging.Sockets.Workers
{
    internal abstract class WorkerBase : IWorker
    {
        protected virtual int WorkerSleepTime => 10;
        internal WorkerStatus Status { get; set; }
        protected Task Task;
        protected CancellationTokenSource CancellationTokenSource;
        protected ILogger Logger;

        private NetworkStream _stream;
        internal NetworkStream NetworkStream
        {
            get
            {
                if (_stream is null)
                    throw new ArgumentNullException(nameof(_stream));
                return _stream;
            }
            set => _stream = value;
        }

        protected WorkerBase(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(this.GetType());
            Status = WorkerStatus.Stopped;
        }

        public void Start()
        {
            if (Status == WorkerStatus.Running)
                throw new InvalidOperationException("Worker is already started");

            StartTask(Run);
            Status = WorkerStatus.Running;
        }

        public void Stop()
        {
            Status = WorkerStatus.Stopped;
        }

        public void Cancel()
        {
            CancellationTokenSource.Cancel();
        }

        public void Close()
        {
            Stop();
            CancellationTokenSource.Cancel();
        }

        private void StartTask(Action action)
        {
            CancellationTokenSource = new CancellationTokenSource();
            Task = new Task(action, CancellationTokenSource.Token);
            Task.ContinueWith(ExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            Task.Start();
        }
        
        private void Run()
        {
            try
            {
                while (Status == WorkerStatus.Running)
                {
                    DoWork();
                    Thread.Sleep(WorkerSleepTime);
                    CancellationTokenSource.Token.ThrowIfCancellationRequested();
                }
            }
            catch (OperationCanceledException)
            {
                Status = WorkerStatus.Stopped;
            }
        }

        protected abstract void DoWork();

        private void ExceptionHandler(Task task)
        { 
            Logger.LogError(task.Exception, $"Worker faulted: {task.Exception?.Message}, {task.Exception?.InnerException?.Message}");
            Status = WorkerStatus.Faulted;
            Task.Delay(500).Wait();
            Start();
        }
    }
}