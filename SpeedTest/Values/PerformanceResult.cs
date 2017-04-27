using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedTest.Values
{
    class PerformanceResult
    {
        public DriverType Driver { get; set; }
        public TestType Type { get; set; }

        public DateTime StartTime { get; set; }
        public TimeSpan TimeSpent { get; set; }
        public OperationType Operation { get; set; }
        public int Operations { get; set; }

        public double Seconds
        {
            get
            {
                return Math.Round(TimeSpent.TotalSeconds, 1);
            }
        }
        public double QuantityPerSec
        {
            get
            {
                return Math.Round(Operations / TimeSpent.TotalSeconds, 1);
            }
        }


        public void SaveFile()
        {
            File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{Guid.NewGuid().ToString("N")}.data"), JsonConvert.SerializeObject(this));
        }
    }
}
