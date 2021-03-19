namespace O2O.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _202009081713 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.T_Order", "RefundPartAmt", c => c.Double(nullable: false));
            AddColumn("dbo.T_OrderDtl", "RefundPartCnt", c => c.Double(nullable: false));
            AlterColumn("dbo.Mt_Account", "WaimaiAppId", c => c.String(maxLength: 20, unicode: false));
            AlterColumn("dbo.Mt_Account", "WaimaiAppSecret", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("dbo.Mt_Account", "TuangouAppKey", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("dbo.Mt_Account", "TuangouAppSecret", c => c.String(maxLength: 50, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Mt_Account", "TuangouAppSecret", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.Mt_Account", "TuangouAppKey", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.Mt_Account", "WaimaiAppSecret", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.Mt_Account", "WaimaiAppId", c => c.String(nullable: false, maxLength: 20, unicode: false));
            DropColumn("dbo.T_OrderDtl", "RefundPartCnt");
            DropColumn("dbo.T_Order", "RefundPartAmt");
        }
    }
}
