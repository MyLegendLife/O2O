namespace O2O.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _202101271635 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.T_ShopConfig", "AutoSync", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.T_ShopConfig", "AutoSync");
        }
    }
}
