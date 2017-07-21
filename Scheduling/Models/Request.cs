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
    
    public partial class Request
    {
        public int Id { get; set; }
        public int offid { get; set; }
        public int teacherid { get; set; }
        public Nullable<int> slotid { get; set; }
        public int CTID { get; set; }
        public System.DateTime Date { get; set; }
        public string Reason { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<int> occupied { get; set; }
        public Nullable<System.DateTime> last_updated { get; set; }
    
        public virtual ClassType ClassType { get; set; }
        public virtual offeredcourse offeredcourse { get; set; }
        public virtual teacher teacher { get; set; }
        public virtual slot slot { get; set; }
    }
}