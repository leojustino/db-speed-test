using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpeedTest.Entities;
using SpeedTest.Values;

namespace SpeedTest.Interfaces
{
    interface IRepository
    {
        DriverType Driver { get; }
        TimeSpan RemoveAllCustomers();
        TimeSpan InsertCustomers(IList<Customer> customers);
        TimeSpan InsertBasics(IList<Basic> basics);
        TimeSpan QueryCustomers(int limit, out IList<Customer> customers);
        TimeSpan UpdateIndexedCustomers(IList<Customer> customers);
        TimeSpan UpdateUnindexedCustomers(IList<Customer> customers);
    }
}
