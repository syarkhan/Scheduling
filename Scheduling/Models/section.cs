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
    
    public partial class section
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public section()
        {
            this.offeredcourses = new HashSet<offeredcourse>();
            this.Students = new HashSet<Student>();
        }
    
        public int secid { get; set; }
        public int PId { get; set; }
        public int ClassId { get; set; }
        public string sectionname { get; set; }
        public int STID { get; set; }
    
        public virtual Class Class { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<offeredcourse> offeredcourses { get; set; }
        public virtual Program Program { get; set; }
        public virtual SemesterType SemesterType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Student> Students { get; set; }
    }
}