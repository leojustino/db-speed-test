using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedTest.Interfaces
{
    interface ITestConfiguration
    {
        int SingleMassiveInserts { get; }
        int ParallelInserts { get; }
        int ParallelUpdates { get; }
    }
}
