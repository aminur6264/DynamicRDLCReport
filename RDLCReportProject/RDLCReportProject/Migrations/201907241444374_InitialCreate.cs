namespace RDLCReportProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Gender = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        FitherName = c.String(),
                        MotherName = c.String(),
                        JoiningDate = c.DateTime(nullable: false),
                        PhoneNumber = c.String(),
                        Email = c.String(),
                        Religion = c.Int(nullable: false),
                        WebAdderss = c.String(),
                        BloodGroup = c.String(),
                        Designation = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Employees");
        }
    }
}
