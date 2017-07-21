using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scheduling.Models.ViewModels
{
    public class FilterScheduleVM
    {
        DBEntities db = new DBEntities();

        public IEnumerable<day> day { get; set; }
        public IEnumerable<vslottype> vslottype { get; set; }



        public Dictionary<string, List<vschedule>> ListByFilter()
        {

            Dictionary<string, List<vschedule>> hSchedule = new Dictionary<string, List<vschedule>>();

            string key, pkey = null;

            List<vschedule> alSch = (from a in db.vschedules orderby a.dayid, a.roomid, a.slotno, a.occupied select a).ToList();

            List<vschedule> alSch1 = new List<vschedule>();
            foreach (var sch in alSch)
            {
                key = sch.dayid + "-" + sch.roomid + "-" + sch.slotno;
                if (key.Equals(pkey))
                {
                    alSch1.Add(sch);
                }
                else
                {
                    alSch1 = new List<vschedule>();
                    alSch1.Add(sch);
                }
                hSchedule[key] = alSch1;

                pkey = key;
            }
            return hSchedule;
        }

    }
}