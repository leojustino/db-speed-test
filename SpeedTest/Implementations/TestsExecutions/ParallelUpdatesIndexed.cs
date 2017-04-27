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
    class ParallelUpdatesIndexed : ITestExecution
    {
        public ParallelUpdatesIndexed(
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
                return TestType.ParallelUpdatesIndexed;
            }
        }

        public event Action<string> WaitingMutex;

        public void ExecuteTest()
        {
            var result = new PerformanceResult { Driver = repository.Driver, Operations = 1, Type = Type, Operation = OperationType.QueryForUpdate };
            var customers = (IList<Customer>)null;
            var newData = Guid.NewGuid().ToString("N");

            WaitingMutex?.Invoke(Constants.Mutex1);

            using (var mutex = new Mutex(false, Constants.Mutex1))
            {
                result.StartTime = DateTime.Now;
                result.TimeSpent = repository.QueryCustomers(configuration.ParallelUpdates, out customers);

                result.SaveFile();
            }

            foreach (var item in customers)
                item.Name = newData;

            result.Operation = OperationType.UpdateIndexed;
            result.Operations = configuration.ParallelUpdates;

            using (var mutex = new Mutex(false, Constants.Mutex2))
            {
                result.StartTime = DateTime.Now;
                result.TimeSpent = repository.UpdateIndexedCustomers(customers);

                result.SaveFile();
            }
        }
    }
}
