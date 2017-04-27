using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SpeedTest.Entities;
using SpeedTest.Interfaces;
using SpeedTest.Values;

namespace SpeedTest.Implementations.TestsExecutions
{
    class ParallelInserts : ITestExecution
    {
        public ParallelInserts(
            IRepository repository, 
            ITestConfiguration configuration)
        {
            this.repository = repository;
            this.configuration = configuration;
        }

        IRepository repository;
        ITestConfiguration configuration;

        public TestType Type
        {
            get
            {
                return TestType.ParallelInserts;
            }
        }

        public event Action<string> WaitingMutex;

        public void ExecuteTest()
        {
            var result = new PerformanceResult { Driver = repository.Driver, Operations = configuration.ParallelInserts, Type = Type, Operation = OperationType.Insert };
            var basics = new List<Basic>(configuration.ParallelInserts);

            for (int i = 0; i < configuration.ParallelInserts; i++)
                basics.Add(new Basic
                {
                    Id = Guid.NewGuid(),
                    NoIndexId = Guid.NewGuid(),
                    TheData = Guid.NewGuid().ToString("N"),
                    TheTime = DateTime.Now
                });

            WaitingMutex?.Invoke(Constants.Mutex1);

            using (var mutex = new Mutex(false, Constants.Mutex1))
            {
                result.StartTime = DateTime.Now;
                result.TimeSpent = repository.InsertBasics(basics);

                result.SaveFile();
            }
        }
    }
}
