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
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Constants;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace SqlTestApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                var insertData = GetInsertData(PerfConstants.CountInserts);
                var result = (PerformanceResult)null;

                Console.WriteLine(Console.Title = "SQL Server Client");
                WarmUp();
                WaitForCoordinationAppToSayGo();
                Console.WriteLine("Running!");

                using (var ctx = new SpeedyDataContext())
                {
                    switch (args[0])
                    {
                        case PerfConstants.CmdInserts:
                            result = DoInserts(insertData, ctx);
                            Console.WriteLine($"Finished with {PerfConstants.CountInserts} SQL inserts in {result.TimeSpent.TotalSeconds} sec.");
                            break;

                        case PerfConstants.CmdQuery:
                            result = DoStraightQueries(ctx, PerfConstants.CountQueries);
                            Console.WriteLine($"Finished with {PerfConstants.CountQueries} SQL basic queries in {result.TimeSpent.TotalSeconds} sec.");
                            break;

                        case PerfConstants.CmdComplexQuery:
                            result = DoComplexQuery(ctx, PerfConstants.CountQueries);
                            Console.WriteLine($"Finished with {PerfConstants.CountQueries} 'joined' SQL queries in {result.TimeSpent.TotalSeconds} sec.");
                            break;

                        case PerfConstants.CmdQueryNoIndex:
                            result = DoQueryWithNoIndex(ctx, PerfConstants.CountQueries);
                            Console.WriteLine($"Finished with {PerfConstants.CountQueries} SQL non-indexed queries in {result.TimeSpent.TotalSeconds} sec.");
                            break;

                        case PerfConstants.CmdUpdates:
                        default:
                            Console.WriteLine("invalid comand");
                            return;
                    }
                }

                File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{Guid.NewGuid().ToString("N")}.data"), JsonConvert.SerializeObject(result));
                Console.WriteLine("Done");
                Console.ReadLine();
            }
        }

        static void WaitForCoordinationAppToSayGo()
        {
            Console.WriteLine("Waiting on mutex");
            Mutex mutex = new Mutex(false, "MONGO TEST MUTEX");
            DateTime t0 = DateTime.Now;
            mutex.WaitOne();
            TimeSpan dt = DateTime.Now - t0;

            if (dt.TotalSeconds < .1)
                Console.WriteLine("WARNING: You may have run this app without first starting the launcher app to coordinate multiple clients...");

            mutex.ReleaseMutex();
            mutex.Dispose();
        }

        static PerformanceResult DoComplexQuery(SpeedyDataContext ctx, int count)
        {
            var foundCount = 0;
            var totalOrders = 0;
            var start = DateTime.Now;
            var sw = Stopwatch.StartNew();

            for (int i = 0; i < count; i++)
            {
                var found = (from c in ctx.Customers where c.Name == ("cust " + i) select c).FirstOrDefault();

                if (found != null)
                {
                    foundCount++;

                    totalOrders += found.CustomerToOrders.Sum(to => to.Order.Quantity);
                }
            }

            sw.Stop();
            Console.WriteLine("Found {0} customers with {1} orders.", foundCount, totalOrders);

            return new PerformanceResult
            {
                DB = "SQL Server Entity Framework",
                Quantity = count,
                StartTime = start,
                Type = ResultType.QueryNoIndex,
                TimeSpent = sw.Elapsed,
            };
        }

        static PerformanceResult DoQueryWithNoIndex(SpeedyDataContext ctx, int count)
        {
            var start = DateTime.Now;
            var sw = Stopwatch.StartNew();

            for (int i = 0; i < count; i++)
            {
                var item = (from b in ctx.Basics where b.TheData.Contains("mac") select b).FirstOrDefault();

                if (item != null)
                    item.TheTime = DateTime.Now;
            }

            sw.Stop();

            return new PerformanceResult
            {
                DB = "SQL Server Entity Framework",
                Quantity = count,
                StartTime = start,
                Type = ResultType.QueryNoIndex,
                TimeSpent = sw.Elapsed,
            };
        }

        static PerformanceResult DoStraightQueries(SpeedyDataContext ctx, int count)
        {
            var first = ctx.Basics.First();
            var missing = ObjectId.GenerateNewId().ToString();
            var start = DateTime.Now;
            var sw = Stopwatch.StartNew();

            for (int i = 0; i < count; i++)
            {
                var id = first.id;

                if (i % 2 == 0)
                    id = missing;

                var item = (from b in ctx.Basics where b.id == id select b).FirstOrDefault();

                if (item != null)
                    item.TheTime = DateTime.Now;
            }

            sw.Stop();

            return new PerformanceResult
            {
                DB = "SQL Server Entity Framework",
                Quantity = count,
                StartTime = start,
                Type = ResultType.Query,
                TimeSpent = sw.Elapsed,
            };
        }

        static void WarmUp()
        {
            Console.WriteLine("Warming up ...");

            using (var ctx = new SpeedyDataContext(ConfigurationManager.ConnectionStrings["SpeedyConnectionString"].ConnectionString))
            {
                ctx.Basics.Count();
                ctx.Basics.DeleteAllOnSubmit(ctx.Basics.ToArray());
                ctx.CustomerToOrders.DeleteAllOnSubmit(ctx.CustomerToOrders.ToArray());
                ctx.Customers.DeleteAllOnSubmit(ctx.Customers.ToArray());
                ctx.SubmitChanges();
            }
        }

        static PerformanceResult DoInserts(List<SimplerKeysBasic> list, SpeedyDataContext ctx)
        {
            var start = DateTime.Now;
            var sw = Stopwatch.StartNew();

            foreach (var basic in list)
            {
                ctx.SimplerKeysBasics.InsertOnSubmit(basic);
                ctx.SubmitChanges();
            }

            sw.Stop();

            return new PerformanceResult
            {
                DB = "SQL Server Entity Framework",
                Quantity = list.Count,
                StartTime = start,
                Type = ResultType.Inserts,
                TimeSpent = sw.Elapsed,
            };
        }

        static List<SimplerKeysBasic> GetInsertData(int inserts)
        {
            var list = new List<SimplerKeysBasic>(inserts);

            for (int i = 0; i < inserts; i++)
            {
                SimplerKeysBasic basic = new SimplerKeysBasic();
                basic.MongoDBeskID = ObjectId.GenerateNewId().ToString();
                basic.TheData = "Graphs are a flexible modeling construct that can be used to model a domain and the indices that partition that domain into an efficient, searchable space. When the relations between the objects of the domain are seen as vertex partitions, then a graph is simply an index that relates vertices to vertices by edges. The way in which these vertices relate to each other determines which graph traversals are most efficient to execute and which problems can be solved by the graph data structure.";
                basic.TheTime = DateTime.Now;
                list.Add(basic);
            }
            return list;
        }
    }
}