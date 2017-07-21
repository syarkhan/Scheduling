using Scheduling.Models;
using Scheduling.Models.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scheduling.Controllers
{
    public class TeacherController : Controller
    {
        DBEntities db = new DBEntities();
        

        teacher teacher = new teacher();

        CombineScheduleVM vm = new CombineScheduleVM();
        // GET: Teacher
        public ActionResult PMDashboard()
        {
            //Hashtable htEmpInformaiton = (Hashtable)Session["EmpDetails"];
            //lblEmopName.Text = htEmpInformaiton["Name"].ToString();
            //lblDesignation.Text = htEmpInformaiton["Designation"].ToString();
            //SessionVM vm = new SessionVM();
            //var data = Session["Username"];

            if ((SessionVM)Session["Username"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                var data = (SessionVM)Session["Username"];

                //var pid = data.teachername;
                ViewBag.pid = data.PId;

                ViewBag.program = data.program;

                ViewBag.teachername = data.teachername;

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


                //Select c.Class from Programs p join section s on s.PID = p.PId join Class c on c.Id = s.ClassId where p.PId = 2 and STID = 1
                //group by c.Class

               


                vm.vschedule = (from a in db.vschedules
                                where ((a.startdate >= start && a.startdate <= end) ||
                                (a.enddate >= start && a.enddate <= end) || (a.startdate <= start && a.enddate >= end))
                                && a.flag == true
                                && a.teacherid == data.teacherid
                                && a.dayid == currentDayOfWeek
                                orderby a.slottypeid, a.secid, a.occupied
                                select a).ToList();
                
            }
            
            return View(vm);
        }



        public ActionResult TeacherDashboard()
        {
            //Hashtable htEmpInformaiton = (Hashtable)Session["EmpDetails"];
            //lblEmopName.Text = htEmpInformaiton["Name"].ToString();
            //lblDesignation.Text = htEmpInformaiton["Designation"].ToString();
            //SessionVM vm = new SessionVM();
            //var data = Session["Username"];

            if ((SessionVM)Session["Username"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                var data = (SessionVM)Session["Username"];

                //var pid = data.teachername;
                

                ViewBag.teachername = data.teachername;

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


                //Select c.Class from Programs p join section s on s.PID = p.PId join Class c on c.Id = s.ClassId where p.PId = 2 and STID = 1
                //group by c.Class




                vm.vschedule = (from a in db.vschedules
                                where ((a.startdate >= start && a.startdate <= end) ||
                                (a.enddate >= start && a.enddate <= end) || (a.startdate <= start && a.enddate >= end))
                                && a.flag == true
                                && a.teacherid == data.teacherid
                                && a.dayid == currentDayOfWeek
                                orderby a.slottypeid, a.secid, a.occupied
                                select a).ToList();

            }

            return View(vm);
        }

        public ActionResult ISchedule()
        {

            if ((SessionVM)Session["Username"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {

                var data = (SessionVM)Session["Username"];

                //var pid = data.teachername;
                var pid = data.PId;
                ViewBag.teachername = data.teachername;

                ViewBag.program = data.program;

                var teacherid = data.teacherid;
                ViewBag.teacherid = teacherid;


                vm.days = db.days.ToList();
                vm.vslottypes = db.vslottypes.Where(o => o.occupied == 3).ToList();

                vm.sections = db.sections.Where(a => a.PId == pid && a.STID == 1).ToList();
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

                ViewBag.dates = dates;



                //select* from vschedule where secid = 1 and slottypeid = 1 and dayid = 1


                var sections = from c in db.Classes
                               join
                               s in db.sections on c.Id equals s.ClassId
                               join
                               p in db.Programs on s.PId equals p.PId
                               where p.PId == pid && s.STID == 1
                               group c by c.Class1 into a
                               select new
                               {
                                   Id = a.FirstOrDefault().Id,
                                   Name = a.FirstOrDefault().Class1
                               };
                ViewBag.sections = new SelectList(sections, "Id", "Name");



                vm.vschedule = (from a in db.vschedules
                                where ((a.startdate >= start && a.startdate <= end) ||
                                (a.enddate >= start && a.enddate <= end) || (a.startdate <= start && a.enddate >= end))
                                && a.flag == true
                                && a.PId == pid
                                orderby a.dayid, a.slottypeid, a.secid, a.occupied
                                select a).ToList();
            }

            return View(vm);
        }

        
        public ActionResult PMRequest()
        {

            if ((SessionVM)Session["Username"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                var data = (SessionVM)Session["Username"];

                ViewBag.program = data.program;
                //var pid = data.teachername;
                ViewBag.pid = data.PId;

                ViewBag.teachername = data.teachername;

                var teacherid = data.teacherid;

                var slottypes = from c in db.slottypes
                                select new
                                {
                                    Id = c.slottypeid,
                                    Name = c.duration
                                };
                ViewBag.slottypes = new SelectList(slottypes, "Id", "Name");


                //var last = db.Requests.ToList().Last();
                return View(db.Requests.Where(s => s.teacherid == teacherid).ToList());
            }

            
        }



        public ActionResult TeacherRequest()
        {

            if ((SessionVM)Session["Username"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                var data = (SessionVM)Session["Username"];

                ViewBag.program = data.program;
                //var pid = data.teachername;
                ViewBag.pid = data.PId;

                ViewBag.teachername = data.teachername;

                var teacherid = data.teacherid;

                var slottypes = from c in db.slottypes
                                select new
                                {
                                    Id = c.slottypeid,
                                    Name = c.duration
                                };
                ViewBag.slottypes = new SelectList(slottypes, "Id", "Name");


                //var last = db.Requests.ToList().Last();
                return View(db.Requests.Where(s => s.teacherid == teacherid).ToList());
            }


        }
    }
}