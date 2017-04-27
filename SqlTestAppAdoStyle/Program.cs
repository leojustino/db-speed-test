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
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Constants;
using Newtonsoft.Json;
using SqlTestApp;

namespace SqlTestAppAdoStyle
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
                try
                {
                    Console.WriteLine(Console.Title = "SQL Server Client - ADO.NET Style");
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

        static void WaitForCoordinationAppToSayGo(string mutexName)
        {
            Console.WriteLine("Waiting on mutex");
            var mutex = new Mutex(false, mutexName);
            var t0 = DateTime.Now;
            mutex.WaitOne();
            var dt = DateTime.Now - t0;

            if (dt.TotalSeconds < .1)
                Console.WriteLine("WARNING: You may have run this app without first starting the launcher app to coordinate multiple clients...");

            mutex.ReleaseMutex();
            mutex.Dispose();
            Console.WriteLine("Running!");
        }

        static SqlConnection NewConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["SpeedyConnectionString"].ConnectionString);
        }

        static void DoInserts()
        {
            var data = GetInsertData();

            WaitForCoordinationAppToSayGo(PerfConstants.Mutex1);

            var start = DateTime.Now;
            var sw = Stopwatch.StartNew();

            using (var conn = NewConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                cmd.CommandText = @"INSERT INTO [Speedy].[dbo].[Basic]([id],[TheData],[TheTime])VALUES(@id,@data,@time)";

                cmd.Parameters.Add("@id", SqlDbType.VarChar);
                cmd.Parameters.Add("@data", SqlDbType.VarChar);
                cmd.Parameters.Add("@time", SqlDbType.DateTime);

                foreach (var basic in data)
                {
                    cmd.Parameters["@id"].Value = basic.id;
                    cmd.Parameters["@data"].Value = basic.TheData;
                    cmd.Parameters["@time"].Value = basic.TheTime;

                    cmd.ExecuteNonQuery();
                }
            }

            sw.Stop();

            new PerformanceResult
            {
                DB = "SQL Server ADO",
                Quantity = data.Count(),
                StartTime = start,
                Type = ResultType.Inserts,
                TimeSpent = sw.Elapsed,
            }.SaveFile();
        }
        static void DoUpdates()
        {
            WaitForCoordinationAppToSayGo(PerfConstants.Mutex1);

            var list = new List<Guid>(PerfConstants.CountUpdates);
            var start = DateTime.Now;
            var sw = Stopwatch.StartNew();

            using (var conn = NewConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                cmd.CommandText = $"select top {PerfConstants.CountUpdates} id from customer";

                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        list.Add(reader.GetGuid(0));

                sw.Stop();

                new PerformanceResult
                {
                    DB = "SQL Server ADO",
                    Quantity = PerfConstants.CountUpdates,
                    StartTime = start,
                    Type = ResultType.LoadDataQuery,
                    TimeSpent = sw.Elapsed,
                }.SaveFile();

                WaitForCoordinationAppToSayGo(PerfConstants.Mutex2);

                start = DateTime.Now;

                sw.Restart();

                cmd.CommandText = $"update customer set name = '{PerfConstants.UpdateData}' where id = @1";

                cmd.Parameters.Add("@1", SqlDbType.UniqueIdentifier);

                foreach (var item in list)
                {
                    cmd.Parameters["@1"].Value = item;
                    cmd.ExecuteNonQuery();
                }

                sw.Stop();

                new PerformanceResult
                {
                    DB = "SQL Server ADO",
                    Quantity = PerfConstants.CountUpdates,
                    StartTime = start,
                    Type = ResultType.Updates,
                    TimeSpent = sw.Elapsed,
                }.SaveFile();
            }
        }

        static IEnumerable<Basic> GetInsertData()
        {
            var list = new List<Basic>(PerfConstants.CountInserts);

            for (int i = 0; i < PerfConstants.CountInserts; i++)
                list.Add(new Basic
                {
                    id = Guid.NewGuid().ToString().Substring(0, 23),
                    TheData = "Graphs are a flexible modeling construct that can be used to model a domain and the indices that partition that domain into an efficient, searchable space. When the relations between the objects of the domain are seen as vertex partitions, then a graph is simply an index that relates vertices to vertices by edges. The way in which these vertices relate to each other determines which graph traversals are most efficient to execute and which problems can be solved by the graph data structure.",
                    TheTime = DateTime.Now,
                });

            return list;
        }

        static void WarmUp()
        {
            Console.WriteLine("Warming up ...");

            using (var conn = NewConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                cmd.CommandText = @"select count(*) from [Speedy].[dbo].[Basic]";
                cmd.ExecuteNonQuery();
            }
        }
    }
}