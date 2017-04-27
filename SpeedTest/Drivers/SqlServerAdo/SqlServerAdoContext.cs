using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpeedTest.Entities;

namespace SpeedTest.Drivers.SqlServerAdo
{
    class SqlServerAdoContext
    {
        static SqlConnection NewConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServerSpeedTestConnection"].ConnectionString);
        }

        public void InsertManyBasics(IList<Basic> basics)
        {
            using (var conn = NewConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                cmd.CommandText = @"insert into [Basic]([Id],[NoIndexId],[TheData],[TheTime])values(@1,@2,@3,@4)";

                cmd.Parameters.Add("@1", SqlDbType.UniqueIdentifier);
                cmd.Parameters.Add("@2", SqlDbType.UniqueIdentifier);
                cmd.Parameters.Add("@3", SqlDbType.VarChar);
                cmd.Parameters.Add("@4", SqlDbType.DateTime);

                foreach (var basic in basics)
                {
                    cmd.Parameters["@1"].Value = basic.Id;
                    cmd.Parameters["@2"].Value = basic.NoIndexId;
                    cmd.Parameters["@3"].Value = basic.TheData;
                    cmd.Parameters["@4"].Value = basic.TheTime;

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void InsertManyCustomers(IList<Customer> customers)
        {
            using (var conn = NewConnection())
            using (var cmd1 = conn.CreateCommand())
            using (var cmd2 = conn.CreateCommand())
            using (var cmd3 = conn.CreateCommand())
            {
                conn.Open();

                cmd1.CommandText = "insert into [Customer]([Id],[NoIndexId],[Name])values(@1,@2,@3)";
                cmd2.CommandText = "insert into [Order]([Id],[NoIndexId],[Quantity])values(@1,@2,@3)";
                cmd2.CommandText = "insert into [CustomerOrderAssociation]([CustomerId],[OrderId])values(@1,@2)";

                cmd1.Parameters.Add("@1", SqlDbType.UniqueIdentifier);
                cmd1.Parameters.Add("@2", SqlDbType.UniqueIdentifier);
                cmd1.Parameters.Add("@3", SqlDbType.VarChar);

                cmd2.Parameters.Add("@1", SqlDbType.UniqueIdentifier);
                cmd2.Parameters.Add("@2", SqlDbType.UniqueIdentifier);
                cmd2.Parameters.Add("@3", SqlDbType.Int);

                cmd3.Parameters.Add("@1", SqlDbType.UniqueIdentifier);
                cmd3.Parameters.Add("@2", SqlDbType.UniqueIdentifier);

                foreach (var customer in customers)
                {
                    var transaction = conn.BeginTransaction();

                    cmd1.Parameters["@1"].Value = customer.Id;
                    cmd1.Parameters["@2"].Value = customer.NoIndexId;
                    cmd1.Parameters["@3"].Value = customer.Name;

                    cmd1.ExecuteNonQuery();

                    foreach (var order in customer.Orders)
                    {
                        cmd2.Parameters["@1"].Value = order.Id;
                        cmd2.Parameters["@2"].Value = order.NoIndexId;
                        cmd2.Parameters["@3"].Value = order.Quantity;

                        cmd2.ExecuteNonQuery();

                        cmd3.Parameters["@1"].Value = customer.Id;
                        cmd3.Parameters["@2"].Value = order.Id;

                        cmd3.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
            }
        }

        public void DeleteCustomers()
        {
            using (var conn = NewConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                cmd.CommandText = "delete from [Customer]";

                cmd.ExecuteNonQuery();
            }
        }

        public IList<Customer> QueryCustomers(int limit)
        {
            using (var conn = NewConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                cmd.CommandText = @"SELECT TOP @1 c.[Id],c.[NoIndexId],c.[Name],o.[Id],o.[NoIndexId],o.[Quantity]
                        FROM [Customer] c
                          left join [CustomerOrderAssociation] a on a.CustomerId = c.Id
                          inner join [Order] o on o.id = a.OrderId";

                cmd.Parameters.AddWithValue("@1", limit);

                var ret = new Dictionary<Guid, Customer>(limit);

                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        var id = reader.GetGuid(0);
                        var orderid = reader.GetSqlGuid(3);
                        var customer = (Customer)null;

                        if (ret.ContainsKey(id))
                            customer = ret[id];
                        else
                            ret.Add(id, customer = new Customer
                            {
                                Id = id,
                                NoIndexId = reader.GetGuid(1),
                                Name = reader.GetString(2),
                            });  

                        if (!orderid.IsNull)
                            customer.Orders.Add(new Order
                            {
                                Id = orderid.Value,
                                NoIndexId = reader.GetGuid(4),
                                Quantity = reader.GetInt32(5)
                            });
                    }

                return ret.Values.ToList();
            }
        }

        public void UpdateIndexedCustomers(IList<Customer> customers)
        {
            using (var conn = NewConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"update [Customer] set [Name]='New name' where [Id]=@1";

                cmd.Parameters.Add("@1", SqlDbType.UniqueIdentifier);

                foreach (var customer in customers)
                {
                    cmd.Parameters["@1"].Value = customer.Id;

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateUnindexedCustomers(IList<Customer> customers)
        {
            using (var conn = NewConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"update [Customer] set [Name]='New name' where [NoIndexId]=@1";

                cmd.Parameters.Add("@1", SqlDbType.UniqueIdentifier);

                foreach (var customer in customers)
                {
                    cmd.Parameters["@1"].Value = customer.NoIndexId;

                    cmd.ExecuteNonQuery();
                }

            }
        }
    }
}
