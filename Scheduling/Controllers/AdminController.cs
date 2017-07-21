using Scheduling.Models;
using Scheduling.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scheduling.Controllers
{
    public class AdminController : Controller
    {
        DBEntities db = new DBEntities();




        // GET: Admin
        public ActionResult IndexOld()
        {

            CombineScheduleVM vm = new CombineScheduleVM();
            vm.days = db.days.ToList();
            vm.rooms = db.rooms.ToList();
            vm.slottypes = db.slottypes.Where(st => st.slottype1 == 1).ToList();
            vm.vslottypes = db.vslottypes.ToList();


            vm.hSchedule = vm.ListByCampus();

            return View(vm);
        }


        public ActionResult Index()
        {

            CombineScheduleVM vmm = new CombineScheduleVM();
            FilterScheduleVM vm = new FilterScheduleVM();
            vmm.ListByCampus();
            vm.day = db.days.ToList();
            vm.vslottype = db.vslottypes.ToList();


            return View(vm);
        }



        public ActionResult Filter(int slottypeid, int dayid, int campus)
        {
            //FilterScheduleVM vm = new FilterScheduleVM();
            //vm.day = db.days.ToList();
            //vm.vslottype = db.vslottypes.ToList();


            return View();
        }



        public ActionResult Sample()
        {
            CombineScheduleVM vm = new CombineScheduleVM();
            vm.days = db.days.ToList();
            vm.vslottypes = db.vslottypes.ToList();
            //vm.slottypes = db.slottypes.Where(st => st.slottype1 == 1).ToList();
            vm.rooms = db.rooms.Where(c => c.campus == "90").ToList();



            return View(vm);
        }


        //public ActionResult Schedule()
        //{
        //    CombineScheduleVM vm = new CombineScheduleVM();
        //    vm.days = db.days.ToList();
        //    vm.vslottypes = db.vslottypes.ToList();
        //    //vm.slottypes = db.slottypes.Where(st => st.slottype1 == 1).ToList();
        //    vm.rooms = db.rooms.Where(c => c.campus == "90").ToList();



        //    return View(vm);
        //}




        public ActionResult Schedule()
        {

            //if (Session["Username"] == null)
            //{
            //    return RedirectToAction("Index", "Login");

            //}
            //else
            //{

            CombineScheduleVM vm = new CombineScheduleVM();
            vm.days = db.days.ToList();
            vm.vslottypes = db.vslottypes.Where(o => o.occupied == 3).ToList();
            //vm.slottypes = db.slottypes.Where(st => st.slottype1 == 1).ToList();
            vm.rooms = db.rooms.Where(c => c.campus == "90").ToList();

            
            //vm.vschedule = 
            // db.vschedules.Where(a => a.dayid >= 1 && a.dayid <= 7 && a.campus == "90").ToList();
            //var current_date = DateTime.Now;
            //var current_week = current_date.AddDays(7);
            //DateTime myDate = DateTime.Parse(dateString);
            //string td = "9-19-2016";
            //string tq = "9-25-2016";

            //DateTime start = Convert.ToDateTime(td);
            //DateTime end = Convert.ToDateTime(tq);

            DateTime date1 = new DateTime(2016, 09, 05);
            //DateTime date2 = new DateTime(2016, 09, 19);
            DateTime date2 = DateTime.Now;
            //var weeks = ((date2 - date1).TotalDays) / 7;
            //int week = Convert.ToInt32(Math.Floor((date2 - date1).TotalDays / 7));
            int week = (int)((date2 - date1).TotalDays / 7) + 1;
            var weeks = Enumerable.Range(1, 18).ToList();
            ViewBag.weeks = new SelectList(weeks);
            ViewBag.week = week;

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

            DateTime n = start.AddDays(7);
            //ViewBag.currentdate = monday.Date.ToString("dd/MMMM");
            ViewBag.currdate = today.Date.ToString("dd/MMMM");
            ViewBag.currtime = today.Hour;
            //ViewBag.currday = today.Date.ToString("ddd");

            ViewBag.dates = dates;
            //select * from vschedule where campus =90 and StartDate>='9/5/2016' and StartDate<='9/11/2016';



            //   SELECT* from vschedule where

            //  ((StartDate BETWEEN '9/26/2016' and '10/2/2016') OR
            //  (EndDate BETWEEN '9/26/2016'and '10/2/2016') OR
            //   StartDate <= '9/26/2016' AND EndDate >= '10/2/2016')
            //and campus = 90

            vm.WeekWise = db.WeekWises.Where(w => w.Date >= start && w.Date <= end).ToList();

            vm.vschedule = (from a in db.vschedules
                            where ((a.startdate >= start && a.startdate <= end) ||
    (a.enddate >= start && a.enddate <= end) || (a.startdate <= start && a.enddate >= end)) && a.campus == "90"
                            orderby a.dayid, a.roomid, a.slottypeid, a.occupied
                            select a).ToList();






            //vm.vschedule = (from a in db.vschedules where (a.dayid >= 1 && a.dayid <= 7) && (a.campus == "90" && 
            //                a.startdate >= start) && (a.startdate <= end) || (a.startdate == start && 
            //                a.enddate != end) && (a.campus=="90") orderby a.dayid, a.roomid, a.slottypeid, a.occupied select a).ToList();

            //select* from vschedule where campus = 90 and StartDate>= '9/12/2016' and StartDate<= '9/17/2016' or StartDate = '9/5/2016' and EndDate!= '9/11/2016' and campus = 90;
            //vm.vschedule = (from a in db.vschedules where a.dayid >= 1 && a.dayid <= 7 && a.campus == "90" orderby a.dayid, a.roomid, a.slottypeid, a.occupied select a).ToList();

            //var sch = db.vschedules.Where(d => d.dayid == 1 && d.slottypeid == 4 && d.roomid == 6);

            //    if (sch.Count()>0)
            //        {

            //        }
            //    else
            //        {
            //    int a = 0;
            //}

            DateTime first = (FirstDateOfWeekISO8601(2016, 17)).AddDays(245);
            DateTime second = (FirstDateOfWeekISO8601(2016, 17)).AddDays(251);
            return View(vm);
            //}
        }

        public static DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }
            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }

























        public ActionResult ScheduleHashMap()
        {

            //if (Session["Username"] == null)
            //{
            //    return RedirectToAction("Index", "Login");

            //}
            //else
            //{

            CombineScheduleVM vm = new CombineScheduleVM();
            vm.days = db.days.ToList();
            vm.vslottypes = db.vslottypes.Where(o => o.occupied == 3).ToList();
            //vm.slottypes = db.slottypes.Where(st => st.slottype1 == 1).ToList();
            vm.rooms = db.rooms.Where(c => c.campus == "90").ToList();
            //vm.vschedule = 
            // db.vschedules.Where(a => a.dayid >= 1 && a.dayid <= 7 && a.campus == "90").ToList();
            //var current_date = DateTime.Now;
            //var current_week = current_date.AddDays(7);
            //DateTime myDate = DateTime.Parse(dateString);
            //string td = "9-19-2016";
            //string tq = "9-25-2016";

            //DateTime start = Convert.ToDateTime(td);
            //DateTime end = Convert.ToDateTime(tq);


            DateTime today = DateTime.Today;
            int currentDayOfWeek = (int)today.DayOfWeek;
            DateTime sunday = today.AddDays(-currentDayOfWeek);
            DateTime monday = sunday.AddDays(1);

            //DateTime mon = monday.Date.ToString("dd/MM/YYYY");
            if (currentDayOfWeek == 0)
            {
                monday = monday.AddDays(-7);
            }
            var dates = Enumerable.Range(0, 7).Select(days => monday.AddDays(days)).ToList();
            DateTime start = dates[0];
            DateTime end = dates[6];

            DateTime n = start.AddDays(7);
            //ViewBag.currentdate = monday.Date.ToString("dd/MMMM");
            ViewBag.currdate = today.Date.ToString("dd/MMMM");
            ViewBag.dates = dates;
            //select * from vschedule where campus =90 and StartDate>='9/5/2016' and StartDate<='9/11/2016';



            //   SELECT* from vschedule where

            //  ((StartDate BETWEEN '9/26/2016' and '10/2/2016') OR
            //  (EndDate BETWEEN '9/26/2016'and '10/2/2016') OR
            //   StartDate <= '9/26/2016' AND EndDate >= '10/2/2016')
            //and campus = 90

            vm.vschedule = (from a in db.vschedules
                            where ((a.startdate >= start && a.startdate <= end) ||
    (a.enddate >= start && a.enddate <= end) || (a.startdate <= start && a.enddate >= end)) && a.campus == "90"
                            orderby a.dayid, a.roomid, a.slottypeid, a.occupied
                            select a).ToList();


            Dictionary<string, List<vschedule>> hSchedule = new Dictionary<string, List<vschedule>>();

            string key, pkey = null;

            List<vschedule> alSch = (from a in db.vschedules
                                     where
             ((a.startdate >= start && a.startdate <= end) ||
             (a.enddate >= start && a.enddate <= end) || (a.startdate <= start && a.enddate >= end)) &&
             a.campus == "90"
                                     orderby a.dayid, a.roomid, a.slottypeid, a.occupied
                                     select a).ToList();

            List<vschedule> alSch1 = new List<vschedule>();
            foreach (var sch in alSch)
            {
                key = sch.dayid.ToString() + "-" + sch.roomid.ToString() + "-" + sch.slottypeid.ToString() + "-" + sch.occupied;
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


            vm.hSchedule = hSchedule;




            //vm.vschedule = (from a in db.vschedules where (a.dayid >= 1 && a.dayid <= 7) && (a.campus == "90" && 
            //                a.startdate >= start) && (a.startdate <= end) || (a.startdate == start && 
            //                a.enddate != end) && (a.campus=="90") orderby a.dayid, a.roomid, a.slottypeid, a.occupied select a).ToList();

            //select* from vschedule where campus = 90 and StartDate>= '9/12/2016' and StartDate<= '9/17/2016' or StartDate = '9/5/2016' and EndDate!= '9/11/2016' and campus = 90;
            //vm.vschedule = (from a in db.vschedules where a.dayid >= 1 && a.dayid <= 7 && a.campus == "90" orderby a.dayid, a.roomid, a.slottypeid, a.occupied select a).ToList();

            //var sch = db.vschedules.Where(d => d.dayid == 1 && d.slottypeid == 4 && d.roomid == 6);

            //    if (sch.Count()>0)
            //        {

            //        }
            //    else
            //        {
            //    int a = 0;
            //}


            return View(vm);
            //}
        }
    }
}