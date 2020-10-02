namespace RDLCReportProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ColumnChange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Employees", "Religion", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Employees", "Religion", c => c.Int(nullable: false));
        }
    }
}
