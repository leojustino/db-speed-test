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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Constants;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace MongoClient
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length > 0)
                try
                {
                    Console.WriteLine(Console.Title = "MongoDB Client");
                    WarmUp();

                    switch (args[0])
                    {
                        case PerfConstants.CmdInserts:
                            DoInserts();
                            break;

                        case PerfConstants.CmdUpdates:
                        case PerfConstants.CmdMasiveUpdates:
                            DoUpdates();
                            break;

                        case PerfConstants.CmdQuery:
                        case PerfConstants.CmdComplexQuery:
                        case PerfConstants.CmdQueryNoIndex:
                        default:
                            Console.WriteLine("invalid comand");
                            Console.ReadKey();
                            return;
                    }

                    Console.WriteLine("Done");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.ReadKey();
                }
        }

        static void WarmUp()
        {
            Console.WriteLine("Warming up ...");

            SpeedyMongoDataContext.BasicCollection.DeleteMany(new BsonDocument());
            SpeedyMongoDataContext.CustomerCollection.DeleteMany(new BsonDocument());
        }
        static void WaitForCoordinationAppToSayGo(string mutexName)
        {
            Console.WriteLine("Waiting on mutex");
            var mutex = new Mutex(false, mutexName);
            DateTime t0 = DateTime.Now;
            mutex.WaitOne();
            TimeSpan dt = DateTime.Now - t0;

            if (dt.TotalSeconds < .1)
                Console.WriteLine("WARNING: You may have run this app without first starting the launcher app to coordinate multiple clients...");

            mutex.ReleaseMutex();
            mutex.Dispose();
            Console.WriteLine("Running!");
        }

        static IEnumerable<Basic> GetInsertData(int inserts)
        {
            Console.WriteLine("Building insert data...");

            var list = new List<Basic>(inserts);

            for (int i = 0; i < inserts; i++)
                list.Add(new Basic
                {
                    _id = ObjectId.GenerateNewId(),
                    TheData = "Graphs are a flexible modeling construct that can be used to model a domain and the indices that partition that domain into an efficient, searchable space. When the relations between the objects of the domain are seen as vertex partitions, then a graph is simply an index that relates vertices to vertices by edges. The way in which these vertices relate to each other determines which graph traversals are most efficient to execute and which problems can be solved by the graph data structure.",
                    TheTime = DateTime.Now,
                });

            return list;
        }

        static PerformanceResult DoComplexQuery(int count)
        {
            var foundCount = 0;
            var totalOrders = 0;
            var start = DateTime.Now;
            var sw = Stopwatch.StartNew();

            for (int i = 0; i < count; i++)
            {
                var name = ("Cust " + i);
                var found = (from c in SpeedyMongoDataContext.Customers where c.Name == name select c).FirstOrDefault();

                if (found != null)
                {
                    foundCount++;

                    totalOrders += found.Orders.Sum(o => o.Quantity);
                }
            }

            sw.Stop();
            Console.WriteLine("Found {0} customers with {1} orders.", foundCount, totalOrders);

            return new PerformanceResult
            {
                DB = "MongoDB",
                Quantity = count,
                StartTime = start,
                TimeSpent = sw.Elapsed,
                Type = ResultType.QueryComplex
            };
        }
        static PerformanceResult DoQueryWithNoIndex(int count)
        {
            var start = DateTime.Now;
            var sw = Stopwatch.StartNew();

            for (int i = 0; i < count; i++)
            {
                var item = (from b in SpeedyMongoDataContext.Basics where b.TheData.Contains("mac") select b).FirstOrDefault();

                if (item != null)
                    item.TheTime = DateTime.Now;
            }

            sw.Stop();

            return new PerformanceResult
            {
                DB = "MongoDB",
                Quantity = count,
                StartTime = start,
                TimeSpent = sw.Elapsed,
                Type = ResultType.QueryNoIndex,
            };
        }
        static PerformanceResult DoStraightQueries(int count)
        {
            var first = SpeedyMongoDataContext.Basics.First();
            var missing = ObjectId.GenerateNewId();
            var start = DateTime.Now;
            var sw = Stopwatch.StartNew();

            for (int i = 0; i < count; i++)
            {
                var id = first._id;

                if (i % 2 == 0)
                    id = missing;

                var item = (from b in SpeedyMongoDataContext.Basics where b._id == id select b).FirstOrDefault();

                if (item != null)
                    item.TheTime = DateTime.Now;
            }

            sw.Stop();

            return new PerformanceResult
            {
                DB = "MongoDB",
                Quantity = count,
                StartTime = start,
                TimeSpent = sw.Elapsed,
                Type = ResultType.Query,
            };
        }
        static void DoInserts()
        {
            var list = GetInsertData(PerfConstants.CountInserts);

            WaitForCoordinationAppToSayGo(PerfConstants.Mutex1);
            var start = DateTime.Now;
            var sw = Stopwatch.StartNew();

            foreach (var basic in list)
                SpeedyMongoDataContext.Add(basic);

            sw.Stop();

            new PerformanceResult
            {
                DB = "MongoDB",
                Quantity = list.Count(),
                StartTime = start,
                TimeSpent = sw.Elapsed,
                Type = ResultType.Inserts,
            }.SaveFile();
        }
        static void DoUpdates()
        {
            WaitForCoordinationAppToSayGo(PerfConstants.Mutex1);

            var start = DateTime.Now;
            var sw = Stopwatch.StartNew();
            var list = SpeedyMongoDataContext.CustomerCollection
                .Find(new BsonDocument())
                .Limit(PerfConstants.CountUpdates)
                .SortBy(a => a.Name)
                .ToList();

            sw.Stop();

            new PerformanceResult
            {
                DB = "MongoDB",
                Quantity = PerfConstants.CountUpdates,
                StartTime = start,
                TimeSpent = sw.Elapsed,
                Type = ResultType.LoadDataQuery,
            }.SaveFile();

            WaitForCoordinationAppToSayGo(PerfConstants.Mutex2);

            start = DateTime.Now;

            sw.Restart();

            foreach (var basic in list)
                SpeedyMongoDataContext.CustomerCollection
                    .UpdateOne(a => a._id == basic._id, Builders<Customer>.Update.Set(b => b.Name, PerfConstants.UpdateData));

            sw.Stop();

            new PerformanceResult
            {
                DB = "MongoDB",
                Quantity = PerfConstants.CountUpdates,
                StartTime = start,
                TimeSpent = sw.Elapsed,
                Type = ResultType.Updates,
            }.SaveFile();
        }
    }
}