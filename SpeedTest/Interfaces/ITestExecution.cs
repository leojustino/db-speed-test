using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpeedTest.Values;

namespace SpeedTest.Interfaces
{
    interface ITestExecution
    {
        event Action<string> WaitingMutex;

        TestType Type { get; }

        void ExecuteTest();
    }
}
