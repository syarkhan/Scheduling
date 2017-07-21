using Scheduling.Models;
using Scheduling.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scheduling.Controllers
{
    public class AACController : Controller
    {
        DBEntities db = new DBEntities();


        //public ActionResult Reports()
        //{

        //    //var rooms_empty = from slot s in db.slots join day d in db.days on s.dayid equals d.dayid
        //    //                  join vslottype st in db.vslottypes on s.slottypeid equals st.slottypeid
        //    //                  join room r in db.rooms on s.roomid equals r.roomid
        //    //                  where d.dayid == 1 && r.campus == "90" && !(from slot s in db.slots join vschedule sch in db.vschedules on s.slotid equals sch.slotid where s.dayid == 1 group s.slotid by s)
        //    //                 select new {s.roomid,r.roomno,r.campus,st.duration,st.occupied,st.slottypeid} 

        //    ReportsVM vm = new ReportsVM();
        //    //var empty=List<>;
        //    DateTime today = DateTime.Now;
        //    int currentDayOfWeek = (int)today.DayOfWeek;
        //    DateTime sunday = today.AddDays(-currentDayOfWeek);
        //    DateTime monday = sunday.AddDays(1);

        //    //DateTime mon = monday.Date.ToString("dd/MM/YYYY");
        //    if (currentDayOfWeek == 0)
        //    {
        //        monday = monday.AddDays(-7);
        //    }
        //    var dates = Enumerable.Range(0, 7).Select(days => monday.AddDays(days)).ToList();
        //    DateTime start = dates[0].Date;
        //    DateTime end = dates[6].Date;


        //    ViewBag.startdate = start;
        //    ViewBag.enddate = end;

        //    var day = from d in db.days
        //               select new
        //               {
        //                   Id = d.dayid,
        //                   Name = d.dayname
        //               };

        //    ViewBag.days = new SelectList(day, "Id", "Name", currentDayOfWeek);

        //    var hour = today.Hour;

        //    var slottypes = from s in db.slottypes
        //                    where s.slottypeid <= 4
        //                    select new
        //                    {
        //                        Id = s.slottypeid,
        //                        Name = s.duration
        //                    };


        //    if (hour >=0 && hour <=10 || hour >=22 && hour <=23)
        //    {
        //        ViewBag.slottypes = new SelectList(slottypes, "Id", "Name", 1);
        //        var empty = db.Database.SqlQuery<EmptyRooms_Result>("EmptyRooms @dayid, @campus, @slottypeid, @startdate, @enddate", new SqlParameter("@dayid", currentDayOfWeek),
        //                    new SqlParameter("@campus", "90"), new SqlParameter("@slottypeid", "1")
        //                    , new SqlParameter("@startdate", start), new SqlParameter("@enddate", end)).ToList();
        //        vm.empty_rooms = empty;
        //    }
        //    else if(hour >= 11 && hour <= 14)
        //    {
        //        ViewBag.slottypes = new SelectList(slottypes, "Id", "Name", 2);
        //        var empty = db.Database.SqlQuery<EmptyRooms_Result>("EmptyRooms @dayid, @campus, @slottypeid, @startdate, @enddate", new SqlParameter("@dayid", currentDayOfWeek),
        //                    new SqlParameter("@campus", "90"), new SqlParameter("@slottypeid", "2")
        //                    , new SqlParameter("@startdate", start), new SqlParameter("@enddate", end)).ToList();
        //        vm.empty_rooms = empty;
        //    }
        //    else if(hour >= 15 && hour <= 17)
        //    {
        //        ViewBag.slottypes = new SelectList(slottypes, "Id", "Name", 3);
        //        var empty = db.Database.SqlQuery<EmptyRooms_Result>("EmptyRooms @dayid, @campus, @slottypeid, @startdate, @enddate", new SqlParameter("@dayid", currentDayOfWeek),
        //                    new SqlParameter("@campus", "90"), new SqlParameter("@slottypeid", "3")
        //                    , new SqlParameter("@startdate", start), new SqlParameter("@enddate", end)).ToList();
        //        vm.empty_rooms = empty;
        //    }
        //    else if(hour >= 18 && hour <= 21)
        //    {
        //        ViewBag.slottypes = new SelectList(slottypes, "Id", "Name", 4);
        //        var empty = db.Database.SqlQuery<EmptyRooms_Result>("EmptyRooms @dayid, @campus, @slottypeid, @startdate, @enddate", new SqlParameter("@dayid", currentDayOfWeek),
        //                     new SqlParameter("@campus", "90"), new SqlParameter("@slottypeid", "4")
        //                     , new SqlParameter("@startdate", start), new SqlParameter("@enddate", end)).ToList();
        //        vm.empty_rooms = empty;
        //    }




        //    return View(vm);
        //}

        public ActionResult AddTeacher()
        {

            CombineScheduleVM vm = new CombineScheduleVM();

            vm.teachers = db.teachers.ToList();


            return View(vm);
        }

        [HttpPost]
        public ActionResult Edit_teacher(teacher teacher)
        {
            if (ModelState.IsValid)
            {
                db.Entry(teacher).State = EntityState.Modified;
                db.SaveChanges();
            }
            // ViewBag.ATID = new SelectList(db.AccountTypes, "ATID", "Description", teacher.ATID);
            //ViewBag.PId = new SelectList(db.Programs, "PId", "Program1", teacher.PId);
            return RedirectToAction("AddTeacher", "AAC");
        }


        [HttpPost]
        public ActionResult AddTeacher(teacher Teacher)
        {
            if (ModelState.IsValid)
            {

                db.teachers.Add(new teacher
                {
                    teachername = Teacher.teachername,
                    Email = Teacher.Email,
                    Password = Teacher.Password,
                    ATID = Teacher.ATID,
                    PId = Teacher.PId


                });
                db.SaveChanges();
            }
            else
            {
                return Json("NO", JsonRequestBehavior.AllowGet);
            }



            return RedirectToAction("AddTeacher", "AAC");
        }




        public ActionResult Current()
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

            var hour = today.Hour;

            ViewBag.today = today.Date.ToString("dd/MMMM");

            if (hour >= 0 && hour <= 10 || hour >= 22 && hour <= 23)
            {


                vm.currentclasses = db.Database.SqlQuery<CurrentClasses_Result>("CurrentClasses @dayid, @slottypeid, @currdate, @startdate, @enddate",
                            new SqlParameter("@dayid", currentDayOfWeek), new SqlParameter("@slottypeid", 1),
                            new SqlParameter("@currdate", today), new SqlParameter("@startdate", start), new SqlParameter("@enddate", end)).ToList();
            }
            else if (hour >= 11 && hour <= 14)
            {
                vm.currentclasses = db.Database.SqlQuery<CurrentClasses_Result>("CurrentClasses @dayid, @slottypeid, @currdate, @startdate, @enddate",
                            new SqlParameter("@dayid", currentDayOfWeek), new SqlParameter("@slottypeid", 2),
                            new SqlParameter("@currdate", today), new SqlParameter("@startdate", start), new SqlParameter("@enddate", end)).ToList();

            }
            else if (hour >= 15 && hour <= 17)
            {
                vm.currentclasses = db.Database.SqlQuery<CurrentClasses_Result>("CurrentClasses @dayid, @slottypeid, @currdate, @startdate, @enddate",
                            new SqlParameter("@dayid", currentDayOfWeek), new SqlParameter("@slottypeid", 3),
                            new SqlParameter("@currdate", today), new SqlParameter("@startdate", start), new SqlParameter("@enddate", end)).ToList();

            }
            else if (hour >= 18 && hour <= 21)
            {

                vm.currentclasses = db.Database.SqlQuery<CurrentClasses_Result>("CurrentClasses @dayid, @slottypeid, @currdate, @startdate, @enddate",
                            new SqlParameter("@dayid", currentDayOfWeek), new SqlParameter("@slottypeid", 4),
                            new SqlParameter("@currdate", today), new SqlParameter("@startdate", start), new SqlParameter("@enddate", end)).ToList();
            }

            return View(vm);
        }


        public ActionResult Empty()
        {

            //DateTime start = Convert.ToDateTime("9/5/2016");
            //DateTime end = Convert.ToDateTime("9/11/2016");

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
            ViewBag.dates = dates;

            ReportsVM vm = new ReportsVM();
            vm.emptyroomscount = db.Database.SqlQuery<EmptyRoomsCountByDay_Result>("EmptyRoomsCountByDay @startdate, @enddate",
                            new SqlParameter("@startdate", start), new SqlParameter("@enddate", end)).ToList();
            return View(vm);
        }

        public ActionResult Monitoring()
        {
            //var off = db.voffcourses.ToList();
            ////    var off = db.sections.Include(s=>s.)
            //var courses = off.Select(o => new
            //{
            //    Id = o.offid,
            //    Name = o.code + "-" + o.title + "-" + o.sectionname
            //}).ToArray();

            var off = from o in db.voffcourses
                      select new
                      {
                          Id = o.offid,
                          Name = o.code + "-" + o.title + "-" + o.sectionname
                      };

            ViewBag.offcourses = new SelectList(off, "Id", "Name");
            return View();
        }



        // GET: AAC
        public ActionResult Schedule()
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Index", "Login");

            }
            else
            {

                ViewBag.username = Session["Username"];
                CombineScheduleVM vm = new CombineScheduleVM();
                vm.days = db.days.ToList();
                vm.vslottypes = db.vslottypes.Where(o => o.occupied == 3).ToList();
                //vm.slottypes = db.slottypes.Where(st => st.slottype1 == 1).ToList();
                vm.rooms = db.rooms.Where(c => c.campus == "90").ToList().OrderBy(c => c.OrderCol);


                //vm.vschedule = 
                // db.vschedules.Where(a => a.dayid >= 1 && a.dayid <= 7 && a.campus == "90").ToList();
                //var current_date = DateTime.Now;
                //var current_week = current_date.AddDays(7);
                //DateTime myDate = DateTime.Parse(dateString);
                //string td = "9-19-2016";
                //string tq = "9-25-2016";

                //DateTime start = Convert.ToDateTime(td);
                //DateTime end = Convert.ToDateTime(tq);

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

                DateTime n = start.AddDays(7);
                //ViewBag.currentdate = monday.Date.ToString("dd/MMMM");
                ViewBag.currdate = today.Date.ToString("dd/MMMM");
                ViewBag.currtime = today.Hour;
                //ViewBag.currday = today.Date.ToString("ddd");

                ViewBag.dates = dates;

                vm.WeekWise = db.WeekWises.Where(w => w.Date >= start && w.Date <= end).ToList();

                vm.vschedule = (from a in db.vschedules
                                where ((a.startdate >= start && a.startdate <= end) ||
                (a.enddate >= start && a.enddate <= end) || (a.startdate <= start && a.enddate >= end)) && a.campus == "90"
                                orderby a.dayid, a.roomid, a.slottypeid, a.occupied
                                select a).ToList();


                //DateTime first = (FirstDateOfWeekISO8601(2016, 17)).AddDays(245);
                //DateTime second = (FirstDateOfWeekISO8601(2016, 17)).AddDays(251);
                return View(vm);
            }
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
    }
}