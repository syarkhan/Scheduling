using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scheduling.Models.ViewModels
{
    public class ExcelVM
    {
        
        public HttpPostedFileBase MyExcelFile { get; set; }

        public string MSExcelTable { get; set; }
    }
}