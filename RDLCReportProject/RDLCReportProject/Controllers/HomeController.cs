using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using RDLCReportProject.Models;
//using RDLCReportProject.Reports;
using RDLCReportProject.ViewModel;

namespace RDLCReportProject.Controllers
{
    public class HomeController : Controller
    {
        ReportDbContext db = new ReportDbContext();
        public ActionResult Index()
        {
            var gender =
                new HashSet<string>(db.Employees.GroupBy(x => x.Gender).Select(x => x.FirstOrDefault()).Select(x => x.Gender))
                    .ToList();
            var bloodGroup =
                new HashSet<string>(db.Employees.GroupBy(x => x.BloodGroup).Select(x => x.FirstOrDefault())
                    .Select(x => x.BloodGroup)).ToList();
            var desig = new HashSet<string>(db.Employees.GroupBy(x => x.Designation).Select(x => x.FirstOrDefault())
                .Select(x => x.Designation)).ToList();
            var rel = new HashSet<string>(db.Employees.GroupBy(x => x.Religion).Select(x => x.FirstOrDefault())
                .Select(x => x.Religion)).ToList();

            var columnNames = db.Database.SqlQuery<string>("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Employees' AND COLUMN_NAME <> 'Id'").ToList();

            FilterViewModel model = new FilterViewModel()
            {
                Genders = gender,
                BloodGroups = bloodGroup,
                Designations = desig,
                Religion = rel,
                ColumnNames = columnNames
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Report(List<string> columns, string gender, string religion, string designations, string bloodGroup)
        {

            string showColumns = "";
            if (columns != null && columns.Count > 0)
            {
                for (int i = 0; i < columns.Count; i++)
                {
                    if (i + 1 == columns.Count)
                        showColumns += columns[i];
                    else
                        showColumns += columns[i] + ",";
                }
            }
            


            LocalReport report = new LocalReport();
            report.ReportPath = Server.MapPath("~/Reports/DynamicReport.rdlc");
            var dsds = db.Employees.ToList();
            var ds = db.Database
                .SqlQuery<DynamicReportViewModel>("EmployeeReport '" + showColumns + "','" + gender + "','" + religion +
                                                  "','" + designations + "','" + bloodGroup + "'").ToList();
            report.DataSources.Add(new ReportDataSource("DataSet1", ds));
            

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string filenameExtension;

            byte[] bytes = report.Render("PDF", null, out mimeType, out encoding, out filenameExtension,out streamids, out warnings);
            MemoryStream stream = new MemoryStream(bytes);
            return File(stream, "application/pdf");
        }

        public ActionResult PDF(string column, string gender, string religion, string designations, string bloodGroup)
        {

            string showColumns = "";

            List<string> columns = column.Split(',').ToList();

            if (columns != null && columns.Count > 0)
            {
                for (int i = 0; i < columns.Count; i++)
                {
                    if (i + 1 == columns.Count)
                        showColumns += columns[i];
                    else
                        showColumns += columns[i] + ",";
                }
            }



            LocalReport report = new LocalReport();
            report.ReportPath = Server.MapPath("~/Reports/DynamicReport.rdlc");
            var dsds = db.Employees.ToList();
            var ds = db.Database
                .SqlQuery<DynamicReportViewModel>("EmployeeReport '" + showColumns + "','" + gender + "','" + religion +
                                                  "','" + designations + "','" + bloodGroup + "'").ToList();
            report.DataSources.Add(new ReportDataSource("DataSet1", ds));


            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string filenameExtension;

            byte[] bytes = report.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
            Session["pdfMemoryStream"] = new MemoryStream(bytes);

            return PartialView("_Viewpdf");
        }
        

        public ActionResult GeneratePdf()
        {
            MemoryStream stream =  (MemoryStream)Session["pdfMemoryStream"];
            return File(stream, "application/pdf");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}