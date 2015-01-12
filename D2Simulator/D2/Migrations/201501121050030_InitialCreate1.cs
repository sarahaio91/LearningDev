namespace D2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Heroes", "currentPosition_ID", "dbo.Positions");
            DropIndex("dbo.Heroes", new[] { "currentPosition_ID" });
            DropTable("dbo.Heroes");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.ID);
            
            CreateIndex("dbo.Heroes", "currentPosition_ID");
            AddForeignKey("dbo.Heroes", "currentPosition_ID", "dbo.Positions", "ID");
        }
    }
}
