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
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;
using Constants;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;
using System.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Data.SqlClient;

namespace LauncherApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            progress.Maximum = PerfConstants.CountFillDatabase;
            controls = new Control[] { comboBox1, textBox1, button1, button2, button3, button4, button5, button6, button7 };
        }

        Control[] controls;
        Mutex mutex1 = new Mutex(true, PerfConstants.Mutex1);
        Mutex mutex2 = new Mutex(true, PerfConstants.Mutex2);
        IList<Process> processes = new List<Process>(1000);
        bool blocked1 = true;
        bool blocked2 = true;

        void CreateProcess(string path, string argument)
        {
            processes.Add(Process.Start(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path), argument));
        }

        void StartMongoClients(object sender, EventArgs e)
        {
            var parameter = comboBox1.Text;
            Parallel.For(0, int.Parse(textBox1.Text), a => CreateProcess(@"MongoClient.exe", parameter));
        }
        void StartSqlClients(object sender, EventArgs e)
        {
            var parameter = comboBox1.Text;
            Parallel.For(0, int.Parse(textBox1.Text), a => CreateProcess(@"SqlTestApp.exe", parameter));
        }
        void StartSqlAdoClients(object sender, EventArgs e)
        {
            var parameter = comboBox1.Text;
            Parallel.For(0, int.Parse(textBox1.Text), a => CreateProcess(@"SqlTestAppAdoStyle.exe", parameter));
        }
        void ShowResults(object sender, EventArgs e)
        {
            var list = new List<PerformanceResult>(10000);
            var results = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "results.json");

            if (File.Exists(results))
                list.AddRange(JsonConvert.DeserializeObject<PerformanceResult[]>(File.ReadAllText(results)));

            foreach (var file in Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory, "*.data"))
            {
                list.Add(JsonConvert.DeserializeObject<PerformanceResult>(File.ReadAllText(file)));
                File.Delete(file);
            }

            File.WriteAllText(results, JsonConvert.SerializeObject(list));

            var t1 = new DataTable();
            var t2 = new DataTable();
            var form = new Form2();

            t1.Columns.Add("Client");
            t1.Columns.Add("Operation");
            t1.Columns.Add("Operations", typeof(int));
            t1.Columns.Add("Seconds", typeof(decimal));
            t1.Columns.Add("Ops/Sec", typeof(decimal));
            t1.Columns.Add("Start Time", typeof(DateTime));

            t2.Columns.Add("Client");
            t2.Columns.Add("Operation");
            t2.Columns.Add("Clients", typeof(int));
            t2.Columns.Add("Operations", typeof(decimal));
            t2.Columns.Add("Seconds", typeof(decimal));
            t2.Columns.Add("Ops/Sec", typeof(decimal));

            var t3 = list
                .GroupBy(a => a.DB)
                .Select(a => new
                {
                    DB = a.Key,
                    Results = a
                        .GroupBy(b => b.Type)
                        .Select(b => new { Type = b.Key, Results = b })
                });

            foreach (var obj in list)
                t1.Rows.Add(obj.DB, obj.Type, obj.Quantity, obj.Seconds, obj.QuantityPerSec, obj.StartTime);

            foreach (var item1 in t3)
                foreach (var item2 in item1.Results)
                    t2.Rows.Add(item1.DB, item2.Type, item2.Results.Count(), item2.Results.Sum(a => a.Quantity), item2.Results.Average(a => a.Seconds), item2.Results.Average(a => a.QuantityPerSec));

            form.dataGridView1.DataSource = t1;
            form.dataGridView2.DataSource = t2;

            ConfigureDataGridViewColumnAsNumber(form.dataGridView1.Columns["Operations"], "#,0");
            ConfigureDataGridViewColumnAsNumber(form.dataGridView1.Columns["Seconds"]);
            ConfigureDataGridViewColumnAsNumber(form.dataGridView1.Columns["Ops/Sec"]);
            ConfigureDataGridViewColumnAsNumber(form.dataGridView1.Columns["Start Time"], "dd/MM/yyyy hh:mm:ss.fff");

            ConfigureDataGridViewColumnAsNumber(form.dataGridView2.Columns["Clients"], "#,0");
            ConfigureDataGridViewColumnAsNumber(form.dataGridView2.Columns["Operations"], "#,0");
            ConfigureDataGridViewColumnAsNumber(form.dataGridView2.Columns["Seconds"]);
            ConfigureDataGridViewColumnAsNumber(form.dataGridView2.Columns["Ops/Sec"]);

            form.FormClosed += (a, b) =>
            {
                ((Form)a).Dispose();
                t1.Dispose();
                t2.Dispose();
            };

            form.Show();
        }
        void ReleaseMutex1(object sender, EventArgs e)
        {
            if (blocked1)
            {
                mutex1.ReleaseMutex();
                button1.Text = "Block Mutex 1";
            }
            else
            {
                mutex1.WaitOne();
                button1.Text = "Release Mutex 1!";
            }

            blocked1 = !blocked1;
        }
        void ReleaseMutex2(object sender, EventArgs e)
        {
            if (blocked2)
            {
                mutex2.ReleaseMutex();
                button8.Text = "Block Mutex 2";
            }
            else
            {
                mutex2.WaitOne();
                button8.Text = "Release Mutex 2!";
            }

            blocked1 = !blocked1;
        }
        void ConfigureMongoDB(object sender, EventArgs e)
        {
            worker.DoWork += (a, b) =>
            {
                worker.ReportProgress(0);

                var w = Stopwatch.StartNew();

                try
                {
                    SpeedyMongoDataContext.Customer.DeleteMany(new BsonDocument());

                    for (int i = 0; i < PerfConstants.CountFillDatabase; i++)
                    {
                        SpeedyMongoDataContext.Customer.InsertOne(new
                        {
                            Name = "Cust " + (1 + i),
                            Orders = new[]
                                {
                                    new { _id = ObjectId.GenerateNewId(), Quantity = 1, },
                                    new { _id = ObjectId.GenerateNewId(), Quantity = 2, },
                                },
                        });
                        worker.ReportProgress(1);
                    }
                }
                finally
                {
                    w.Stop();
                    worker.ReportProgress(2, w.Elapsed.TotalSeconds);
                }
            };
            worker.RunWorkerAsync();
        }
        void ConfigureSqlServer(object sender, EventArgs e)
        {
            worker.DoWork += (a, b) =>
            {
                worker.ReportProgress(0);

                var w = Stopwatch.StartNew();

                try
                {
                    using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SpeedyConnectionString"].ConnectionString))
                    using (var delete = conn.CreateCommand())
                    using (var customer = conn.CreateCommand())
                    using (var orders = conn.CreateCommand())
                    using (var customerToOrders = conn.CreateCommand())
                    {
                        conn.Open();

                        delete.CommandText = @"delete from [customertoorders]delete from [customer]delete from [orders]";
                        orders.CommandText = @"insert into [orders]([id],[quantity])VALUES(@id,@quantity)";
                        customer.CommandText = @"insert into [customer]([id],[name])VALUES(@id,@name)";
                        customerToOrders.CommandText = @"insert into [customertoorders]([id],[orderid],[customerid])VALUES(@id,@orderid,@customerid)";

                        customer.Parameters.Add("@id", SqlDbType.UniqueIdentifier);
                        customer.Parameters.Add("@name", SqlDbType.VarChar);
                        orders.Parameters.Add("@id", SqlDbType.UniqueIdentifier);
                        orders.Parameters.Add("@quantity", SqlDbType.Int);
                        customerToOrders.Parameters.Add("@id", SqlDbType.UniqueIdentifier);
                        customerToOrders.Parameters.Add("@orderid", SqlDbType.UniqueIdentifier);
                        customerToOrders.Parameters.Add("@customerid", SqlDbType.UniqueIdentifier);

                        delete.ExecuteNonQuery();

                        for (int i = 0; i < PerfConstants.CountFillDatabase; i++)
                        {
                            var customerId = Guid.NewGuid();
                            var orderId = Guid.NewGuid();

                            customer.Parameters["@id"].Value = customerId;
                            customer.Parameters["@name"].Value = "Cust " + (1 + i);
                            customer.ExecuteNonQuery();

                            orders.Parameters["@id"].Value = orderId;
                            orders.Parameters["@quantity"].Value = 1;
                            orders.ExecuteNonQuery();

                            customerToOrders.Parameters["@id"].Value = Guid.NewGuid();
                            customerToOrders.Parameters["@customerid"].Value = customerId;
                            customerToOrders.Parameters["@orderid"].Value = orderId;
                            customerToOrders.ExecuteNonQuery();

                            orders.Parameters["@id"].Value = orderId = Guid.NewGuid();
                            orders.Parameters["@quantity"].Value = 2;
                            orders.ExecuteNonQuery();

                            customerToOrders.Parameters["@id"].Value = Guid.NewGuid();
                            customerToOrders.Parameters["@customerid"].Value = customerId;
                            customerToOrders.Parameters["@orderid"].Value = orderId;
                            customerToOrders.ExecuteNonQuery();
                            worker.ReportProgress(1);
                        }
                    }
                }
                finally
                {
                    w.Stop();
                    worker.ReportProgress(2, w.Elapsed.TotalSeconds);
                }
            };
            worker.RunWorkerAsync();
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            switch (e.ProgressPercentage)
            {
                case 0:
                    status.Text = "";
                    progress.Value = 1;

                    foreach (var item in controls)
                        item.Enabled = false;
                    break;
                case 1:
                    progress.PerformStep();
                    break;
                case 2:
                    status.Text = $"completed in {Math.Round((double)e.UserState, 1)} seconds";
                    progress.Value = 1;

                    foreach (var item in controls)
                        item.Enabled = true;
                    break;
            }
        }

        void ConfigureDataGridViewColumnAsNumber(DataGridViewColumn column, string format = "#,0.0", DataGridViewContentAlignment alignment = DataGridViewContentAlignment.MiddleRight)
        {
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            column.DefaultCellStyle.Alignment = alignment;
            column.DefaultCellStyle.Format = format;
        }
    }



    public class SpeedyMongoDataContext
    {
        public static MongoClient client = new MongoClient(ConfigurationManager.AppSettings["mongoDbConnection"]);
        public static IMongoDatabase db = client.GetDatabase("Speedy");

        public static IMongoCollection<object> Basic
        {
            get
            {
                return db.GetCollection<object>("Basic");
            }
        }
        public static IMongoCollection<object> Customer
        {
            get
            {
                return db.GetCollection<object>("Customer");
            }
        }
    }
}