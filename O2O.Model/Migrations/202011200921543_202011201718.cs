namespace O2O.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _202011201718 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.T_StockRuleProd", "StockRuleId", "dbo.T_StockRule");
            DropForeignKey("dbo.T_StockRuleShop", "StockRuleId", "dbo.T_StockRule");
            DropPrimaryKey("dbo.T_StockRule");
            DropPrimaryKey("dbo.T_StockRuleProd");
            DropPrimaryKey("dbo.T_StockRuleShop");
            AlterColumn("dbo.T_StockRule", "Id", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.T_StockRuleProd", "Id", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.T_StockRuleShop", "Id", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey("dbo.T_StockRule", "Id");
            AddPrimaryKey("dbo.T_StockRuleProd", "Id");
            AddPrimaryKey("dbo.T_StockRuleShop", "Id");
            AddForeignKey("dbo.T_StockRuleProd", "StockRuleId", "dbo.T_StockRule", "Id", cascadeDelete: true);
            AddForeignKey("dbo.T_StockRuleShop", "StockRuleId", "dbo.T_StockRule", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.T_StockRuleShop", "StockRuleId", "dbo.T_StockRule");
            DropForeignKey("dbo.T_StockRuleProd", "StockRuleId", "dbo.T_StockRule");
            DropPrimaryKey("dbo.T_StockRuleShop");
            DropPrimaryKey("dbo.T_StockRuleProd");
            DropPrimaryKey("dbo.T_StockRule");
            AlterColumn("dbo.T_StockRuleShop", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.T_StockRuleProd", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.T_StockRule", "Id", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.T_StockRuleShop", "Id");
            AddPrimaryKey("dbo.T_StockRuleProd", "Id");
            AddPrimaryKey("dbo.T_StockRule", "Id");
            AddForeignKey("dbo.T_StockRuleShop", "StockRuleId", "dbo.T_StockRule", "Id", cascadeDelete: true);
            AddForeignKey("dbo.T_StockRuleProd", "StockRuleId", "dbo.T_StockRule", "Id", cascadeDelete: true);
        }
    }
}
