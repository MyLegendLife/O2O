namespace O2O.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 增加OrderType字段 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.T_Order", "OrderType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.T_Order", "OrderType");
        }
    }
}
