using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scheduling.Models.ViewModels
{
    public class WeeksVM
    {
        public int CurrentWeek { get; set; }
        public IEnumerable<SelectListItem> TotalWeeks { get; set; }
    }
}