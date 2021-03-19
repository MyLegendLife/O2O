namespace O2O.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _202101271716 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.T_ShopConfig", "MtAutoSync", c => c.Int(nullable: false));
            AddColumn("dbo.T_ShopConfig", "EleAutoSync", c => c.Int(nullable: false));
            DropColumn("dbo.T_ShopConfig", "AutoSync");
        }
        
        public override void Down()
        {
            AddColumn("dbo.T_ShopConfig", "AutoSync", c => c.Int(nullable: false));
            DropColumn("dbo.T_ShopConfig", "EleAutoSync");
            DropColumn("dbo.T_ShopConfig", "MtAutoSync");
        }
    }
}
