namespace O2O.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _202012241707 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.T_OrderDtl", "ProdName", c => c.String(nullable: false, maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.T_OrderDtl", "ProdName", c => c.String(nullable: false, maxLength: 50, unicode: false));
        }
    }
}
