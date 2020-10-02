using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Web.Mvc;
using RDLCReportProject.Models;
using RDLCReportProject.ViewModel;

namespace RDLCReportProject.Controllers
{
    public class ExcelReportController : Controller
    {
        //Canbe use EPPlus https://github.com/JanKallman/EPPlus
        ReportDbContext db = new ReportDbContext();
        // GET: ExcelReport
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
            
            List<DynamicReportViewModel> dynamicReportExcel = db.Database.SqlQuery<DynamicReportViewModel>("EmployeeReport '" + showColumns + "','" + gender + "','" + religion + "','" + designations + "','" + bloodGroup + "'").ToList();

            Microsoft.Office.Interop.Excel.Application xlApp;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            Microsoft.Office.Interop.Excel.Range chartRange;
            object misValue = System.Reflection.Missing.Value;
            xlApp = new Microsoft.Office.Interop.Excel.Application();

            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            try
            {
                xlWorkSheet.Name = "Summary";
                for (int i = 0; i < columns.Count; i++)
                {
                    xlWorkSheet.Cells[4, i + 1] = columns;
                }


                
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }

            byte[] fileContents = null;
            

            string filePath = Server.MapPath("~/Files/EmployeeReport.xlsx");
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            xlWorkBook.ActiveSheet.SaveCopyAs(filePath);
            //xlWorkBook.SaveAs("", Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);

            //MemoryStream memStream = new MemoryStream(filePath);

            //return File(memStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            return File(filePath, "multipart/form-data", "EmployeeReport.xlsx");
        }
    }
}