// A Sample Application by Michael Kennedy
// http://www.michaelckennedy.net  | @mkennedy
// 
// This application is meant to run a very rudimentary comparison
// of performance between SQL Server and MongoDB in .NET
// See the full blog post here:
//
//    URL-TO-COME
//
using System;
using System.Configuration;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MongoClient
{
    public class SpeedyMongoDataContext
    {
        static MongoDB.Driver.MongoClient client = new MongoDB.Driver.MongoClient(ConfigurationManager.AppSettings["mongoDbConnection"]);
        static IMongoDatabase db = client.GetDatabase("Speedy");

        public static IQueryable<Customer> Customers
        {
            get
            {
                return db.GetCollection<Customer>(typeof(Customer).Name).AsQueryable();
            }
        }           
        public static IQueryable<Basic> Basics
        {
            get
            {
                return db.GetCollection<Basic>(typeof(Basic).Name).AsQueryable();
            }
        }

        public static IMongoCollection<Basic> BasicCollection
        {
            get
            {
                return db.GetCollection<Basic>(typeof(Basic).Name);
            }
        }
        public static IMongoCollection<Customer> CustomerCollection
        {
            get
            {
                return db.GetCollection<Customer>(typeof(Customer).Name);
            }
        }

        public static void Add<T>(T entity) where T : MongoObj, new()
        {
            db.GetCollection<T>(typeof(T).Name).InsertOne(entity);
        }
        public static void Delete<T>(T entity) where T : MongoObj, new()
        {
            db.GetCollection<T>(typeof(T).Name).DeleteOne(a => a._id == entity._id);
        }
        public static void Save<T>(T entity) where T : MongoObj, new()
        {
            db.GetCollection<T>(typeof(T).Name).ReplaceOne(a => a._id == entity._id, entity);
        }
        public static void DropCollection<T>()
        {
            db.DropCollection(typeof(T).Name);
        }
    }
}