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
using System.IO;
using Newtonsoft.Json;

namespace Constants
{
    public class PerfConstants
    {
        public const string UpdateData = "asd";
        public const string Mutex1 = "{9025F9EE-6817-44FC-B0BA-B0EFBFDE5902}";
        public const string Mutex2 = "{16903858-D62A-40C6-97D8-A8E69C954551}";

        public const int CountUpdates = 10000;
        public const int CountInserts = 2000;
        public const int CountQueries = 100;
        public const int CountFillDatabase = 50000;

        public const string CmdInserts = "Inserts";
        public const string CmdUpdates = "Updates";
        public const string CmdMasiveUpdates = "Updates Masive";
        public const string CmdQuery = "Query";
        public const string CmdComplexQuery = "Query Complex";
        public const string CmdQueryNoIndex = "Query No Index";
    }

    public enum ResultType
    {
        QueryComplex,
        Query,
        QueryNoIndex,
        LoadDataQuery,
        Inserts,
        Updates,
        MasiveUpdates,
    }
    public class PerformanceResult
    {
        public string DB { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public int Quantity { get; set; }
        public ResultType Type { get; set; }
        public TimeSpan TimeSpent { get; set; }

        public double Seconds
        {
            get
            {
                return Math.Round(TimeSpent.TotalSeconds, 1);
            }
        }
        public double QuantityPerSec
        {
            get
            {
                return Math.Round(Quantity / TimeSpent.TotalSeconds, 1);
            }
        }


        public void SaveFile()
        {
            File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{Guid.NewGuid().ToString("N")}.data"), JsonConvert.SerializeObject(this));
        }
    }
}