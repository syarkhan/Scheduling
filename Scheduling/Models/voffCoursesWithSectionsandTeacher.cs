//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Scheduling.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class voffCoursesWithSectionsandTeacher
    {
        public int secid { get; set; }
        public int offid { get; set; }
        public int courseid { get; set; }
        public int teacherid { get; set; }
        public string program { get; set; }
        public string @class { get; set; }
        public string sectionname { get; set; }
        public string teachername { get; set; }
        public string Section { get; set; }
        public string title { get; set; }
        public Nullable<int> StudentCount { get; set; }
    }
}
