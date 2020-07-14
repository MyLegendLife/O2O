namespace O2O.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _202007131551 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.T_User", "SetBuyPara", c => c.String(maxLength: 100, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.T_User", "SetBuyPara");
        }
    }
}
