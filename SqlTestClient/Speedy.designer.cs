﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SqlTestApp
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="Speedy")]
	public partial class SpeedyDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertBasic(Basic instance);
    partial void UpdateBasic(Basic instance);
    partial void DeleteBasic(Basic instance);
    partial void InsertCustomer(Customer instance);
    partial void UpdateCustomer(Customer instance);
    partial void DeleteCustomer(Customer instance);
    partial void InsertOrder(Order instance);
    partial void UpdateOrder(Order instance);
    partial void DeleteOrder(Order instance);
    partial void InsertCustomerToOrder(CustomerToOrder instance);
    partial void UpdateCustomerToOrder(CustomerToOrder instance);
    partial void DeleteCustomerToOrder(CustomerToOrder instance);
    partial void InsertSimplerKeysBasic(SimplerKeysBasic instance);
    partial void UpdateSimplerKeysBasic(SimplerKeysBasic instance);
    partial void DeleteSimplerKeysBasic(SimplerKeysBasic instance);
    #endregion
		
		public SpeedyDataContext() : 
				base(global::SqlTestApp.Properties.Settings.Default.SpeedyConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public SpeedyDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public SpeedyDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public SpeedyDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public SpeedyDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Basic> Basics
		{
			get
			{
				return this.GetTable<Basic>();
			}
		}
		
		public System.Data.Linq.Table<Customer> Customers
		{
			get
			{
				return this.GetTable<Customer>();
			}
		}
		
		public System.Data.Linq.Table<Order> Orders
		{
			get
			{
				return this.GetTable<Order>();
			}
		}
		
		public System.Data.Linq.Table<CustomerToOrder> CustomerToOrders
		{
			get
			{
				return this.GetTable<CustomerToOrder>();
			}
		}
		
		public System.Data.Linq.Table<SimplerKeysBasic> SimplerKeysBasics
		{
			get
			{
				return this.GetTable<SimplerKeysBasic>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Basic")]
	public partial class Basic : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private string _id;
		
		private string _TheData;
		
		private System.DateTime _TheTime;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnidChanging(string value);
    partial void OnidChanged();
    partial void OnTheDataChanging(string value);
    partial void OnTheDataChanged();
    partial void OnTheTimeChanging(System.DateTime value);
    partial void OnTheTimeChanged();
    #endregion
		
		public Basic()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_id", DbType="Char(24) NOT NULL", CanBeNull=false, IsPrimaryKey=true)]
		public string id
		{
			get
			{
				return this._id;
			}
			set
			{
				if ((this._id != value))
				{
					this.OnidChanging(value);
					this.SendPropertyChanging();
					this._id = value;
					this.SendPropertyChanged("id");
					this.OnidChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TheData", DbType="NVarChar(500) NOT NULL", CanBeNull=false)]
		public string TheData
		{
			get
			{
				return this._TheData;
			}
			set
			{
				if ((this._TheData != value))
				{
					this.OnTheDataChanging(value);
					this.SendPropertyChanging();
					this._TheData = value;
					this.SendPropertyChanged("TheData");
					this.OnTheDataChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TheTime", DbType="DateTime NOT NULL")]
		public System.DateTime TheTime
		{
			get
			{
				return this._TheTime;
			}
			set
			{
				if ((this._TheTime != value))
				{
					this.OnTheTimeChanging(value);
					this.SendPropertyChanging();
					this._TheTime = value;
					this.SendPropertyChanged("TheTime");
					this.OnTheTimeChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Customer")]
	public partial class Customer : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private System.Guid _ID;
		
		private string _Name;
		
		private EntitySet<CustomerToOrder> _CustomerToOrders;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(System.Guid value);
    partial void OnIDChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    #endregion
		
		public Customer()
		{
			this._CustomerToOrders = new EntitySet<CustomerToOrder>(new Action<CustomerToOrder>(this.attach_CustomerToOrders), new Action<CustomerToOrder>(this.detach_CustomerToOrders));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ID", DbType="UniqueIdentifier NOT NULL", IsPrimaryKey=true)]
		public System.Guid ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="NVarChar(10) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Customer_CustomerToOrder", Storage="_CustomerToOrders", ThisKey="ID", OtherKey="CustomerID")]
		public EntitySet<CustomerToOrder> CustomerToOrders
		{
			get
			{
				return this._CustomerToOrders;
			}
			set
			{
				this._CustomerToOrders.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_CustomerToOrders(CustomerToOrder entity)
		{
			this.SendPropertyChanging();
			entity.Customer = this;
		}
		
		private void detach_CustomerToOrders(CustomerToOrder entity)
		{
			this.SendPropertyChanging();
			entity.Customer = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Orders")]
	public partial class Order : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private System.Guid _ID;
		
		private int _Quantity;
		
		private EntitySet<CustomerToOrder> _CustomerToOrders;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(System.Guid value);
    partial void OnIDChanged();
    partial void OnQuantityChanging(int value);
    partial void OnQuantityChanged();
    #endregion
		
		public Order()
		{
			this._CustomerToOrders = new EntitySet<CustomerToOrder>(new Action<CustomerToOrder>(this.attach_CustomerToOrders), new Action<CustomerToOrder>(this.detach_CustomerToOrders));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ID", DbType="UniqueIdentifier NOT NULL", IsPrimaryKey=true)]
		public System.Guid ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Quantity", DbType="Int NOT NULL")]
		public int Quantity
		{
			get
			{
				return this._Quantity;
			}
			set
			{
				if ((this._Quantity != value))
				{
					this.OnQuantityChanging(value);
					this.SendPropertyChanging();
					this._Quantity = value;
					this.SendPropertyChanged("Quantity");
					this.OnQuantityChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Order_CustomerToOrder", Storage="_CustomerToOrders", ThisKey="ID", OtherKey="OrderId")]
		public EntitySet<CustomerToOrder> CustomerToOrders
		{
			get
			{
				return this._CustomerToOrders;
			}
			set
			{
				this._CustomerToOrders.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_CustomerToOrders(CustomerToOrder entity)
		{
			this.SendPropertyChanging();
			entity.Order = this;
		}
		
		private void detach_CustomerToOrders(CustomerToOrder entity)
		{
			this.SendPropertyChanging();
			entity.Order = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.CustomerToOrders")]
	public partial class CustomerToOrder : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private System.Guid _ID;
		
		private System.Guid _OrderId;
		
		private System.Guid _CustomerID;
		
		private EntityRef<Customer> _Customer;
		
		private EntityRef<Order> _Order;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(System.Guid value);
    partial void OnIDChanged();
    partial void OnOrderIdChanging(System.Guid value);
    partial void OnOrderIdChanged();
    partial void OnCustomerIDChanging(System.Guid value);
    partial void OnCustomerIDChanged();
    #endregion
		
		public CustomerToOrder()
		{
			this._Customer = default(EntityRef<Customer>);
			this._Order = default(EntityRef<Order>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ID", DbType="UniqueIdentifier NOT NULL", IsPrimaryKey=true)]
		public System.Guid ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_OrderId", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid OrderId
		{
			get
			{
				return this._OrderId;
			}
			set
			{
				if ((this._OrderId != value))
				{
					if (this._Order.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnOrderIdChanging(value);
					this.SendPropertyChanging();
					this._OrderId = value;
					this.SendPropertyChanged("OrderId");
					this.OnOrderIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CustomerID", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid CustomerID
		{
			get
			{
				return this._CustomerID;
			}
			set
			{
				if ((this._CustomerID != value))
				{
					if (this._Customer.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnCustomerIDChanging(value);
					this.SendPropertyChanging();
					this._CustomerID = value;
					this.SendPropertyChanged("CustomerID");
					this.OnCustomerIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Customer_CustomerToOrder", Storage="_Customer", ThisKey="CustomerID", OtherKey="ID", IsForeignKey=true)]
		public Customer Customer
		{
			get
			{
				return this._Customer.Entity;
			}
			set
			{
				Customer previousValue = this._Customer.Entity;
				if (((previousValue != value) 
							|| (this._Customer.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Customer.Entity = null;
						previousValue.CustomerToOrders.Remove(this);
					}
					this._Customer.Entity = value;
					if ((value != null))
					{
						value.CustomerToOrders.Add(this);
						this._CustomerID = value.ID;
					}
					else
					{
						this._CustomerID = default(System.Guid);
					}
					this.SendPropertyChanged("Customer");
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Order_CustomerToOrder", Storage="_Order", ThisKey="OrderId", OtherKey="ID", IsForeignKey=true)]
		public Order Order
		{
			get
			{
				return this._Order.Entity;
			}
			set
			{
				Order previousValue = this._Order.Entity;
				if (((previousValue != value) 
							|| (this._Order.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Order.Entity = null;
						previousValue.CustomerToOrders.Remove(this);
					}
					this._Order.Entity = value;
					if ((value != null))
					{
						value.CustomerToOrders.Add(this);
						this._OrderId = value.ID;
					}
					else
					{
						this._OrderId = default(System.Guid);
					}
					this.SendPropertyChanged("Order");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.SimplerKeysBasic")]
	public partial class SimplerKeysBasic : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private string _MongoDBeskID;
		
		private string _TheData;
		
		private System.DateTime _TheTime;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnMongoDBeskIDChanging(string value);
    partial void OnMongoDBeskIDChanged();
    partial void OnTheDataChanging(string value);
    partial void OnTheDataChanged();
    partial void OnTheTimeChanging(System.DateTime value);
    partial void OnTheTimeChanged();
    #endregion
		
		public SimplerKeysBasic()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MongoDBeskID", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string MongoDBeskID
		{
			get
			{
				return this._MongoDBeskID;
			}
			set
			{
				if ((this._MongoDBeskID != value))
				{
					this.OnMongoDBeskIDChanging(value);
					this.SendPropertyChanging();
					this._MongoDBeskID = value;
					this.SendPropertyChanged("MongoDBeskID");
					this.OnMongoDBeskIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TheData", DbType="NVarChar(500) NOT NULL", CanBeNull=false)]
		public string TheData
		{
			get
			{
				return this._TheData;
			}
			set
			{
				if ((this._TheData != value))
				{
					this.OnTheDataChanging(value);
					this.SendPropertyChanging();
					this._TheData = value;
					this.SendPropertyChanged("TheData");
					this.OnTheDataChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TheTime", DbType="DateTime NOT NULL")]
		public System.DateTime TheTime
		{
			get
			{
				return this._TheTime;
			}
			set
			{
				if ((this._TheTime != value))
				{
					this.OnTheTimeChanging(value);
					this.SendPropertyChanging();
					this._TheTime = value;
					this.SendPropertyChanged("TheTime");
					this.OnTheTimeChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
