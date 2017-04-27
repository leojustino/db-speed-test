using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedTest.Values
{
    enum ClientTests
    {
        ParallelInserts,
        ParallelUpdatesIndexed,
        ParallelUpdatesNoIndexed,

        SingleMassiveInserts,

    }
}
