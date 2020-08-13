using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Scheduling.Services
{
    public class TestSpecificTimeScheduling : SpecificTimeScheduling
    {
        protected override IEnumerable<TimeSpan> GetTimes()
        {
            yield return new TimeSpan(1, 0, 0);
            yield return new TimeSpan(12, 30, 0);
            yield return new TimeSpan(18, 41, 30);
            yield return new TimeSpan(23, 59, 59);
        }

        protected override async Task Run(object state)
        {
            Debug.WriteLine($"O processo executou a {DateTime.Now}");

            await Task.CompletedTask;
        }
    }
}
