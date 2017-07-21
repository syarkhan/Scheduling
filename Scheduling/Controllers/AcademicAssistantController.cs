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
    public class AcademicAssistantController : Controller
    {
        DBEntities db = new DBEntities();
        // GET: AcademicAssistant
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


        public ActionResult Teacher_List()
        {

            return View();

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