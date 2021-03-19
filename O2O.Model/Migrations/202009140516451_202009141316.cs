namespace O2O.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _202009141316 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.T_Order", "DispatcherName", c => c.String(maxLength: 40, unicode: false));
            AddColumn("dbo.T_Order", "DispatcherMobile", c => c.String(maxLength: 40, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.T_Order", "DispatcherMobile");
            DropColumn("dbo.T_Order", "DispatcherName");
        }
    }
}
