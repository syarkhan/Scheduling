using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Scheduling.Models;
using Scheduling.Models.ViewModels;

namespace Scheduling.Controllers
{
    public class AndroidApiController : ApiController
    {
        DBEntities db = new DBEntities();

        //[NonSerialized]
        [HttpGet]
        public IEnumerable<StudentsApi> getStudents(int username)
        {

            //JSONObject
            List<StudentsApi> students = new List<StudentsApi>();
            StudentsApi student = new StudentsApi();

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

            var student_name = db.Students.Where(s => s.StudentId == username).Select(a => new { a.StudentName, a.secid }).Single();

            var data = (from a in db.vschedules
                        join
                        s in db.Students on a.secid equals s.secid
                        where ((a.startdate >= start && a.startdate <= end) ||
                        (a.enddate >= start && a.enddate <= end) || (a.startdate <= start && a.enddate >= end))
                        && a.flag == true
                        && a.secid == student_name.secid
                        && s.StudentId == username
                        orderby a.secid, a.slottypeid, a.occupied
                        select new { a, s }).ToList();

            DateTime date = new DateTime();

            foreach (var item in data)
            {
                foreach (var d in dates)
                {
                    if ((int)d.DayOfWeek == item.a.dayid)
                    {
                        date = d.Date;
                    }
                }
                students.Add(new StudentsApi
                {

                    StudentName = item.s.StudentName,
                    sectionname = item.a.sectionname,
                    course = item.a.title,
                    teacher = item.a.teachername,
                    roomno = item.a.roomno,
                    campus = item.a.campus,
                    //dayname = DateTime.Now.StartOfWeek(item.a.dayid).ToString("MM/dd/yyyy"),
                    dayname = date.ToString("dddd-dd/MMMM"),
                    duration = item.a.duration,
                    dateOrder = date


                });
            }
            return students.ToArray().OrderBy(a=>a.dateOrder);


        }

        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "hello","hi" };
        //}
    }
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, int startOfWeek)
        {
            int diff = Convert.ToInt16(dt.DayOfWeek) - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            return dt.AddDays(-1 * diff).Date;
        }
    }
}
