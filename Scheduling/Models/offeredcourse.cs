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
    
    public partial class offeredcourse
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public offeredcourse()
        {
            
            this.schedules = new HashSet<schedule>();
            this.sectiondetails = new HashSet<sectiondetail>();
            this.Requests = new HashSet<Request>();
        }
    
        public int offid { get; set; }
        public Nullable<int> courseid { get; set; }
        public Nullable<int> secid { get; set; }
        public Nullable<int> teacherid { get; set; }
        public Nullable<int> students_count { get; set; }
    
        public virtual course course { get; set; }
        public virtual section section { get; set; }
        public virtual teacher teacher { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<schedule> schedules { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sectiondetail> sectiondetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Request> Requests { get; set; }
    }
}