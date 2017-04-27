using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using SpeedTest.Entities;

namespace SpeedTest.Drivers.MongoDB
{
    class MongoDbContext
    {
        public MongoDbContext()
        {
            client = new MongoClient(ConfigurationManager.ConnectionStrings["MongoDBSpeedTestConnection"].ConnectionString);
            db = client.GetDatabase("SpeedTest");
            basicCollection = db.GetCollection<Basic>(typeof(Basic).Name);
            customerCollection = db.GetCollection<Customer>(typeof(Customer).Name);
        }

        MongoClient client;
        IMongoDatabase db;
        IMongoCollection<Basic> basicCollection;
        IMongoCollection<Customer> customerCollection;
        
        public IMongoCollection<Basic> BasicCollection
        {
            get
            {
                return basicCollection;
            }
        }
        public IMongoCollection<Customer> CustomerCollection
        {
            get
            {
                return customerCollection;
            }
        }
    }
}
