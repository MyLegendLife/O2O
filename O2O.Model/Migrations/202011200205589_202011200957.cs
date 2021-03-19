namespace O2O.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _202011200957 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.T_ShopConfigStock", "ShopConfigId", "dbo.T_ShopConfig");
            DropIndex("dbo.T_ShopConfigStock", new[] { "ShopConfigId" });
            CreateTable(
                "dbo.T_StockRule",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 50, unicode: false),
                        RuleName = c.String(nullable: false, maxLength: 50, unicode: false),
                        Description = c.String(maxLength: 200, unicode: false),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.T_StockRuleProd",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ProdNo = c.String(nullable: false, maxLength: 50, unicode: false),
                        ProdName = c.String(nullable: false, maxLength: 50, unicode: false),
                        MtStock = c.Double(nullable: false),
                        EleStock = c.Double(nullable: false),
                        StockRuleId = c.Guid(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.T_StockRule", t => t.StockRuleId, cascadeDelete: true)
                .Index(t => t.StockRuleId);
            
            CreateTable(
                "dbo.T_StockRuleShop",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ShopNo = c.String(nullable: false, maxLength: 50, unicode: false),
                        StockRuleId = c.Guid(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.T_StockRule", t => t.StockRuleId, cascadeDelete: true)
                .Index(t => t.StockRuleId);
            
            DropColumn("dbo.T_ShopConfig", "MtAutoUpDown");
            DropColumn("dbo.T_ShopConfig", "EleAutoUpDown");
            DropTable("dbo.T_ShopConfigStock");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.T_ShopConfigStock",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 50, unicode: false),
                        ShopNo = c.String(nullable: false, maxLength: 50, unicode: false),
                        ProdId = c.String(nullable: false, maxLength: 50, unicode: false),
                        ProdNo = c.String(nullable: false, maxLength: 50, unicode: false),
                        SafeStock = c.Double(nullable: false),
                        TakeType = c.Int(nullable: false),
                        ShopConfigId = c.Guid(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.T_ShopConfig", "EleAutoUpDown", c => c.Int(nullable: false));
            AddColumn("dbo.T_ShopConfig", "MtAutoUpDown", c => c.Int(nullable: false));
            DropForeignKey("dbo.T_StockRuleShop", "StockRuleId", "dbo.T_StockRule");
            DropForeignKey("dbo.T_StockRuleProd", "StockRuleId", "dbo.T_StockRule");
            DropIndex("dbo.T_StockRuleShop", new[] { "StockRuleId" });
            DropIndex("dbo.T_StockRuleProd", new[] { "StockRuleId" });
            DropTable("dbo.T_StockRuleShop");
            DropTable("dbo.T_StockRuleProd");
            DropTable("dbo.T_StockRule");
            CreateIndex("dbo.T_ShopConfigStock", "ShopConfigId");
            AddForeignKey("dbo.T_ShopConfigStock", "ShopConfigId", "dbo.T_ShopConfig", "Id", cascadeDelete: true);
        }
    }
}
