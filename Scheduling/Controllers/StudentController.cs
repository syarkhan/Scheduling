using Scheduling.Models;
using Scheduling.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scheduling.Controllers
{
    public class StudentController : Controller
    {
        DBEntities db = new DBEntities();
        

        CombineScheduleVM vm = new CombineScheduleVM();

        // GET: Student
        public ActionResult Dashboard()
        {
            if ((SessionVM)Session["Username"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                var data = (SessionVM)Session["Username"];

                var pid = data.PId;
                //ViewBag.pid = data.PId;

                ViewBag.program = data.program+" "+data.Class+"-"+data.sectionname;

                ViewBag.studentname = data.studentname;

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
                                && a.PId == pid
                                && a.dayid == currentDayOfWeek
                                orderby a.secid, a.slottypeid, a.occupied
                                select a)
                                .ToList();

            }

            return View(vm);
        }



        public ActionResult Python()
        {
            var i = run_cmd("-i E:/BSCS/add.py");

            return View();

        }

        private string run_cmd(string cmd)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = @"C:/Users/Sheryar Khan/Anaconda3/python.exe";
            start.CreateNoWindow = true;
            start.Arguments = string.Format("{0}", cmd);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    //Console.Write(result);
                    process.WaitForExit();
                    return result;
                }
            }
        }
    }
}