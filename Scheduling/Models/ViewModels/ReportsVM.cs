using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scheduling.Models.ViewModels
{
    public class ReportsVM
    {
        public IEnumerable<EmptyRooms_Result> empty_rooms { get; set; }


        public IEnumerable<MonitoringByOffId_Result> monitoringbyoffid { get; set; }

        public IEnumerable<EmptyRoomsCountByDay_Result> emptyroomscount { get; set; }


        public IEnumerable<CurrentClasses_Result> currentclasses { get; set; }


        public IEnumerable<PMEmptyRooms_Result> PMEmptyRooms { get; set; }


    }
}