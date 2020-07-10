namespace O2O.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _07081045 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.T_Order", "DaySeq", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.T_Order", "DaySeq");
        }
    }
}
