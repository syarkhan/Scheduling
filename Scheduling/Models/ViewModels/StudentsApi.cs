using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scheduling.Models.ViewModels
{
    public class StudentsApi
    {
        public string StudentName { get; set; }

        public string sectionname { get; set; }

        public string course { get; set; }

        public string teacher { get; set; }

        public string roomno { get; set; }


        public string campus { get; set; }


        public string duration { get; set; }

        public string dayname { get; set; }

        public DateTime dateOrder { get; set; }



    }
}