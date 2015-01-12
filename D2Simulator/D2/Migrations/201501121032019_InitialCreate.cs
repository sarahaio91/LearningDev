namespace D2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Heroes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        currentHealth = c.Int(nullable: false),
                        maxHealth = c.Int(nullable: false),
                        currentLV = c.Int(nullable: false),
                        currentPosition_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Positions", t => t.currentPosition_ID)
                .Index(t => t.currentPosition_ID);
            
            CreateTable(
                "dbo.Positions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Heroes", "currentPosition_ID", "dbo.Positions");
            DropIndex("dbo.Heroes", new[] { "currentPosition_ID" });
            DropTable("dbo.Positions");
            DropTable("dbo.Heroes");
        }
    }
}
