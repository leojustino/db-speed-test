using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpeedTest.Drivers.SqlServerEntityFramework;
using SpeedTest.Entities;
using SpeedTest.Interfaces;
using SpeedTest.Values;
using SpeedTest.Helpers;

namespace SpeedTest.Implementations.Repositories
{
    class SqlServerEntityRepository : IRepository
    {
        public SqlServerEntityRepository(SqlServerEntityContext context)
        {
            this.context = context;
        }

        SqlServerEntityContext context;

        public DriverType Driver
        {
            get
            {
                return DriverType.SqlServerEntityFramework;
            }
        }

        public TimeSpan InsertBasics(IList<Basic> basics)
        {
            return this.Elapsed(() =>
            {
                context.Basic.AddRange(basics);
                context.SaveChanges();
            });
        }

        public TimeSpan InsertCustomers(IList<Customer> customers)
        {
            return this.Elapsed(() =>
            {
                context.Customer.AddRange(customers);
                context.SaveChanges();
            });
        }

        public TimeSpan QueryCustomers(int limit, out IList<Customer> customers)
        {
            var retorno = (IList<Customer>)null;
            var time = this.Elapsed(() => retorno = context.Customer.Take(limit).ToList());
            customers = retorno;

            return time;
        }

        public TimeSpan RemoveAllCustomers()
        {
            return this.Elapsed(() =>
            {
                context.Customer.RemoveRange(context.Customer.ToArray());
                context.SaveChanges();
            });
        }

        public TimeSpan UpdateIndexedCustomers(IList<Customer> customers)
        {
            return this.Elapsed(() =>
            {
                foreach (var oldCustomer in customers)
                    context.Customer.Find(oldCustomer.Id).Name = "New name";

                context.SaveChanges();
            });
        }

        public TimeSpan UpdateUnindexedCustomers(IList<Customer> customers)
        {
            return this.Elapsed(() =>
            {
                foreach (var oldCustomer in customers)
                    context.Customer.First(a => a.NoIndexId == oldCustomer.NoIndexId).Name = "New name";

                context.SaveChanges();
            });
        }
    }
}
