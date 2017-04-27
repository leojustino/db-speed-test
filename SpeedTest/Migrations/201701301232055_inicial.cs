namespace SpeedTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inicial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Basic",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TheData = c.String(maxLength: 120, unicode: false),
                        TheTime = c.DateTime(nullable: false),
                        NoIndexId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 120, unicode: false),
                        NoIndexId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Quantity = c.Int(nullable: false),
                        NoIndexId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomerOrderAssociation",
                c => new
                    {
                        CustomerId = c.Guid(nullable: false),
                        OrderId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.CustomerId, t.OrderId })
                .ForeignKey("dbo.Customer", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.Order", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.CustomerId)
                .Index(t => t.OrderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerOrderAssociation", "OrderId", "dbo.Order");
            DropForeignKey("dbo.CustomerOrderAssociation", "CustomerId", "dbo.Customer");
            DropIndex("dbo.CustomerOrderAssociation", new[] { "OrderId" });
            DropIndex("dbo.CustomerOrderAssociation", new[] { "CustomerId" });
            DropTable("dbo.CustomerOrderAssociation");
            DropTable("dbo.Order");
            DropTable("dbo.Customer");
            DropTable("dbo.Basic");
        }
    }
}
