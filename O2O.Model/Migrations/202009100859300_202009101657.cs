namespace O2O.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _202009101657 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.T_ShopConfig",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 20, unicode: false),
                        ShopNo = c.String(nullable: false, maxLength: 20, unicode: false),
                        MtAutoConfirm = c.Int(nullable: false),
                        EleAutoConfirm = c.Int(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.T_ShopConfig");
        }
    }
}
