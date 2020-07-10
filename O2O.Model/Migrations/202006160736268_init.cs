namespace O2O.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ele_Account",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        AccountNo = c.String(nullable: false, maxLength: 20, unicode: false),
                        AccountName = c.String(nullable: false, maxLength: 20, unicode: false),
                        AccessToken = c.String(nullable: false, maxLength: 40, unicode: false),
                        TokenType = c.String(maxLength: 10, unicode: false),
                        RefreshToken = c.String(nullable: false, maxLength: 40, unicode: false),
                        Scope = c.String(maxLength: 10, unicode: false),
                        ExpiresDate = c.DateTime(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.T_User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Ele_Shop",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ShopNo = c.String(nullable: false, maxLength: 10, unicode: false),
                        ShopId = c.Long(nullable: false),
                        AccountId = c.Guid(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ele_Account", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.AccountId);
            
            CreateTable(
                "dbo.T_User",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(nullable: false, maxLength: 50, unicode: false),
                        LoginName = c.String(nullable: false, maxLength: 50, unicode: false),
                        PasswordHash = c.String(nullable: false, maxLength: 50, unicode: false),
                        PasswordSalt = c.String(nullable: false, maxLength: 50, unicode: false),
                        ConnString = c.String(nullable: false, maxLength: 500, unicode: false),
                        Ket = c.String(nullable: false, maxLength: 20, unicode: false),
                        Description = c.String(maxLength: 100, unicode: false),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Mt_Account",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        AccountNo = c.String(nullable: false, maxLength: 20, unicode: false),
                        AccountName = c.String(nullable: false, maxLength: 50, unicode: false),
                        Description = c.String(maxLength: 200, unicode: false),
                        WaimaiAppId = c.String(nullable: false, maxLength: 20, unicode: false),
                        WaimaiAppSecret = c.String(nullable: false, maxLength: 50, unicode: false),
                        TuangouAppKey = c.String(nullable: false, maxLength: 50, unicode: false),
                        TuangouAppSecret = c.String(nullable: false, maxLength: 50, unicode: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.T_User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.T_Order",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TakeType = c.Int(nullable: false),
                        OrderId = c.String(nullable: false, maxLength: 20, unicode: false),
                        TtlPrice = c.Double(nullable: false),
                        Consume = c.Double(nullable: false),
                        UserName = c.String(maxLength: 100, unicode: false),
                        UserMobile = c.String(maxLength: 100, unicode: false),
                        DeliverTime = c.DateTime(nullable: false),
                        DeliverAddress = c.String(maxLength: 500, unicode: false),
                        DeliverFee = c.Double(nullable: false),
                        MemoStr = c.String(),
                        OptTime = c.DateTime(nullable: false),
                        PayType = c.Int(nullable: false),
                        State = c.Int(nullable: false),
                        CancelCode = c.String(maxLength: 20, unicode: false),
                        CancelReason = c.String(maxLength: 100, unicode: false),
                        RefundCode = c.String(maxLength: 20, unicode: false),
                        RefundReason = c.String(maxLength: 100, unicode: false),
                        BuyState = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 20, unicode: false),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.T_OrderDtl",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ProdNo = c.String(nullable: false, maxLength: 20, unicode: false),
                        ProdName = c.String(nullable: false, maxLength: 50, unicode: false),
                        ProdUnit = c.String(nullable: false, maxLength: 10, unicode: false),
                        Price = c.Double(nullable: false),
                        ItemCnt = c.Double(nullable: false),
                        ItemSum = c.Double(nullable: false),
                        OrderId = c.Guid(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.T_Order", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.T_OrderDtl", "OrderId", "dbo.T_Order");
            DropForeignKey("dbo.Ele_Account", "UserId", "dbo.T_User");
            DropForeignKey("dbo.Mt_Account", "UserId", "dbo.T_User");
            DropForeignKey("dbo.Ele_Shop", "AccountId", "dbo.Ele_Account");
            DropIndex("dbo.T_OrderDtl", new[] { "OrderId" });
            DropIndex("dbo.Mt_Account", new[] { "UserId" });
            DropIndex("dbo.Ele_Shop", new[] { "AccountId" });
            DropIndex("dbo.Ele_Account", new[] { "UserId" });
            DropTable("dbo.T_OrderDtl");
            DropTable("dbo.T_Order");
            DropTable("dbo.Mt_Account");
            DropTable("dbo.T_User");
            DropTable("dbo.Ele_Shop");
            DropTable("dbo.Ele_Account");
        }
    }
}
