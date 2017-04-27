using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SpeedTest.Entities;
using SpeedTest.Interfaces;
using SpeedTest.Values;

namespace SpeedTest.Implementations.TestsExecutions
{
    class SingleMassiveInserts : ITestExecution
    {
        public SingleMassiveInserts(
            IRepository repository,
            ITestConfiguration testConfiguration)
        {
            this.repository = repository;
            this.testConfiguration = testConfiguration;
        }

        IRepository repository;
        ITestConfiguration testConfiguration;

        public TestType Type
        {
            get
            {
                return TestType.SingleMasiveInserts;
            }
        }

        public event Action<string> WaitingMutex;

        public void ExecuteTest()
        {
            var result = new PerformanceResult { Driver = repository.Driver, Operations = 1, Type = Type, Operation = OperationType.Delete };
            var customers = new List<Customer>(testConfiguration.SingleMassiveInserts);

            for (int i = 0; i < testConfiguration.SingleMassiveInserts; i++)
                customers.Add(new Customer
                {
                    Id = Guid.NewGuid(),
                    Name = Guid.NewGuid().ToString("N"),
                    NoIndexId = Guid.NewGuid(),
                    Orders = new Order[]
                    {
                        new Order { Id = Guid.NewGuid(), NoIndexId = Guid.NewGuid(), Quantity = 1 },
                        new Order { Id = Guid.NewGuid(), NoIndexId = Guid.NewGuid(), Quantity = 2 }
                    },
                });

            WaitingMutex?.Invoke(Constants.Mutex1);

            using (var mutex = new Mutex(false, Constants.Mutex1))
            {
                result.StartTime = DateTime.Now;
                result.TimeSpent = repository.RemoveAllCustomers();

                result.SaveFile();
            }

            result.Operations = testConfiguration.SingleMassiveInserts;
            result.Operation = OperationType.Insert;

            WaitingMutex?.Invoke(Constants.Mutex2);

            using (var mutex = new Mutex(false, Constants.Mutex2))
            {
                result.StartTime = DateTime.Now;
                result.TimeSpent = repository.InsertCustomers(customers);

                result.SaveFile();
            }
        }
    }
}
