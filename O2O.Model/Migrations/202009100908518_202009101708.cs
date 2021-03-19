namespace O2O.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _202009101708 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.T_ShopConfig");
            AlterColumn("dbo.T_ShopConfig", "Id", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey("dbo.T_ShopConfig", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.T_ShopConfig");
            AlterColumn("dbo.T_ShopConfig", "Id", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.T_ShopConfig", "Id");
        }
    }
}
