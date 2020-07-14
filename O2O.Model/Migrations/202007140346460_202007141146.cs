namespace O2O.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _202007141146 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Ele_Shop", "AccountId", "dbo.Ele_Account");
            DropForeignKey("dbo.Ele_Account", "UserId", "dbo.T_User");
            DropForeignKey("dbo.Mt_Account", "UserId", "dbo.T_User");
            AddForeignKey("dbo.Ele_Shop", "AccountId", "dbo.Ele_Account", "Id");
            AddForeignKey("dbo.Ele_Account", "UserId", "dbo.T_User", "Id");
            AddForeignKey("dbo.Mt_Account", "UserId", "dbo.T_User", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Mt_Account", "UserId", "dbo.T_User");
            DropForeignKey("dbo.Ele_Account", "UserId", "dbo.T_User");
            DropForeignKey("dbo.Ele_Shop", "AccountId", "dbo.Ele_Account");
            AddForeignKey("dbo.Mt_Account", "UserId", "dbo.T_User", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Ele_Account", "UserId", "dbo.T_User", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Ele_Shop", "AccountId", "dbo.Ele_Account", "Id", cascadeDelete: true);
        }
    }
}
