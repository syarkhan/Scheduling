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
    
    public partial class vslottype
    {
        public int slottypeid { get; set; }
        public string duration { get; set; }
        public Nullable<System.TimeSpan> ClassStartTime { get; set; }
        public Nullable<int> BreakDuration { get; set; }
        public Nullable<System.TimeSpan> ClassEndTime { get; set; }
        public Nullable<int> slotno { get; set; }
        public Nullable<decimal> sort { get; set; }
        public int subslotid { get; set; }
        public Nullable<int> occupied { get; set; }
    }
}