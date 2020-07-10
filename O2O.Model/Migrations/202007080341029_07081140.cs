namespace O2O.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _07081140 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.T_OrderDtl", "ProdNo", c => c.String(nullable: false, maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.T_OrderDtl", "ProdNo", c => c.String(nullable: false, maxLength: 20, unicode: false));
        }
    }
}
