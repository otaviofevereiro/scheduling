using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Scheduling.Services
{
    public abstract class IntervalScheduling : IHostedService, IDisposable
    {
        private Timer timer;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(Run, null, TimeSpan.Zero, GetInterval());

            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);

            await Task.CompletedTask;
        }

        protected abstract TimeSpan GetInterval();
        protected abstract void Run(object state);


        #region IDisposable Support
        private bool disposedValue = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    timer.Dispose();
                }

                disposedValue = true;
            }
        }
        #endregion
    }
}
