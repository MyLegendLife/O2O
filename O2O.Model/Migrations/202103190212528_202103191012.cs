namespace O2O.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _202103191012 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.T_Order", "Greeting", c => c.String(maxLength: 500, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.T_Order", "Greeting");
        }
    }
}
