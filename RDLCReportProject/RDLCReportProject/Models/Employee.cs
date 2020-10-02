using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using RDLCReportProject.ViewModel;

namespace RDLCReportProject.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public  DateTime DateOfBirth { get; set; }
        public string FitherName { get; set; }
        public string MotherName { get; set; }
        public DateTime JoiningDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Religion { get; set; }
        public string WebAdderss { get; set; }
        public string BloodGroup { get; set; }
        public string Designation { get; set; }

    }

    public class ReportDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        //public DbSet<DynamicReportViewModel> DynamicReport { get; set; }
    }

}