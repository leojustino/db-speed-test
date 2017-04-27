using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpeedTest.Drivers.MongoDB;
using SpeedTest.Entities;
using SpeedTest.Interfaces;
using SpeedTest.Values;
using SpeedTest.Helpers;
using MongoDB.Driver;
using MongoDB.Bson;

namespace SpeedTest.Implementations.Repositories
{
    class MongoDbRepository : IRepository
    {
        public MongoDbRepository(MongoDbContext context)
        {
            context = this.context;
        }

        static UpdateDefinitionBuilder<Customer> update = Builders<Customer>.Update;

        MongoDbContext context;

        public DriverType Driver
        {
            get
            {
                return DriverType.MongoDB;
            }
        }

        public TimeSpan InsertBasics(IList<Basic> basics)
        {
            return this.Elapsed(() => context.BasicCollection.InsertMany(basics));
        }

        public TimeSpan InsertCustomers(IList<Customer> customers)
        {
            return this.Elapsed(() => context.CustomerCollection.InsertMany(customers));
        }

        public TimeSpan QueryCustomers(int limit, out IList<Customer> customers)
        {
            var temp = (IList<Customer>)null;
            var time = this.Elapsed(() =>
            {
                temp = context.CustomerCollection.Find(new BsonDocument()).Limit(limit).ToList();
            });

            customers = temp;

            return time;
        }

        public TimeSpan RemoveAllCustomers()
        {
            return this.Elapsed(() => context.CustomerCollection.DeleteMany(new BsonDocument()));
        }

        public TimeSpan UpdateIndexedCustomers(IList<Customer> customers)
        {
            return this.Elapsed(() =>
            {
                foreach (var customer in customers)
                    context.CustomerCollection.UpdateOne(a => a.Id == customer.Id, update.Set(a => a.Name, "New name"));
            });
        }

        public TimeSpan UpdateUnindexedCustomers(IList<Customer> customers)
        {
            return this.Elapsed(() =>
            {
                foreach (var customer in customers)
                    context.CustomerCollection.UpdateOne(a => a.NoIndexId == customer.NoIndexId, update.Set(a => a.Name, "New name"));
            });
        }
    }
}
