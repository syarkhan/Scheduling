using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scheduling.Models.ViewModels
{
    public class SessionVM
    {

        public int PId { get ; set; }

        public string program { get; set; }

        public int ATID { get; set; }

        public int teacherid { get; set; }

        public string teachername { get; set; }

        public int studentid { get; set; }

        public string studentname { get; set; }

        public int secid { get; set; }

        public string Class { get; set; }

        public string sectionname { get; set; }



    }
}