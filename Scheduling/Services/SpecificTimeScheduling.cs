using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Scheduling.Services
{
    public abstract class SpecificTimeScheduling : IHostedService, IDisposable
    {
        private Timer timer;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            ValidateTimes();

            UpdateTimer();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        protected abstract IEnumerable<TimeSpan> GetTimes();

        protected abstract Task Run(object state);

        private async void DoRun(object state)
        {
            try
            {
                await Run(state);
            }
            finally
            {
                UpdateTimer();
            }
        }

        private TimeSpan GetTimeNextExectionTime(DateTime dateTime)
        {
            var time = GetTimes().Where(t => t >= dateTime.TimeOfDay)
                                 .Distinct()
                                 .OrderBy(t => t)
                                 .FirstOrDefault();

            if (time == TimeSpan.Zero)
                time = GetTimes().OrderBy(t => t).FirstOrDefault();

            var timeForNextExecution = time - dateTime.TimeOfDay;

            if (timeForNextExecution < TimeSpan.Zero)
                return time.Add(TimeSpan.FromHours(24)) - dateTime.TimeOfDay;

            return timeForNextExecution;
        }
        private void UpdateTimer()
        {
            timer = new Timer(DoRun, null, GetTimeNextExectionTime(DateTime.Now), Timeout.InfiniteTimeSpan);
        }

        private void ValidateTimes()
        {
            foreach (var time in GetTimes())
            {
                if (time.Days >= 1)
                    throw new InvalidOperationException("O horário informado é inválido, deve passar um horario do dia de 00:00:00 a 23:59:59");
            }
        }

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
