namespace O2O.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _202011180931 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.T_ShopConfigStock",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 50, unicode: false),
                        ShopNo = c.String(nullable: false, maxLength: 50, unicode: false),
                        ProdId = c.String(nullable: false, maxLength: 50, unicode: false),
                        ProdNo = c.String(nullable: false, maxLength: 50, unicode: false),
                        SafeStock = c.Double(nullable: false),
                        TakeType = c.Int(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.T_ShopConfig", "MtAutoUpDown", c => c.Int(nullable: false));
            AddColumn("dbo.T_ShopConfig", "EleAutoUpDown", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.T_ShopConfig", "EleAutoUpDown");
            DropColumn("dbo.T_ShopConfig", "MtAutoUpDown");
            DropTable("dbo.T_ShopConfigStock");
        }
    }
}
