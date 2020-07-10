namespace O2O.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _06171116 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.T_Order", "ShopNo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.T_Order", "ShopNo");
        }
    }
}
