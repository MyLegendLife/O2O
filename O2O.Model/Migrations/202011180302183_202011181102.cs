namespace O2O.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _202011181102 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.T_ShopConfigStock", "ShopConfigId", c => c.Guid(nullable: false));
            CreateIndex("dbo.T_ShopConfigStock", "ShopConfigId");
            AddForeignKey("dbo.T_ShopConfigStock", "ShopConfigId", "dbo.T_ShopConfig", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.T_ShopConfigStock", "ShopConfigId", "dbo.T_ShopConfig");
            DropIndex("dbo.T_ShopConfigStock", new[] { "ShopConfigId" });
            DropColumn("dbo.T_ShopConfigStock", "ShopConfigId");
        }
    }
}
