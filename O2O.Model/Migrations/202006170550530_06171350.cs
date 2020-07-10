namespace O2O.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _06171350 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.T_OrderDtl", "OrderId", "dbo.T_Order");
            DropPrimaryKey("dbo.T_Order");
            AlterColumn("dbo.T_Order", "Id", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey("dbo.T_Order", "Id");
            AddForeignKey("dbo.T_OrderDtl", "OrderId", "dbo.T_Order", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.T_OrderDtl", "OrderId", "dbo.T_Order");
            DropPrimaryKey("dbo.T_Order");
            AlterColumn("dbo.T_Order", "Id", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.T_Order", "Id");
            AddForeignKey("dbo.T_OrderDtl", "OrderId", "dbo.T_Order", "Id", cascadeDelete: true);
        }
    }
}
