using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedTest.Helpers
{
    public static class ObjectHelper
    {
        public static TimeSpan Elapsed(this object obj, Action action)
        {
            var timer = Stopwatch.StartNew();

            try
            {
                action?.Invoke();
            }
            catch { }

            timer.Stop();
            return timer.Elapsed;
        }
    }
}
