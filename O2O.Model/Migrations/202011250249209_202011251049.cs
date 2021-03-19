namespace O2O.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _202011251049 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.T_ShopConfig", "AutoSale", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.T_ShopConfig", "AutoSale");
        }
    }
}
