using System;
using System.Diagnostics;

namespace Scheduling.Services
{
    public class TestIntervalScheduling : IntervalScheduling
    {
        protected override TimeSpan GetInterval()
        {
            return new TimeSpan(0, 0, 1);
        }

        protected override void Run(object state)
        {
            Debug.WriteLine($"O processo executou a {DateTime.Now}");
        }
    }
}
