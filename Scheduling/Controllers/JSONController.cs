using Newtonsoft.Json;
using Scheduling.Models;
using Scheduling.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Scheduling.Controllers
{
    public class JSONController : Controller
    {
        DBEntities db = new DBEntities();

        // GET: JSON
        //public ActionResult FilterBySlotsAndCampus(int durid, int dayid, string campus)
        //{


        //    Dictionary<string, List<vschedule>> hSchedule = new Dictionary<string, List<vschedule>>();

        //    string key, pkey = null;

        //    List<vschedule> alSch = (from a in db.vschedules where a.dayid == dayid && a.slottypeid == durid && a.campus == campus orderby a.roomid, a.occupied select a).ToList();

        //    List<vschedule> alSch1 = new List<vschedule>();
        //    foreach (var sch in alSch)
        //    {
        //        key = sch.roomid.ToString();
        //        if (key.Equals(pkey))
        //        {
        //            alSch1.Add(sch);
        //        }
        //        else
        //        {
        //            alSch1 = new List<vschedule>();
        //            alSch1.Add(sch);
        //        }
        //        hSchedule[key] = alSch1;

        //        pkey = key;
        //    }
        //    //var result = JsonConvert.SerializeObject(hSchedule.ToArray(),Formatting.Indented);
        //    return Json(hSchedule.ToArray(), JsonRequestBehavior.AllowGet);



        //}



        public PartialViewResult FilterBySlotsAndCampus(int durid, int dayid, string campus,string slottime)
        {
            CombineScheduleVM vm = new CombineScheduleVM();
            ViewBag.durid = durid;
            ViewBag.dayid = dayid;
            ViewBag.campus = campus;
            ViewBag.slottime = slottime;
            //Dictionary<string, List<vschedule>> hSchedule = new Dictionary<string, List<vschedule>>();

            //string key, pkey = null;

            //List<vschedule> alSch = (from a in db.vschedules where a.dayid == dayid && a.slottypeid == durid && a.campus == campus orderby a.roomid, a.occupied select a).ToList();

            //List<vschedule> alSch1 = new List<vschedule>();
            //foreach (var sch in alSch)
            //{
            //    key = sch.roomid.ToString();
            //    if (key.Equals(pkey))
            //    {
            //        alSch1.Add(sch);
            //    }
            //    else
            //    {
            //        alSch1 = new List<vschedule>();
            //        alSch1.Add(sch);
            //    }
            //    hSchedule[key] = alSch1;

            //    pkey = key;
            //}

            //vm.hSchedule = hSchedule;
            vm.rooms = db.rooms.Where(c => c.campus == campus).ToList().OrderBy(c=>c.OrderCol);
            //var result = JsonConvert.SerializeObject(hSchedule.ToArray(),Formatting.Indented);
            return PartialView("Schedule_Partial", vm);
            //return Json(hSchedule.ToArray(), JsonRequestBehavior.AllowGet);



        }



        //Academic Officer
        [HttpGet]
        public PartialViewResult FilterByCampusAndDate_AO(string campus,string start,string end)
        {
            CombineScheduleVM vm = new CombineScheduleVM();
            //ViewBag.durid = durid;
            //ViewBag.dayid = dayid;
            //ViewBag.campus = campus;

            string sd = start.Replace('/', '-');
            string ed = end.Replace('/', '-');

            //DateTime startdate = Convert.ToDateTime(sd);
            //DateTime enddate = Convert.ToDateTime(ed);

            DateTime startdate = DateTime.ParseExact(start, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            DateTime enddate = DateTime.ParseExact(end, "MM/dd/yyyy", CultureInfo.InvariantCulture);


            DateTime today = DateTime.Today;
            int currentDayOfWeek = (int)startdate.DayOfWeek;
            DateTime sunday = startdate.AddDays(-currentDayOfWeek);
            DateTime monday = sunday.AddDays(1);

            if (currentDayOfWeek == 0)
            {
                monday = monday.AddDays(-7);
            }
            var dates = Enumerable.Range(0, 7).Select(days => monday.AddDays(days)).ToList();


            ViewBag.currdate = today.Date.ToString("dd/MMMM");
            ViewBag.dates = dates;

            vm.days = db.days.ToList();
            vm.vslottypes = db.vslottypes.Where(o => o.occupied == 3).ToList();

            vm.vschedule = (from a in db.vschedules
                            where ((a.startdate >= startdate && a.startdate <= enddate) ||
                            (a.enddate >= startdate && a.enddate <= enddate) || (a.startdate <= startdate && a.enddate >= enddate)) && a.campus == campus
                            orderby a.dayid, a.roomid, a.slottypeid, a.occupied
                            select a).ToList();
            vm.rooms = db.rooms.Where(c => c.campus == campus).ToList().OrderBy(c=>c.OrderCol);
            vm.WeekWise = db.WeekWises.Where(w => w.Date >= startdate && w.Date <= enddate).ToList();
            //var result = JsonConvert.SerializeObject(hSchedule.ToArray(),Formatting.Indented);
            return PartialView("AO_Sch_Partial", vm);
            //return Json(hSchedule.ToArray(), JsonRequestBehavior.AllowGet);



        }

        [HttpGet]
        public PartialViewResult FilterByWeekLeft_AO(string campus, string start,int weekno)
        {
            
                CombineScheduleVM vm = new CombineScheduleVM();
                //ViewBag.durid = durid;
                //ViewBag.dayid = dayid;
                //ViewBag.campus = campus;
                var weeks = Enumerable.Range(1, 18).ToList();
                ViewBag.weeks = new SelectList(weeks);
                ViewBag.week = weekno;
                string sd = start.Replace('/', '-');
                //string ed = end.Replace('/', '-');

                //DateTime startdate = Convert.ToDateTime(sd);

                DateTime startdate = DateTime.ParseExact(start, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                //DateTime enddate = Convert.ToDateTime(ed);


            DateTime enddate = startdate.AddDays(-1);


                DateTime today = DateTime.Today;
                //int currentDay = (int)today.DayOfWeek;
                int currentDayOfWeek = (int)enddate.DayOfWeek;
                DateTime sunday = enddate.AddDays(-currentDayOfWeek);
                DateTime monday = sunday.AddDays(1);

                if (currentDayOfWeek == 0)
                {
                    monday = monday.AddDays(-7);
                }
                var dates = Enumerable.Range(0, 7).Select(days => monday.AddDays(days)).ToList();

                DateTime startd = dates[0];
                DateTime end = dates[6];


                //DateTime s = new DateTime(2016, 11, 21);
                //DateTime e = new DateTime(2016, 11, 27);

                ViewBag.currdate = today.Date.ToString("dd/MMMM");
                ViewBag.dates = dates;

                vm.days = db.days.ToList();
                vm.vslottypes = db.vslottypes.Where(o => o.occupied == 3).ToList();

                vm.vschedule = (from a in db.vschedules
                                where ((a.startdate >= startd && a.startdate <= end) ||
                                (a.enddate >= startd && a.enddate <= end) || (a.startdate <= startd && a.enddate >= end)) && a.campus == campus
                                orderby a.dayid, a.roomid, a.slottypeid, a.occupied
                                select a).ToList();
                vm.rooms = db.rooms.Where(c => c.campus == campus).ToList().OrderBy(c=>c.OrderCol);
                vm.WeekWise = db.WeekWises.Where(w=>w.Date >= startd && w.Date <= end).ToList();
            //var result = JsonConvert.SerializeObject(hSchedule.ToArray(),Formatting.Indented);
            if (weekno == 8)
            {
                return PartialView("MidWeek",vm);
            }
            else
            {
                return PartialView("AO_Sch_Partial", vm);
            }
            //return Json(hSchedule.ToArray(), JsonRequestBehavior.AllowGet);



        }

        [HttpGet]
        public PartialViewResult FilterByWeekDropDown_AO(string campus,int weekno)
        {

            var sdate = db.Semesters.Where(s => s.flag == true).Select(s => s.SemesterStartDate).Single();

            var startOfYear = new DateTime(sdate.Year, 1, 1);

            var sem_start = (sdate - startOfYear).TotalDays - 1;

            


            CombineScheduleVM vm = new CombineScheduleVM();
            DateTime first = (FirstDateOfWeekISO8601(2017, weekno)).AddDays(sem_start);
            DateTime second = (FirstDateOfWeekISO8601(2017, weekno)).AddDays((sem_start + 6));
            var weeks = Enumerable.Range(1, 18).ToList();
            ViewBag.weeks = new SelectList(weeks);
            ViewBag.week = weekno;
           


            DateTime today = first;
            //int currentDay = (int)today.DayOfWeek;
            int currentDayOfWeek = (int)second.DayOfWeek;
            DateTime sunday = second.AddDays(-currentDayOfWeek);
            DateTime monday = sunday.AddDays(1);

            if (currentDayOfWeek == 0)
            {
                monday = monday.AddDays(-7);
            }
            var dates = Enumerable.Range(0, 7).Select(days => monday.AddDays(days)).ToList();

            DateTime startd = dates[0];
            DateTime end = dates[6];


            ViewBag.currdate = DateTime.Now.Date.ToString("dd/MMMM");
            ViewBag.dates = dates;

            vm.days = db.days.ToList();
            vm.vslottypes = db.vslottypes.Where(o => o.occupied == 3).ToList();

            vm.WeekWise = db.WeekWises.Where(w => w.Date >= startd && w.Date <= end).ToList();

            vm.vschedule = (from a in db.vschedules
                            where ((a.startdate >= startd && a.startdate <= end) ||
                            (a.enddate >= startd && a.enddate <= end) || (a.startdate <= startd && a.enddate >= end)) && a.campus == campus
                            orderby a.dayid, a.roomid, a.slottypeid, a.occupied
                            select a).ToList();
            vm.rooms = db.rooms.Where(c => c.campus == campus).ToList().OrderBy(c=>c.OrderCol);
            //var result = JsonConvert.SerializeObject(hSchedule.ToArray(),Formatting.Indented);
            if (weekno == 8)
            {
                return PartialView("MidWeek", vm);
            }
            else
            {
                return PartialView("AO_Sch_Partial", vm);
            }
            
        }
        
        [HttpGet]
        public PartialViewResult FilterByWeekRight_AO(string campus, string end1)
        {
            CombineScheduleVM vm = new CombineScheduleVM();
            //ViewBag.durid = durid;
            //ViewBag.dayid = dayid;
            //ViewBag.campus = campus;

            //string sd = start.Replace('/', '-');
            string ed = end1.Replace('/', '-');

            //DateTime startdate = Convert.ToDateTime(sd);
            //DateTime enddate = Convert.ToDateTime(ed);

            DateTime enddate = DateTime.ParseExact(end1, "MM/dd/yyyy", CultureInfo.InvariantCulture);


            DateTime startdate = enddate.AddDays(+1);


            DateTime today = DateTime.Today;
            //int currentDay = (int)today.DayOfWeek;
            int currentDayOfWeek = (int)enddate.DayOfWeek;
            DateTime sunday = enddate.AddDays(+currentDayOfWeek);
            DateTime monday = sunday.AddDays(1);

            if (currentDayOfWeek == 0)
            {
                monday = monday.AddDays(+0);
            }
            var dates = Enumerable.Range(0, 7).Select(days => monday.AddDays(days)).ToList();

            DateTime startd = dates[0];
            DateTime end = dates[6];


            ViewBag.currdate = today.Date.ToString("dd/MMMM");
            ViewBag.dates = dates;


            vm.WeekWise = db.WeekWises.Where(w => w.Date >= startd && w.Date <= end).ToList();
            vm.days = db.days.ToList();
            vm.vslottypes = db.vslottypes.Where(o => o.occupied == 3).ToList();

            vm.vschedule = (from a in db.vschedules
                            where ((a.startdate >= startd && a.startdate <= end) ||
                            (a.enddate >= startd && a.enddate <= end) || (a.startdate <= startd && a.enddate >= end)) && a.campus == campus
                            orderby a.dayid, a.roomid, a.slottypeid, a.occupied
                            select a).ToList();
            vm.rooms = db.rooms.Where(c => c.campus == campus).ToList().OrderBy(c=>c.OrderCol);
            //var result = JsonConvert.SerializeObject(hSchedule.ToArray(),Formatting.Indented);
            return PartialView("AO_Sch_Partial", vm);
            //return Json(hSchedule.ToArray(), JsonRequestBehavior.AllowGet);
        }




        //Academic Assistant
        [HttpGet]
        public PartialViewResult FilterByCampusAndDate_AA(string campus, string start, string end)
        {
            CombineScheduleVM vm = new CombineScheduleVM();
            //ViewBag.durid = durid;
            //ViewBag.dayid = dayid;
            //ViewBag.campus = campus;

            string sd = start.Replace('/', '-');
            string ed = end.Replace('/', '-');

            //DateTime startdate = Convert.ToDateTime(sd);
            //DateTime enddate = Convert.ToDateTime(ed);

            DateTime startdate = DateTime.ParseExact(start, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            DateTime enddate = DateTime.ParseExact(end, "MM/dd/yyyy", CultureInfo.InvariantCulture);


            DateTime today = DateTime.Today;
            int currentDayOfWeek = (int)startdate.DayOfWeek;
            DateTime sunday = startdate.AddDays(-currentDayOfWeek);
            DateTime monday = sunday.AddDays(1);

            if (currentDayOfWeek == 0)
            {
                monday = monday.AddDays(-7);
            }
            var dates = Enumerable.Range(0, 7).Select(days => monday.AddDays(days)).ToList();


            ViewBag.currdate = today.Date.ToString("dd/MMMM");
            ViewBag.dates = dates;

            vm.days = db.days.ToList();
            vm.vslottypes = db.vslottypes.Where(o => o.occupied == 3).ToList();

            vm.vschedule = (from a in db.vschedules
                            where ((a.startdate >= startdate && a.startdate <= enddate) ||
                            (a.enddate >= startdate && a.enddate <= enddate) || (a.startdate <= startdate && a.enddate >= enddate)) && a.campus == campus
                            orderby a.dayid, a.roomid, a.slottypeid, a.occupied
                            select a).ToList();
            vm.rooms = db.rooms.Where(c => c.campus == campus).ToList().OrderBy(c => c.OrderCol);
            vm.WeekWise = db.WeekWises.Where(w => w.Date >= startdate && w.Date <= enddate).ToList();
            //var result = JsonConvert.SerializeObject(hSchedule.ToArray(),Formatting.Indented);
            return PartialView("AA_Sch_Partial", vm);
            //return Json(hSchedule.ToArray(), JsonRequestBehavior.AllowGet);



        }

        [HttpGet]
        public PartialViewResult FilterByWeekLeft_AA(string campus, string start, int weekno)
        {

            CombineScheduleVM vm = new CombineScheduleVM();
            //ViewBag.durid = durid;
            //ViewBag.dayid = dayid;
            //ViewBag.campus = campus;
            var weeks = Enumerable.Range(1, 18).ToList();
            ViewBag.weeks = new SelectList(weeks);
            ViewBag.week = weekno;
            string sd = start.Replace('/', '-');
            //string ed = end.Replace('/', '-');

            //DateTime startdate = Convert.ToDateTime(sd);
            DateTime startdate = DateTime.ParseExact(start, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            //DateTime enddate = Convert.ToDateTime(ed);


            DateTime enddate = startdate.AddDays(-1);


            DateTime today = DateTime.Today;
            //int currentDay = (int)today.DayOfWeek;
            int currentDayOfWeek = (int)enddate.DayOfWeek;
            DateTime sunday = enddate.AddDays(-currentDayOfWeek);
            DateTime monday = sunday.AddDays(1);

            if (currentDayOfWeek == 0)
            {
                monday = monday.AddDays(-7);
            }
            var dates = Enumerable.Range(0, 7).Select(days => monday.AddDays(days)).ToList();

            DateTime startd = dates[0];
            DateTime end = dates[6];


            //DateTime s = new DateTime(2016, 11, 21);
            //DateTime e = new DateTime(2016, 11, 27);

            ViewBag.currdate = today.Date.ToString("dd/MMMM");
            ViewBag.dates = dates;

            vm.days = db.days.ToList();
            vm.vslottypes = db.vslottypes.Where(o => o.occupied == 3).ToList();

            vm.vschedule = (from a in db.vschedules
                            where ((a.startdate >= startd && a.startdate <= end) ||
                            (a.enddate >= startd && a.enddate <= end) || (a.startdate <= startd && a.enddate >= end)) && a.campus == campus
                            orderby a.dayid, a.roomid, a.slottypeid, a.occupied
                            select a).ToList();
            vm.rooms = db.rooms.Where(c => c.campus == campus).ToList().OrderBy(c => c.OrderCol);
            vm.WeekWise = db.WeekWises.Where(w => w.Date >= startd && w.Date <= end).ToList();
            //var result = JsonConvert.SerializeObject(hSchedule.ToArray(),Formatting.Indented);
            if (weekno == 8)
            {
                return PartialView("MidWeek", vm);
            }
            else
            {
                return PartialView("AA_Sch_Partial", vm);
            }
            //return Json(hSchedule.ToArray(), JsonRequestBehavior.AllowGet);



        }

        [HttpGet]
        public PartialViewResult FilterByWeekDropDown_AA(string campus, int weekno)
        {


            CombineScheduleVM vm = new CombineScheduleVM();
            DateTime first = (FirstDateOfWeekISO8601(2016, weekno)).AddDays(245);
            DateTime second = (FirstDateOfWeekISO8601(2016, weekno)).AddDays(251);
            var weeks = Enumerable.Range(1, 18).ToList();
            ViewBag.weeks = new SelectList(weeks);
            ViewBag.week = weekno;



            DateTime today = first;
            //int currentDay = (int)today.DayOfWeek;
            int currentDayOfWeek = (int)second.DayOfWeek;
            DateTime sunday = second.AddDays(-currentDayOfWeek);
            DateTime monday = sunday.AddDays(1);

            if (currentDayOfWeek == 0)
            {
                monday = monday.AddDays(-7);
            }
            var dates = Enumerable.Range(0, 7).Select(days => monday.AddDays(days)).ToList();

            DateTime startd = dates[0];
            DateTime end = dates[6];


            ViewBag.currdate = DateTime.Now.Date.ToString("dd/MMMM");
            ViewBag.dates = dates;

            vm.days = db.days.ToList();
            vm.vslottypes = db.vslottypes.Where(o => o.occupied == 3).ToList();

            vm.WeekWise = db.WeekWises.Where(w => w.Date >= startd && w.Date <= end).ToList();

            vm.vschedule = (from a in db.vschedules
                            where ((a.startdate >= startd && a.startdate <= end) ||
                            (a.enddate >= startd && a.enddate <= end) || (a.startdate <= startd && a.enddate >= end)) && a.campus == campus
                            orderby a.dayid, a.roomid, a.slottypeid, a.occupied
                            select a).ToList();
            vm.rooms = db.rooms.Where(c => c.campus == campus).ToList().OrderBy(c => c.OrderCol);
            //var result = JsonConvert.SerializeObject(hSchedule.ToArray(),Formatting.Indented);
            if (weekno == 8)
            {
                return PartialView("MidWeek", vm);
            }
            else
            {
                return PartialView("AA_Sch_Partial", vm);
            }

        }

        [HttpGet]
        public PartialViewResult FilterByWeekRight_AA(string campus, string end1)
        {
            CombineScheduleVM vm = new CombineScheduleVM();
            //ViewBag.durid = durid;
            //ViewBag.dayid = dayid;
            //ViewBag.campus = campus;

            //string sd = start.Replace('/', '-');
            string ed = end1.Replace('/', '-');

            //DateTime startdate = Convert.ToDateTime(sd);
            //DateTime enddate = Convert.ToDateTime(ed);
            DateTime enddate = DateTime.ParseExact(end1, "MM/dd/yyyy", CultureInfo.InvariantCulture);


            DateTime startdate = enddate.AddDays(+1);


            DateTime today = DateTime.Today;
            //int currentDay = (int)today.DayOfWeek;
            int currentDayOfWeek = (int)enddate.DayOfWeek;
            DateTime sunday = enddate.AddDays(+currentDayOfWeek);
            DateTime monday = sunday.AddDays(1);

            if (currentDayOfWeek == 0)
            {
                monday = monday.AddDays(+0);
            }
            var dates = Enumerable.Range(0, 7).Select(days => monday.AddDays(days)).ToList();

            DateTime startd = dates[0];
            DateTime end = dates[6];


            ViewBag.currdate = today.Date.ToString("dd/MMMM");
            ViewBag.dates = dates;


            vm.WeekWise = db.WeekWises.Where(w => w.Date >= startd && w.Date <= end).ToList();
            vm.days = db.days.ToList();
            vm.vslottypes = db.vslottypes.Where(o => o.occupied == 3).ToList();

            vm.vschedule = (from a in db.vschedules
                            where ((a.startdate >= startd && a.startdate <= end) ||
                            (a.enddate >= startd && a.enddate <= end) || (a.startdate <= startd && a.enddate >= end)) && a.campus == campus
                            orderby a.dayid, a.roomid, a.slottypeid, a.occupied
                            select a).ToList();
            vm.rooms = db.rooms.Where(c => c.campus == campus).ToList().OrderBy(c => c.OrderCol);
            //var result = JsonConvert.SerializeObject(hSchedule.ToArray(),Formatting.Indented);
            return PartialView("AA_Sch_Partial", vm);
            //return Json(hSchedule.ToArray(), JsonRequestBehavior.AllowGet);
        }



        //AAC
        [HttpGet]
        public PartialViewResult FilterByCampusAndDate_AAC(string campus, string start, string end)
        {
            CombineScheduleVM vm = new CombineScheduleVM();
            //ViewBag.durid = durid;
            //ViewBag.dayid = dayid;
            //ViewBag.campus = campus;

            string sd = start.Replace('/', '-');
            string ed = end.Replace('/', '-');

            //DateTime startdate = Convert.ToDateTime(sd);
            //DateTime enddate = Convert.ToDateTime(ed);

            DateTime startdate = DateTime.ParseExact(start, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            DateTime enddate = DateTime.ParseExact(end, "MM/dd/yyyy", CultureInfo.InvariantCulture);


            DateTime today = DateTime.Today;
            int currentDayOfWeek = (int)startdate.DayOfWeek;
            DateTime sunday = startdate.AddDays(-currentDayOfWeek);
            DateTime monday = sunday.AddDays(1);

            if (currentDayOfWeek == 0)
            {
                monday = monday.AddDays(-7);
            }
            var dates = Enumerable.Range(0, 7).Select(days => monday.AddDays(days)).ToList();


            ViewBag.currdate = today.Date.ToString("dd/MMMM");
            ViewBag.dates = dates;

            vm.days = db.days.ToList();
            vm.vslottypes = db.vslottypes.Where(o => o.occupied == 3).ToList();

            vm.vschedule = (from a in db.vschedules
                            where ((a.startdate >= startdate && a.startdate <= enddate) ||
                            (a.enddate >= startdate && a.enddate <= enddate) || (a.startdate <= startdate && a.enddate >= enddate)) && a.campus == campus
                            orderby a.dayid, a.roomid, a.slottypeid, a.occupied
                            select a).ToList();
            vm.rooms = db.rooms.Where(c => c.campus == campus).ToList().OrderBy(c => c.OrderCol);
            vm.WeekWise = db.WeekWises.Where(w => w.Date >= startdate && w.Date <= enddate).ToList();
            //var result = JsonConvert.SerializeObject(hSchedule.ToArray(),Formatting.Indented);
            return PartialView("AAC_Sch_Partial", vm);
            //return Json(hSchedule.ToArray(), JsonRequestBehavior.AllowGet);



        }

        [HttpGet]
        public PartialViewResult FilterByWeekLeft_AAC(string campus, string start, int weekno)
        {

            CombineScheduleVM vm = new CombineScheduleVM();
            //ViewBag.durid = durid;
            //ViewBag.dayid = dayid;
            //ViewBag.campus = campus;
            var weeks = Enumerable.Range(1, 18).ToList();
            ViewBag.weeks = new SelectList(weeks);
            ViewBag.week = weekno;
            string sd = start.Replace('/', '-');
            //string ed = end.Replace('/', '-');

            //DateTime startdate = Convert.ToDateTime(sd);
            DateTime startdate = DateTime.ParseExact(start, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            //DateTime enddate = Convert.ToDateTime(ed);


            DateTime enddate = startdate.AddDays(-1);


            DateTime today = DateTime.Today;
            //int currentDay = (int)today.DayOfWeek;
            int currentDayOfWeek = (int)enddate.DayOfWeek;
            DateTime sunday = enddate.AddDays(-currentDayOfWeek);
            DateTime monday = sunday.AddDays(1);

            if (currentDayOfWeek == 0)
            {
                monday = monday.AddDays(-7);
            }
            var dates = Enumerable.Range(0, 7).Select(days => monday.AddDays(days)).ToList();

            DateTime startd = dates[0];
            DateTime end = dates[6];


            //DateTime s = new DateTime(2016, 11, 21);
            //DateTime e = new DateTime(2016, 11, 27);

            ViewBag.currdate = today.Date.ToString("dd/MMMM");
            ViewBag.dates = dates;

            vm.days = db.days.ToList();
            vm.vslottypes = db.vslottypes.Where(o => o.occupied == 3).ToList();

            vm.vschedule = (from a in db.vschedules
                            where ((a.startdate >= startd && a.startdate <= end) ||
                            (a.enddate >= startd && a.enddate <= end) || (a.startdate <= startd && a.enddate >= end)) && a.campus == campus
                            orderby a.dayid, a.roomid, a.slottypeid, a.occupied
                            select a).ToList();
            vm.rooms = db.rooms.Where(c => c.campus == campus).ToList().OrderBy(c => c.OrderCol);
            vm.WeekWise = db.WeekWises.Where(w => w.Date >= startd && w.Date <= end).ToList();
            //var result = JsonConvert.SerializeObject(hSchedule.ToArray(),Formatting.Indented);
            if (weekno == 8)
            {
                return PartialView("MidWeek", vm);
            }
            else
            {
                return PartialView("AAC_Sch_Partial", vm);
            }
            //return Json(hSchedule.ToArray(), JsonRequestBehavior.AllowGet);



        }

        [HttpGet]
        public PartialViewResult FilterByWeekDropDown_AAC(string campus, int weekno)
        {


            CombineScheduleVM vm = new CombineScheduleVM();
            DateTime first = (FirstDateOfWeekISO8601(2016, weekno)).AddDays(245);
            DateTime second = (FirstDateOfWeekISO8601(2016, weekno)).AddDays(251);
            var weeks = Enumerable.Range(1, 18).ToList();
            ViewBag.weeks = new SelectList(weeks);
            ViewBag.week = weekno;



            DateTime today = first;
            //int currentDay = (int)today.DayOfWeek;
            int currentDayOfWeek = (int)second.DayOfWeek;
            DateTime sunday = second.AddDays(-currentDayOfWeek);
            DateTime monday = sunday.AddDays(1);

            if (currentDayOfWeek == 0)
            {
                monday = monday.AddDays(-7);
            }
            var dates = Enumerable.Range(0, 7).Select(days => monday.AddDays(days)).ToList();

            DateTime startd = dates[0];
            DateTime end = dates[6];


            ViewBag.currdate = DateTime.Now.Date.ToString("dd/MMMM");
            ViewBag.dates = dates;

            vm.days = db.days.ToList();
            vm.vslottypes = db.vslottypes.Where(o => o.occupied == 3).ToList();

            vm.WeekWise = db.WeekWises.Where(w => w.Date >= startd && w.Date <= end).ToList();

            vm.vschedule = (from a in db.vschedules
                            where ((a.startdate >= startd && a.startdate <= end) ||
                            (a.enddate >= startd && a.enddate <= end) || (a.startdate <= startd && a.enddate >= end)) && a.campus == campus
                            orderby a.dayid, a.roomid, a.slottypeid, a.occupied
                            select a).ToList();
            vm.rooms = db.rooms.Where(c => c.campus == campus).ToList().OrderBy(c => c.OrderCol);
            //var result = JsonConvert.SerializeObject(hSchedule.ToArray(),Formatting.Indented);
            if (weekno == 8)
            {
                return PartialView("MidWeek", vm);
            }
            else
            {
                return PartialView("AAC_Sch_Partial", vm);
            }

        }

        [HttpGet]
        public PartialViewResult FilterByWeekRight_AAC(string campus, string end1)
        {
            CombineScheduleVM vm = new CombineScheduleVM();
            //ViewBag.durid = durid;
            //ViewBag.dayid = dayid;
            //ViewBag.campus = campus;

            //string sd = start.Replace('/', '-');
            string ed = end1.Replace('/', '-');

            //DateTime startdate = Convert.ToDateTime(sd);
            //DateTime enddate = Convert.ToDateTime(ed);

            DateTime enddate = DateTime.ParseExact(end1, "MM/dd/yyyy", CultureInfo.InvariantCulture);


            DateTime startdate = enddate.AddDays(+1);


            DateTime today = DateTime.Today;
            //int currentDay = (int)today.DayOfWeek;
            int currentDayOfWeek = (int)enddate.DayOfWeek;
            DateTime sunday = enddate.AddDays(+currentDayOfWeek);
            DateTime monday = sunday.AddDays(1);

            if (currentDayOfWeek == 0)
            {
                monday = monday.AddDays(+0);
            }
            var dates = Enumerable.Range(0, 7).Select(days => monday.AddDays(days)).ToList();

            DateTime startd = dates[0];
            DateTime end = dates[6];


            ViewBag.currdate = today.Date.ToString("dd/MMMM");
            ViewBag.dates = dates;


            vm.WeekWise = db.WeekWises.Where(w => w.Date >= startd && w.Date <= end).ToList();
            vm.days = db.days.ToList();
            vm.vslottypes = db.vslottypes.Where(o => o.occupied == 3).ToList();

            vm.vschedule = (from a in db.vschedules
                            where ((a.startdate >= startd && a.startdate <= end) ||
                            (a.enddate >= startd && a.enddate <= end) || (a.startdate <= startd && a.enddate >= end)) && a.campus == campus
                            orderby a.dayid, a.roomid, a.slottypeid, a.occupied
                            select a).ToList();
            vm.rooms = db.rooms.Where(c => c.campus == campus).ToList().OrderBy(c => c.OrderCol);
            //var result = JsonConvert.SerializeObject(hSchedule.ToArray(),Formatting.Indented);
            return PartialView("AAC_Sch_Partial", vm);
            //return Json(hSchedule.ToArray(), JsonRequestBehavior.AllowGet);
        }







        [HttpGet]
        public PartialViewResult FilterByWeek(string campus, string start)
        {
            CombineScheduleVM vm = new CombineScheduleVM();
            //ViewBag.durid = durid;
            //ViewBag.dayid = dayid;
            //ViewBag.campus = campus;

            string sd = start.Replace('/', '-');
            //string ed = end.Replace('/', '-');

            DateTime startdate = Convert.ToDateTime(sd);
            //DateTime enddate = Convert.ToDateTime(ed);


            DateTime enddate = startdate.AddDays(-1);


            DateTime today = DateTime.Today;
            //int currentDay = (int)today.DayOfWeek;
            int currentDayOfWeek = (int)enddate.DayOfWeek;
            DateTime sunday = enddate.AddDays(-currentDayOfWeek);
            DateTime monday = sunday.AddDays(1);

            if (currentDayOfWeek == 0)
            {
                monday = monday.AddDays(-7);
            }
            var dates = Enumerable.Range(0, 7).Select(days => monday.AddDays(days)).ToList();

            DateTime startd = dates[0];
            DateTime end = dates[6];


            ViewBag.currdate = today.Date.ToString("dd/MMMM");
            ViewBag.dates = dates;

            vm.days = db.days.ToList();
            vm.vslottypes = db.vslottypes.Where(o => o.occupied == 3).ToList();

            vm.vschedule = (from a in db.vschedules
                            where ((a.startdate >= startd && a.startdate <= end) ||
                            (a.enddate >= startd && a.enddate <= end) || (a.startdate <= startd && a.enddate >= end)) && a.campus == campus
                            orderby a.dayid, a.roomid, a.slottypeid, a.occupied
                            select a).ToList();
            vm.rooms = db.rooms.Where(c => c.campus == campus).ToList().OrderBy(c=>c.OrderCol);
            //var result = JsonConvert.SerializeObject(hSchedule.ToArray(),Formatting.Indented);
            return PartialView("Sch_Partial", vm);
            //return Json(hSchedule.ToArray(), JsonRequestBehavior.AllowGet);



        }





        public ActionResult TotalRoomsByCampus(string campus)
        {

            var rooms = db.rooms.Where(r => r.campus == campus).ToArray().Select(S => new
            {
                roomid = S.roomid,
                roomno = S.roomno,
                
                
            });
            
            return Json(rooms, JsonRequestBehavior.AllowGet);

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



        public JsonResult UsernameValidate(string username)
        {
            Thread.Sleep(2000);
            return Json(!db.Logins.Any(l => l.Username == username),JsonRequestBehavior.AllowGet);
            
        }



        public ActionResult GetAllTeachers()
        {


            var teachers = db.teachers.Select(s => new
            {
                value = s.teacherid,
                label = s.teachername
            }).ToArray();


            return Json(teachers, JsonRequestBehavior.AllowGet);

        }


        public ActionResult OffCourseBySchid(int schid)
        {
            CombineScheduleVM vm = new CombineScheduleVM();

            var schedule = db.vschedules.Where(s=>s.schid == schid).Select(s => new
            {

                s.teacherid,
                s.teachername,
                s.offid,
                course = s.code + "-" + s.title + "-" + s.sectionname

            }).ToArray();

            

            //var teachers = db.teachers.Select(s => new
            //{
            //    value = s.teacherid,
            //    label = s.teachername
            //}).ToArray();


            var off = db.voffcourses.Where(o=>o.STID==1).ToList();
            //    var off = db.sections.Include(s=>s.)
            var courses = off.Select(o => new
            {
                value = o.offid,
                label = o.teachername + " - "+o.code + "-" + o.title + "-" + o.sectionname
            }).ToArray();


            return Json(new
            { schedule = schedule,
              courses = courses
            }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetAllCourses()
        {

            var off = db.voffcourses.Where(o=>o.STID==1).ToList();
            //    var off = db.sections.Include(s=>s.)
            var courses = off.Select(o => new
            {
                value = o.offid,
                label = o.teachername+ " - "+ o.code + "-" + o.title + "-" + o.sectionname
            }).ToArray();


            return Json(courses, JsonRequestBehavior.AllowGet);

        }


        public ActionResult GetCancelledCourses()
        {

//            select v.schid,o.offid,o.courseid,o.secid,o.title,o.sectionname,v.teacherid,v.teachername,v.occupied,v.SchCTId from vschedule v
//join voffcourses o on v.offid = o.offid
//join Weekwise w on v.schid = w.schid
//where w.CTId = 2

            //var query =
            //   from post in database.Posts
            //   join meta in database.Post_Metas on post.ID equals meta.Post_ID
            //   where post.ID == id
            //   select new { Post = post, Meta = meta };

            var off =
               (from vschedule v in db.vschedules
               join voffcours o in db.voffcourses on v.offid equals o.offid
               join WeekWise w in db.WeekWises on v.schid equals w.schid
               where w.CTId == 2
               select new {o.code,o.offid,o.sectionname,o.title }).ToList();

            //var co = off.ToList();
            //var off = db.schedules.Include(i=>i.).ToList();
            //    var off = db.sections.Include(s=>s.)
            var courses = off.Select(o => new
            {
                value = o.offid,
                label = o.code + "-" + o.title + "-" + o.sectionname
            }).ToArray();


            return Json(courses, JsonRequestBehavior.AllowGet);

        }







        [HttpPost]
        public JsonResult InsertSchedule(int[] offids, int occupied, int dayid, int slottypeid, int roomid, string start)
        {

            //Thread.Sleep(10000);
            DateTime startdate = DateTime.ParseExact(start, "MM/dd/yyyy", CultureInfo.InvariantCulture);

            int slotid = db.slots.Where(s => s.dayid == dayid && s.slottypeid == slottypeid && s.roomid == roomid).Select(s => s.slotid).Single();

            //DateTime startDate = new DateTime(2017, 2, 13);
            DateTime endDate = new DateTime(2017, 6, 12);

            int sid = 2;
            var IsClass =  (from a in db.vschedules
                 where ((a.startdate >= startdate && a.startdate <= endDate) ||
                 (a.enddate >= startdate && a.enddate <= endDate) || (a.startdate <= startdate && a.enddate >= endDate))
                 && a.flag == true && a.slotid == slotid
                 select a).ToList();

            if(IsClass.Count()>0){
                return Json("NO", JsonRequestBehavior.AllowGet);
            }
            else
            {

                //List<int> off= List<>;


                foreach (var offid in offids)
            {
                
                db.schedules.Add(new schedule
                {
                    offid = offid,
                    slotid = slotid,
                    occupied = occupied,
                    StartDate = startdate,
                    EndDate = endDate,
                    SId = sid


                });
            }
                db.SaveChanges();
            //return null;


            if (offids.Count() == 1)
            {
                int offid1 = offids[0];
                
                return Json(db.vschedules.Where(o => o.offid == offid1 && o.slotid == slotid && o.occupied == occupied && o.flag == true).ToArray(), JsonRequestBehavior.AllowGet);
            }
            else if (offids.Count() == 2)
            {
                int offid1 = offids[0];
                int offid2 = offids[1];
                
                return Json(db.vschedules.Where(o => (o.offid == offid1 || o.offid == offid2) && o.slotid == slotid && o.occupied == occupied && o.flag == true).ToArray(), JsonRequestBehavior.AllowGet);
            }
            else if (offids.Count() == 3)
            {
                int offid1 = offids[0];
                int offid2 = offids[1];
                int offid3 = offids[2];
                
                return Json(db.vschedules.Where(o => (o.offid == offid1 || o.offid == offid2 || o.offid == offid3) && o.slotid == slotid && o.occupied == occupied && o.flag == true).ToArray(), JsonRequestBehavior.AllowGet);
            }
            else if (offids.Count() == 4)
            {
                int offid1 = offids[0];
                int offid2 = offids[1];
                int offid3 = offids[2];
                int offid4 = offids[3];
                return Json(db.vschedules.Where(o => (o.offid == offid1 || o.offid == offid2 || o.offid == offid3 || o.offid == offid4) && o.slotid == slotid && o.occupied == occupied && o.flag == true).ToArray(), JsonRequestBehavior.AllowGet);
            }
            }
            return null;
            //db.vschedules.Where(o => o.offid == offid && o.slotid == slotid && o.occupied == occupied).Single();


            //return Json(db.vschedules.Where(o => o.offid == 538 && o.slotid == 697 && o.occupied == 3).Single(), JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult MakeupClass(int offid, int occupied, int dayid, int slottypeid, int roomid,int ctid,DateTime startdate,DateTime enddate)
        {
            DateTime date = new DateTime();
            int day = ((int)startdate.DayOfWeek == 0) ? 7 : (int)startdate.DayOfWeek;
            var dates = Enumerable.Range(0, 7).Select(days => startdate.AddDays(days)).ToList();
            //.ForEach(s=> s.).Where(s => (int)s.DayOfWeek == dayid).Select(s => s.Date);
            
            foreach(var d in dates)
            {
                if((int)d.DayOfWeek == dayid)
                {
                    date = d.Date;
                }
            }


            int slotid = db.slots.Where(s => s.dayid == dayid && s.slottypeid == slottypeid && s.roomid == roomid).Select(s => s.slotid).Single();

            

            //DateTime startDate = new DateTime(2016, 9, 5);
            //DateTime endDate = new DateTime(2017, 1, 9);

            if (db.vschedules.Where(o => o.offid == offid && o.slotid == slotid && o.occupied == occupied).Count() > 0)
            {
                return Json("NO", JsonRequestBehavior.AllowGet);
            }
            else
            {

                db.schedules.Add(new schedule
                {
                    offid = offid,
                    slotid = slotid,
                    occupied = occupied,
                    CTId=ctid,
                    StartDate = startdate,
                    EndDate = enddate,
                    SId=2


                });
                db.SaveChanges();

                var slot = (from s in db.slots
                            join
                            d in db.days on s.dayid equals d.dayid
                            join
                            r in db.rooms on s.roomid equals r.roomid
                            join
                            st in db.slottypes on s.slottypeid equals st.slottypeid
                            where s.slotid == slotid
                            select new { s, d, r, st }).Single();

                var students_email = (from s in db.sections
                                      join
                                      st in db.Students on s.secid equals st.secid
                                      join
                                      o in db.offeredcourses on s.secid equals o.secid
                                      join
                                      c in db.courses on o.courseid equals c.courseid
                                      join
                                      t in db.teachers on s.PId equals t.PId
                                      where o.offid == offid
                                      select new { s, st, o, c, t }).ToList();

                foreach (var email in students_email)
                {
                    var senderEmail = new MailAddress("szabistdemo@gmail.com", "SZABIST");
                    var receiverEmail = new MailAddress(email.st.Email, email.st.Email);

                    //var program = db.sections.Where()

                    var password = "demoforszabist";
                    var subject = "SZABIST Email";
                    var body = email.s.Program.Program1 + " " + email.s.Class.Class1 + " " + email.s.sectionname + ": " + email.c.title + " [" + email.o.teacher.teachername + "] "
                        + "makeup class will be held on " + date.ToString("dddd") + " " + date.Day + " " + date.ToString("MMMM") + ", " + slot.st.duration + " at Room " +
                        slot.r.roomno + " " + slot.r.campus + " campus. ZABDESK";


                    var smtp = new SmtpClient
                    {

                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)

                    };

                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = senderEmail;
                    mailMessage.To.Add(receiverEmail);
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;

                    smtp.Send(mailMessage);
                }


                return Json(db.vschedules.Where(o => o.offid == offid && o.slotid == slotid && o.occupied == occupied).Single(), JsonRequestBehavior.AllowGet);
            }

            //db.vschedules.Where(o => o.offid == offid && o.slotid == slotid && o.occupied == occupied).Single();


            //return Json(db.vschedules.Where(o => o.offid == 538 && o.slotid == 697 && o.occupied == 3).Single(), JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult WeekWiseClassHeld(int[] schid_splits_ClsHeld, TimeSpan cls_start, TimeSpan cls_end, int? cls_break, DateTime date, int weekno, int type)
        {
            //if (db.vschedules.Where(o => o.offid == offid && o.slotid == slotid && o.occupied == occupied).Count() > 0)
            //{
            //    return Json("NO", JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            //string sd = start.Replace('/', '-');
            ////string ed = end.Replace('/', '-');

            //DateTime startdate = Convert.ToDateTime(sd);
            int sch1 = schid_splits_ClsHeld[0];

            foreach (int schid in schid_splits_ClsHeld)
            {

                db.WeekWises.Add(new WeekWise
                {
                    schid = schid,
                    ClassStarted = cls_start,
                    ClassEnded = cls_end,
                    BreakDuration = cls_break,
                    Date = date,
                    Week_no = weekno,
                    CTId = type

                });
                db.SaveChanges();
            }
            return Json(db.WeekWises.Where(w => w.schid == sch1 && w.Date == date).Single(), JsonRequestBehavior.AllowGet);

            //}



        }



        public JsonResult WeekWiseClassCancel(int[] schid_split_frClassCancel, DateTime date, int weekno, int type)
        {
            int sch2 = schid_split_frClassCancel[0];
            int dayid = (int)date.DayOfWeek;

            //if (db.vschedules.Where(o => o.offid == offid && o.slotid == slotid && o.occupied == occupied).Count() > 0)
            //{
            //    return Json("NO", JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            //string sd = start.Replace('/', '-');
            ////string ed = end.Replace('/', '-');

            //DateTime startdate = Convert.ToDateTime(sd);



            foreach (int schid in schid_split_frClassCancel)
            {

                db.WeekWises.Add(new WeekWise
                {
                    schid = schid,
                    Date = date,
                    Week_no = weekno,
                    CTId = type

                });
                db.SaveChanges();

                var offid = db.vschedules.Where(s => s.schid == schid).Select(s => s.offid).Single();
                var students_email = (from s in db.sections
                                      join
                                      st in db.Students on s.secid equals st.secid
                                      join
                                      o in db.offeredcourses on s.secid equals o.secid
                                      join
                                      c in db.courses on o.courseid equals c.courseid
                                      join
                                      t in db.teachers on s.PId equals t.PId
                                      join
                                      v in db.vschedules on o.offid equals v.offid
                                      where o.offid == offid && v.SchCTId == null && v.dayid == dayid
                                      select new { s, st, o, c, t, v }).ToList();

                foreach (var email in students_email)
                {
                    var senderEmail = new MailAddress("szabistdemo@gmail.com", "SZABIST");
                    var receiverEmail = new MailAddress(email.st.Email, email.st.Email);

                    //var program = db.sections.Where()

                    var password = "demoforszabist";
                    var subject = "SZABIST Email";
                    var body = email.s.Program.Program1 + " " + email.s.Class.Class1 + " " + email.s.sectionname + ": " + email.c.title + " [" + email.o.teacher.teachername + "] "
                        + "regular class scheduled for " + date.ToString("dddd") + " " + date.Day + " " + date.ToString("MMMM") + " " + email.v.duration + " has been cancelled. ZABDESK"
                        ;


                    var smtp = new SmtpClient
                    {

                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)

                    };

                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = senderEmail;
                    mailMessage.To.Add(receiverEmail);
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;

                    smtp.Send(mailMessage);
                }

            }


            

            return Json(db.WeekWises.Where(w => w.schid == sch2 && w.Date == date).Single(), JsonRequestBehavior.AllowGet);
            //}



        }




        public PartialViewResult PmSemDropDown(int semesterNumber)
        {

            var data = (SessionVM)Session["Username"];

            //var pid = data.teachername;
            var pid = data.PId;
            ViewBag.teachername = data.teachername;

            CombineScheduleVM vm = new CombineScheduleVM();

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

            ViewBag.dates = dates;

            if (semesterNumber != 0)
            {
                
                vm.days = db.days.ToList();
                vm.vslottypes = db.vslottypes.Where(o => o.occupied == 3).ToList();

                //vm.sections = db.sections.Where(a => a.sectionname.Contains("BCS/BS " + semesterNumber) && a.STID == 3).ToList();

                vm.sections = db.sections.Where(a => a.ClassId == semesterNumber && a.STID == 1 && a.PId == pid).ToList();
                //vm.slottypes = db.slottypes.Where(st => st.slottype1 == 1).ToList();
                //vm.rooms = db.rooms.ToList();


                //select* from vschedule where secid = 1 and slottypeid = 1 and dayid = 1

                ViewBag.numbrOfClasses = vm.sections.Count();

                vm.vschedule = (from a in db.vschedules
                                where ((a.startdate >= start && a.startdate <= end) ||
                                (a.enddate >= start && a.enddate <= end) || (a.startdate <= start && a.enddate >= end))
                                &&  a.flag == true
                                && a.PId == pid
                                && a.ClassID == semesterNumber
                                orderby a.dayid, a.slottypeid, a.secid, a.occupied
                                select a).ToList();

                return PartialView("PmSemDrop_partial", vm);

            }
            else
            {
                vm.days = db.days.ToList();
                vm.vslottypes = db.vslottypes.Where(o => o.occupied == 3).ToList();

                vm.sections = db.sections.Where(a => a.PId == pid && a.STID == 1).ToList();

                
                vm.vschedule = (from a in db.vschedules
                                where ((a.startdate >= start && a.startdate <= end) ||
                                (a.enddate >= start && a.enddate <= end) || (a.startdate <= start && a.enddate >= end))
                                && a.flag == true
                                && a.PId == pid
                                orderby a.dayid, a.slottypeid, a.secid, a.occupied
                                select a).ToList();

                return PartialView("AllPmSemDrop_partial", vm);


            }

           


        }



        public JsonResult CheckIfSlotEmpty(int dayid, int slottypeid , int secid)
        {
            ReportsVM vm = new ReportsVM();

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

            var PM_EmptyRooms = db.Database.SqlQuery<PMEmptyRooms_Result>("PMEmptyRooms @dayid, @slottypeid, @startdate, @enddate", new SqlParameter("@dayid", dayid)
                           , new SqlParameter("@slottypeid", slottypeid)
                           , new SqlParameter("@startdate", start), new SqlParameter("@enddate", end)).ToList();

            var empty_rooms = PM_EmptyRooms.Select(pm => new
            {
                value = pm.slotid,
                label = pm.slotname
            }).ToArray();

            var off = db.voffcourses.Where(o => o.STID == 1 && o.secid == secid).ToList();
            //    var off = db.sections.Include(s=>s.)
            var courses = off.Select(o => new
            {
                value = o.offid,
                label = o.code + "-" + o.title + "-" + o.sectionname
            }).ToArray();



            //vm.PMEmptyRooms = PM_EmptyRooms;

            return Json(new { empty = empty_rooms, courses = courses }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult CheckIfSlotEmptyByDate(DateTime date,int temp_slottypeid)
        {
            ReportsVM vm = new ReportsVM();

            //DateTime today = DateTime.Now;
            int dayid = (int)date.DayOfWeek;
            DateTime sunday = date.AddDays(-dayid);

            DateTime monday = sunday.AddDays(1);

            //DateTime mon = monday.Date.ToString("dd/MM/YYYY");
            if (dayid == 0)
            {
                monday = monday.AddDays(-7);
            }
            var dates = Enumerable.Range(0, 7).Select(days => monday.AddDays(days)).ToList();
            DateTime start = dates[0].Date;
            DateTime end = dates[6].Date;

            var PM_EmptyRooms = db.Database.SqlQuery<PMEmptyRooms_Result>("PMEmptyRooms @dayid, @slottypeid, @startdate, @enddate", new SqlParameter("@dayid", dayid)
                           , new SqlParameter("@slottypeid", temp_slottypeid)
                           , new SqlParameter("@startdate", start), new SqlParameter("@enddate", end)).ToList();

            var empty_rooms = PM_EmptyRooms.Select(pm => new
            {
                value = pm.slotid,
                label = pm.slotname
            }).ToArray();

            return Json(empty_rooms,JsonRequestBehavior.AllowGet);

        }


        public JsonResult GetCoursesByProgram()
        {
            var data = (SessionVM)Session["Username"];

            var pid = data.PId;
            //ViewBag.pid = data.PId;

            //ViewBag.teachername = data.teachername;

            var off = db.offeredcourses.Where(o => o.section.STID == 1 && o.section.Program.PId==pid).ToList();
            //    var off = db.sections.Include(s=>s.)
            var courses = off.Select(o => new
            {
                value = o.offid,
                label = o.course.code + "-" + o.course.title + "-"+ o.section.Program.Program1 + "-" + o.section.Class.Class1 + "-" +  o.section.sectionname
            }).ToArray();


            return Json(courses, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetCoursesByTeacherId()
        {
            var data = (SessionVM)Session["Username"];

            var teacherid = data.teacherid;
            //ViewBag.pid = data.PId;

            //ViewBag.teachername = data.teachername;

            var off = db.offeredcourses.Where(o => o.section.STID == 1 && o.teacher.teacherid == teacherid).ToList();
            //    var off = db.sections.Include(s=>s.)
            var courses = off.Select(o => new
            {
                value = o.offid,
                label = o.course.code + "-" + o.course.title + "-" + o.section.Program.Program1 + "-" + o.section.Class.Class1 + "-" + o.section.sectionname
            }).ToArray();


            return Json(courses, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetDaysByOffid(int offid)
        {
            var days = db.vschedules.Where(s => s.offid == offid && s.SchCTId == null).Select(s => s.dayid).ToArray();

            return Json(days, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public PartialViewResult AddRequest(int offid, int? slotid,int CTID, DateTime date,int? occupied)
        {
            var data = (SessionVM)Session["Username"];

            var teacherid = data.teacherid;
            //DateTime today = DateTime.Now;

            
                db.Requests.Add(new Request
                {
                    offid = offid,
                    teacherid = teacherid,
                    slotid = slotid,
                    Date = date,
                    CTID = CTID,
                    Status = false,
                    occupied = occupied

                });
                db.SaveChanges();
                //return Json("false", JsonRequestBehavior.AllowGet);
            //}
            

            //Thread.Sleep(2000);
            
           
            var last = db.Requests.Include(s=>s.offeredcourse).Include(s=>s.slot).ToList().Last();
            //ViewBag.Id = last.Last().Id;
            
            
            //var Id = last.Id;
            //var a = db.Requests.Where(s => s.Id == Id).ToList();

            return PartialView("last_added_request", last);
        }

        //[HttpPost]
        //public JsonResult WeekWiseClassHeld(int schid, TimeSpan cls_start, TimeSpan cls_end, int? cls_break, DateTime date,int weekno,int type)
        //{
        //    //if (db.vschedules.Where(o => o.offid == offid && o.slotid == slotid && o.occupied == occupied).Count() > 0)
        //    //{
        //    //    return Json("NO", JsonRequestBehavior.AllowGet);
        //    //}
        //    //else
        //    //{
        //    //string sd = start.Replace('/', '-');
        //    ////string ed = end.Replace('/', '-');

        //    //DateTime startdate = Convert.ToDateTime(sd);
        //    db.WeekWises.Add(new WeekWise
        //    {
        //        schid = schid,
        //        ClassStarted = cls_start,
        //        ClassEnded = cls_end,
        //        BreakDuration = cls_break,
        //        Date = date,
        //        Week_no= weekno,
        //        CTId=type

        //    });
        //    db.SaveChanges();
        //    return Json(db.WeekWises.Where(w => w.schid == schid && w.Date==date).Single(), JsonRequestBehavior.AllowGet);
        //    //}



        //}



        //public JsonResult WeekWiseClassCancel(int schid, DateTime date, int weekno, int type)
        //{

        //    //if (db.vschedules.Where(o => o.offid == offid && o.slotid == slotid && o.occupied == occupied).Count() > 0)
        //    //{
        //    //    return Json("NO", JsonRequestBehavior.AllowGet);
        //    //}
        //    //else
        //    //{
        //    //string sd = start.Replace('/', '-');
        //    ////string ed = end.Replace('/', '-');

        //    //DateTime startdate = Convert.ToDateTime(sd);
        //    db.WeekWises.Add(new WeekWise
        //    {
        //        schid = schid,
        //        Date = date,
        //        Week_no = weekno,
        //        CTId = type

        //    });
        //    db.SaveChanges();
        //    return Json(db.WeekWises.Where(w => w.schid == schid && w.Date == date).Single(), JsonRequestBehavior.AllowGet);
        //    //}



        //}


        public PartialViewResult FilterEmptyRoomBySlottype(int slottypeid,string start, string end, string campus,int dayid)
        {
            ReportsVM vm = new ReportsVM();

            //string sd = startdate.Replace('/', '-');
            //string ed = enddate.Replace('/', '-');

            //DateTime startdate1 = Convert.ToDateTime(sd);
            //DateTime enddate1 = Convert.ToDateTime(ed);
            var empty = db.Database.SqlQuery<EmptyRooms_Result>("EmptyRooms @dayid, @campus, @slottypeid, @startdate, @enddate", new SqlParameter("@dayid", dayid),
                           new SqlParameter("@campus", campus), new SqlParameter("@slottypeid", slottypeid)
                           , new SqlParameter("@startdate", start), new SqlParameter("@enddate", end)).ToList();
            vm.empty_rooms = empty;

          
            //return null;
            return PartialView("Report_Partial_Empty",vm);
        }

        public PartialViewResult FilterEmptyRoomCountBySlottype(int dayid,int slottypeid)
        {
            ReportsVM vm = new ReportsVM();

            //string sd = startdate.Replace('/', '-');
            //string ed = enddate.Replace('/', '-');

            //DateTime start = new DateTime(2016, 09, 05);
            //DateTime end = new DateTime(2016, 09, 11);

            //DateTime start = db.Semesters.Where(s => s.flag == true).Select(s => s.SemesterStartDate).Single();
            //DateTime end = db.Semesters.Where(s => s.flag == true).Select(s => s.SemesterEndDate).Single();

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



            var empty = db.Database.SqlQuery<EmptyRooms_Result>("EmptyRooms @dayid, @slottypeid, @startdate, @enddate", new SqlParameter("@dayid", dayid)
                           ,new SqlParameter("@slottypeid", slottypeid)
                           , new SqlParameter("@startdate", start), new SqlParameter("@enddate", end)).OrderBy(s=>s.campus).ToList();
            vm.empty_rooms = empty;

            Thread.Sleep(1000);

            //return null;
            return PartialView("Report_Partial_Empty", vm);
        }

        public PartialViewResult MonitoringByOffid(int offid)
        {
            ReportsVM vm = new ReportsVM();



            var monitoring = db.Database.SqlQuery<MonitoringByOffId_Result>("MonitoringByOffId @offid", new SqlParameter("@offid", offid)).OrderBy(s=>s.Date).ToList();
            vm.monitoringbyoffid = monitoring;
            return PartialView("Report_Partial_Monitoring",vm);

        }




        [HttpPost]
        public JsonResult PREdit(int schid, string end)
        {

            DateTime enddate = DateTime.ParseExact(end, "MM/dd/yyyy", CultureInfo.InvariantCulture);

            schedule schedule = new schedule();
            db.schedules.Find(schid).EndDate = enddate;
            //db.Entry(schedule).State = EntityState.Modified;
            
            db.SaveChanges();

            return null;
            //return Json(db.WeekWises.Where(w => w.schid == schid && w.Date == date).Single(), JsonRequestBehavior.AllowGet);
            //}



        }


        [HttpPost]
        public JsonResult NormalEdit(int schid, int offid)
        {

            //DateTime enddate = DateTime.ParseExact(end, "MM/dd/yyyy", CultureInfo.InvariantCulture);

            //schedule schedule = new schedule();
            //db.schedules.Find(schid).teacherid = teacherid;

            db.schedules.Find(schid).offid = offid;
            //db.Entry(schedule).State = EntityState.Modified;

            db.SaveChanges();

            //return null;
            return Json(db.vschedules.Where(w => w.schid == schid).Single(), JsonRequestBehavior.AllowGet);
            //}



        }


        [HttpPost]
        public JsonResult ApproveCancelRequest(int requestid,int offid, DateTime date)
        {
            

            int dayid = (int)date.DayOfWeek;
            int currentDayOfWeek = (int)date.DayOfWeek;
            DateTime sunday = date.AddDays(-currentDayOfWeek);
            DateTime monday = sunday.AddDays(1);

            //DateTime mon = monday.Date.ToString("dd/MM/YYYY");
            if (currentDayOfWeek == 0)
            {
                monday = monday.AddDays(-7);
            }
            var dates = Enumerable.Range(0, 7).Select(days => monday.AddDays(days)).ToList();
            DateTime start = dates[0].Date;
            DateTime end = dates[6].Date;

            var schid = db.vschedules.Where(a => ((a.startdate >= start && a.startdate <= end) ||
            (a.enddate >= start && a.enddate <= end) || (a.startdate <= start && a.enddate >= end))
            && a.offid == offid && a.dayid == dayid)
            .Select(a => a.schid).Single();

            db.Requests.Find(requestid).Status = true;

            db.Requests.Find(requestid).last_updated = DateTime.Now.Date;


            db.WeekWises.Add(new WeekWise
            {
                schid = schid,
                Date = date,
                CTId = 2

            });
            db.SaveChanges();
            //select* from section s join Student st on s.secid = st.secid join offeredcourse o on s.secid = o.secid join course c on c.courseid = o.courseid where offid = 1230
            var students_email = (from s in db.sections
                                  join
                                  st in db.Students on s.secid equals st.secid
                                  join
                                  o in db.offeredcourses on s.secid equals o.secid
                                  join
                                  c in db.courses on o.courseid equals c.courseid
                                  join 
                                  t in db.teachers on s.PId equals t.PId
                                  join
                                  v in db.vschedules on o.offid equals v.offid
                                  where o.offid == offid && v.SchCTId == null && v.dayid == dayid select new { s,st,o,c,t,v }).ToList();

            foreach (var email in students_email)
            {
                var senderEmail = new MailAddress("szabistdemo@gmail.com", "SZABIST");
                var receiverEmail = new MailAddress(email.st.Email, email.st.Email);

                //var program = db.sections.Where()

                var password = "demoforszabist";
                var subject = "SZABIST Email";
                var body = email.s.Program.Program1+" "+email.s.Class.Class1+" "+email.s.sectionname+": "+email.c.title +" ["+email.o.teacher.teachername+"] "
                    +"regular class scheduled for "+date.ToString("dddd") +" " +date.Day +" "+date.ToString("MMMM") +" "+email.v.duration+" has been cancelled. ZABDESK"
                    ;


                var smtp = new SmtpClient
                {

                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail.Address, password)

                };

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = senderEmail;
                mailMessage.To.Add(receiverEmail);
                mailMessage.Subject = subject;
                mailMessage.Body = body;

                smtp.Send(mailMessage);
            }

            //SmtpClient client = new SmtpClient();
            

            //MailMessage mailMessage = new MailMessage();
            //mailMessage.From = new MailAddress("someone@somewhere.com");
            //mailMessage.To.Add("sheheryaarkhan97@yahoo.com");
            //mailMessage.Subject = "Hello There";
            //mailMessage.Body = "Hello my friend!";

            //client.Send(mailMessage);

            return Json("Class cancellation approved!", JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult ApproveMakeupRequest(int requestid,int offid, int slotid,int occupied ,DateTime date)
        {

            //int dayid = (int)date.DayOfWeek;

            //var teacherid = db.vschedules.Where(s => s.offid == offid).Select(s => s.schid).Single();

            //DateTime today = date;
            int currentDayOfWeek = (int)date.DayOfWeek;
            DateTime sunday = date.AddDays(-currentDayOfWeek);
            DateTime monday = sunday.AddDays(1);

            //DateTime mon = monday.Date.ToString("dd/MM/YYYY");
            if (currentDayOfWeek == 0)
            {
                monday = monday.AddDays(-7);
            }
            var dates = Enumerable.Range(0, 7).Select(days => monday.AddDays(days)).ToList();
            DateTime start = dates[0].Date;
            DateTime end = dates[6].Date;



            DateTime start1 = DateTime.Now;

            DateTime end1 = db.Semesters.Where(s => s.flag == true).Select(s => s.SemesterEndDate).Single();
            

            db.Requests.Find(requestid).Status = true;

            db.Requests.Find(requestid).last_updated = DateTime.Now.Date;


            db.schedules.Add(new schedule
            {
                
                StartDate = start,
                EndDate = end,
                offid=offid,
                slotid=slotid,
                occupied=occupied,
                CTId = 3,
                SId=2

            });
            db.SaveChanges();

            var slot = (from s in db.slots
                        join
                        d in db.days on s.dayid equals d.dayid
                        join
                        r in db.rooms on s.roomid equals r.roomid
                        join
                        st in db.slottypes on s.slottypeid equals st.slottypeid
                        where s.slotid == slotid
                        select new {s,d,r,st}).Single();

            var students_email = (from s in db.sections
                                  join
                                  st in db.Students on s.secid equals st.secid
                                  join
                                  o in db.offeredcourses on s.secid equals o.secid
                                  join
                                  c in db.courses on o.courseid equals c.courseid
                                  join
                                  t in db.teachers on s.PId equals t.PId
                                  where o.offid == offid
                                  select new { s, st, o, c, t }).ToList();

            foreach (var email in students_email)
            {
                var senderEmail = new MailAddress("szabistdemo@gmail.com", "SZABIST");
                var receiverEmail = new MailAddress(email.st.Email, email.st.Email);

                //var program = db.sections.Where()

                var password = "demoforszabist";
                var subject = "SZABIST Email";
                var body = email.s.Program.Program1 + " " + email.s.Class.Class1 + " " + email.s.sectionname + ": " + email.c.title + " [" + email.o.teacher.teachername + "] "
                    + "makeup class will be held on " + date.ToString("dddd") + " " + date.Day + " " + date.ToString("MMMM") + ", " + slot.st.duration+ " at Room "+ 
                    slot.r.roomno +" "+slot.r.campus +" campus. ZABDESK";


                var smtp = new SmtpClient
                {

                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail.Address, password)

                };

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = senderEmail;
                mailMessage.To.Add(receiverEmail);
                mailMessage.Subject = subject;
                mailMessage.Body = body;

                smtp.Send(mailMessage);
            }

            return Json("Class Makeup approved!", JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public ActionResult Edit_teacher(int Teacher_id)
        {
            db.Configuration.ProxyCreationEnabled = false;

            var teacher = db.teachers.Where(m => m.teacherid == Teacher_id).Single();

            if (teacher == null)
            {
                return HttpNotFound();


            }
            else
            {
                return Json(teacher, JsonRequestBehavior.AllowGet);



            }






        }

    }

}




        
    
