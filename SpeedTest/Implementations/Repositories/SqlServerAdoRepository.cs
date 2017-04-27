using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpeedTest.Drivers.SqlServerAdo;
using SpeedTest.Entities;
using SpeedTest.Helpers;
using SpeedTest.Interfaces;
using SpeedTest.Values;

namespace SpeedTest.Implementations.Repositories
{
    class SqlServerAdoRepository : IRepository
    {
        public SqlServerAdoRepository(SqlServerAdoContext context)
        {
            this.context = context;
        }

        SqlServerAdoContext context;

        public DriverType Driver
        {
            get
            {
                return DriverType.SqlServerAdo;
            }
        }

        public TimeSpan InsertBasics(IList<Basic> basics)
        {
            return this.Elapsed(() => context.InsertManyBasics(basics));
        }

        public TimeSpan InsertCustomers(IList<Customer> customers)
        {
            return this.Elapsed(() => context.InsertManyCustomers(customers));
        }

        public TimeSpan QueryCustomers(int limit, out IList<Customer> customers)
        {
            var temp = (IList<Customer>)null;
            var time = this.Elapsed(() => temp = context.QueryCustomers(limit));
            customers = temp;

            return time;
        }

        public TimeSpan RemoveAllCustomers()
        {
            return this.Elapsed(() => context.DeleteCustomers());
        }

        public TimeSpan UpdateIndexedCustomers(IList<Customer> customers)
        {
            return this.Elapsed(() => context.UpdateIndexedCustomers(customers));
        }

        public TimeSpan UpdateUnindexedCustomers(IList<Customer> customers)
        {
            return this.Elapsed(() => context.UpdateUnindexedCustomers(customers));
        }
    }
}
