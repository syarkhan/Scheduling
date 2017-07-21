using Scheduling.Models;
using Scheduling.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scheduling.Controllers
{
    public class PMController : Controller
    {
        DBEntities db = new DBEntities();

        // GET: PM
        public ActionResult ISchedule()
        {

            CombineScheduleVM vm = new CombineScheduleVM();
            vm.days = db.days.ToList();
            vm.vslottypes = db.vslottypes.Where(o => o.occupied == 3).ToList();

            vm.sections = db.sections.Where(a => a.sectionname.Contains("BCS/BS") && a.STID == 1).ToList();
            //vm.slottypes = db.slottypes.Where(st => st.slottype1 == 1).ToList();
            //vm.rooms = db.rooms.ToList();

            var sdate = db.Semesters.Where(s => s.flag == true).Select(s => s.SemesterStartDate).Single();

            DateTime date1 = Convert.ToDateTime(sdate);

            //DateTime date1 = new DateTime(2016, 09, 05);
            //DateTime date2 = new DateTime(2016, 09, 19);
            DateTime date2 = DateTime.Now;
            //var weeks = ((date2 - date1).TotalDays) / 7;
            //int week = Convert.ToInt32(Math.Floor((date2 - date1).TotalDays / 7));
            int week = (int)((date2 - date1).TotalDays / 7) + 1;
            var weeks = Enumerable.Range(1, 18).ToList();
            ViewBag.weeks = new SelectList(weeks);
            ViewBag.week = week;

            List<int> durations = new List<int>(new int[] { 1, 2, 3 });
            ViewBag.weeks = new SelectList(weeks);


            DateTime today = DateTime.Now;
            int currentDayOfWeek = (int)today.DayOfWeek;
            DateTime sunday = today.AddDays(-currentDayOfWeek);
            DateTime monday = sunday.AddDays(1);

            //DateTime mon = monday.Date.ToString("dd/MM/YYYY");
            if (currentDayOfWeek == 0)
            {
                monday = monday.AddDays(-7);
            }
            var dates = Enumerable.Range(0, 7).Select(days => monday.AddDays(days)).ToList();
            DateTime start = dates[0].Date;
            DateTime end = dates[6].Date;



            //select* from vschedule where secid = 1 and slottypeid = 1 and dayid = 1



            vm.vschedule = (from a in db.vschedules
                            where ((a.startdate >= start && a.startdate <= end) ||
                            (a.enddate >= start && a.enddate <= end) || (a.startdate <= start && a.enddate >= end))
                            && a.flag == true
                            && a.sectionname.Contains("BCS/BS")
                            orderby a.dayid, a.slottypeid, a.secid,a.occupied
                            select a).ToList();

            return View(vm);
        }
    }
}