namespace O2O.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ele_Config",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        AccessToken = c.String(nullable: false, maxLength: 50, unicode: false),
                        RefreshToken = c.String(nullable: false, maxLength: 50, unicode: false),
                        ExpiresDate = c.DateTime(nullable: false),
                        Sign = c.String(nullable: false, maxLength: 20, unicode: false),
                        UserId = c.Guid(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.T_User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.T_User",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        UserNo = c.String(nullable: false, maxLength: 20, unicode: false),
                        UserName = c.String(nullable: false, maxLength: 50, unicode: false),
                        ConnString = c.String(nullable: false, maxLength: 100, unicode: false),
                        Ket = c.String(nullable: false, maxLength: 20, unicode: false),
                        Description = c.String(maxLength: 100, unicode: false),
                        MtConfigId = c.Guid(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Mt_Config", t => t.MtConfigId, cascadeDelete: true)
                .Index(t => t.MtConfigId);
            
            CreateTable(
                "dbo.Ele_Shop",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ShopId = c.String(nullable: false, maxLength: 20, unicode: false),
                        ShopName = c.String(nullable: false, maxLength: 50, unicode: false),
                        ShopNo = c.String(nullable: false, maxLength: 20, unicode: false),
                        Sign = c.String(nullable: false, maxLength: 10, unicode: false),
                        UserId = c.Guid(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.T_User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Mt_Config",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        WaimaiAppId = c.String(nullable: false, maxLength: 20, unicode: false),
                        WaimaiAppSecret = c.String(nullable: false, maxLength: 50, unicode: false),
                        TuangouAppKey = c.String(nullable: false, maxLength: 50, unicode: false),
                        TuangouAppSecret = c.String(nullable: false, maxLength: 50, unicode: false),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Mt_Shop",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PoiId = c.String(nullable: false, maxLength: 20, unicode: false),
                        PoiName = c.String(nullable: false, maxLength: 50, unicode: false),
                        AppAuthToken = c.String(nullable: false, maxLength: 100, unicode: false),
                        BusinessType = c.Int(nullable: false),
                        ShopNo = c.String(nullable: false, maxLength: 10, unicode: false),
                        UserId = c.Guid(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.T_User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Ele_Order",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        AA = c.String(nullable: false, maxLength: 20, unicode: false),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Mt_Order",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        OrderId = c.String(nullable: false, maxLength: 30, unicode: false),
                        OrderIdView = c.String(nullable: false, maxLength: 30, unicode: false),
                        Caution = c.String(maxLength: 200, unicode: false),
                        CTime = c.DateTime(nullable: false),
                        UTime = c.DateTime(nullable: false),
                        DaySeq = c.Int(nullable: false),
                        DeliveryTime = c.DateTime(nullable: false),
                        OriginalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RecipientAddress = c.String(maxLength: 200, unicode: false),
                        RecipientName = c.String(maxLength: 20, unicode: false),
                        RecipientPhone = c.String(maxLength: 20, unicode: false),
                        ShippingPhone = c.String(maxLength: 20, unicode: false),
                        ShippingFee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsSaled = c.Int(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Mt_OrderDtl",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        SkuId = c.String(nullable: false, maxLength: 30, unicode: false),
                        FoodName = c.String(nullable: false, maxLength: 30, unicode: false),
                        Quantity = c.Int(nullable: false),
                        BoxNum = c.Int(nullable: false),
                        BoxPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Unit = c.String(maxLength: 10, unicode: false),
                        FoodDiscount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Spec = c.String(maxLength: 30, unicode: false),
                        FoodPreperty = c.String(maxLength: 30, unicode: false),
                        OrderId = c.Guid(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Mt_Order", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Mt_OrderDtl", "OrderId", "dbo.Mt_Order");
            DropForeignKey("dbo.Mt_Shop", "UserId", "dbo.T_User");
            DropForeignKey("dbo.T_User", "MtConfigId", "dbo.Mt_Config");
            DropForeignKey("dbo.Ele_Shop", "UserId", "dbo.T_User");
            DropForeignKey("dbo.Ele_Config", "UserId", "dbo.T_User");
            DropIndex("dbo.Mt_OrderDtl", new[] { "OrderId" });
            DropIndex("dbo.Mt_Shop", new[] { "UserId" });
            DropIndex("dbo.Ele_Shop", new[] { "UserId" });
            DropIndex("dbo.T_User", new[] { "MtConfigId" });
            DropIndex("dbo.Ele_Config", new[] { "UserId" });
            DropTable("dbo.Mt_OrderDtl");
            DropTable("dbo.Mt_Order");
            DropTable("dbo.Ele_Order");
            DropTable("dbo.Mt_Shop");
            DropTable("dbo.Mt_Config");
            DropTable("dbo.Ele_Shop");
            DropTable("dbo.T_User");
            DropTable("dbo.Ele_Config");
        }
    }
}
