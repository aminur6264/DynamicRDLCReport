using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RDLCReportProject.ViewModel
{
    public class FilterViewModel
    {
        public List<string> Genders { get; set; }
        public List<string> Designations { get; set; }
        public List<string> BloodGroups { get; set; }
        public List<string> Religion { get; set; }
        public List<string> ColumnNames { get; set; }
    }
}
