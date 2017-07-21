using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Scheduling.GA;
using Scheduling.Models;
using Scheduling.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scheduling.Controllers
{
    public class AcademicOfficerController : Controller
    {
        DBEntities db = new DBEntities();

        CombineScheduleVM vm = new CombineScheduleVM();



        public ActionResult Index()
        {
            return View();
        }
        //public ActionResult Test()
        //{
        //    Dictionary<string, List<slot>> hSchedule = new Dictionary<string, List<slot>>();

        //    var rooms = db.rooms.ToList();
        //    var slottypes = db.vslottypes.Where(o => o.occupied == 3).ToList();
        //    var days = db.days.ToList();
        //    var courses = db.offeredcourses.OrderBy(a=>a.offid).ToList();

        //    List<slot> slots = (from a in db.slots orderby a.dayid, a.slottypeid, a.roomid select a).ToList();



        //    List<string> alSch1 = new List<string>();

        //    string key, pkey = null;

        //    foreach (var slot in slots)
        //    {
        //        key = slot.dayid + "-" + slot.slottypeid + "-" + slot.roomid;
        //        if (key.Equals(pkey))
        //        {
        //            alSch1.Add(slot);
        //        }
        //        else
        //        {
        //            alSch1 = new List<slot>();
        //            alSch1.Add(slot);
        //        }
        //        hSchedule[key] = alSch1;

        //        pkey = key;
        //    }
        //    var sc = hSchedule;
        //    //for ()
        //    return null;
        //}







        // GET: AcademicOfficer
        public ActionResult Schedule()
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Index", "Login");

            }
            else
            {
                ViewBag.username = Session["Username"];
                
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


        public ActionResult Requests()
        {

            if (Session["Username"] == null)
            {
                return RedirectToAction("Index", "Login");

            }
            else
            {
                return View(db.Requests.ToList());
            }
        }

        public PartialViewResult Algorithm()
        {
            
            Stopwatch sw = Stopwatch.StartNew();
            Driver driver = new Driver();
            driver.data = new Data();
            int generationNumber = 0;

            GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm(driver.data);
            Population population = (new Population(Driver.POPULATION_SIZE, driver.data)).sortByFitness();

            var list = population.Schedules.ToList();
            driver.classNumb = 1;

            while (population.Schedules[0].Fitness != 1.0)
            {
                Console.WriteLine("> Generation # " + ++generationNumber);
                //Console.Write("  Schedule # |                                           ");
                //Console.Write("Classes [dept,class,room,Teacher,meeting-time]       ");
                //Console.WriteLine("                                  | Fitness | Conflicts");
                //Console.Write("-----------------------------------------------------------------------------------");
                //Console.WriteLine("-------------------------------------------------------------------------------------");
                //population = geneticAlgorithm.evolve(population).sortByFitness();
                //driver.scheduleNumb = 0;
                //population.Schedules.ForEach(schedule => Console.WriteLine("       " + driver.scheduleNumb++ + "     | " + schedule + " | " + string.Format("{0:F5}", schedule.Fitness) + " | " + schedule.NumbOfConflicts));

                population = geneticAlgorithm.evolve(population).sortByFitness();
                driver.scheduleNumb = 0;

                if (population.Schedules[0].Fitness == 1.0)
                {
                    vm.NewClasses = population.Schedules[0].Classes.ToList();
                    ViewBag.generations = generationNumber;
                    
                    //driver.printScheduleAsTable(population.Schedules[0], generationNumber);
                }

                //vm.days = db.days.ToList();
                //vm.vslottypes = db.vslottypes.Where(o => o.occupied == 3).ToList();
                //vm.rooms = db.rooms.Where(c => c.campus == "90").ToList().OrderBy(c => c.OrderCol);
                //vm.slots = db.slots.Where(s=> s.room.campus == "90").ToList();
                //driver.printScheduleAsTable(population.Schedules[0], generationNumber);
                driver.classNumb = 1;
            }


            sw.Stop();
            var minutes = sw.Elapsed.TotalMinutes;
            var seconds = sw.Elapsed.TotalSeconds;
            ViewBag.minutes = minutes;
            ViewBag.seconds = seconds;
            return PartialView("Algo_Partial",vm);
        }


        [HttpPost]
        public ActionResult Algorithm(int[] slotid, int[] offid)
        {

            //db.schedules.Find(schid).EndDate = enddate;

            

            DateTime startdate = new DateTime(2017,9,5);
            DateTime enddate = new DateTime(2018, 1, 9);

            
            //var sch = db.schedules.First(s => s.StartDate == startdate && s.EndDate == enddate);

            //db.schedules.Remove(sch);

            //db.SaveChanges();

            db.Semesters.Where(s => s.flag == true).Single().flag = false;
            db.Semesters.Add(new Semester
            {
                STID = 3,
                flag = true,
                SemesterStartDate = startdate,
                SemesterEndDate = enddate,
                Year = "2017"
            });

            db.SaveChanges();

            var sid = db.Semesters.Where(s => s.flag == true).Single().SId;

            foreach (var item in offid.Zip(slotid , (a , b) => new { A = a, B = b }))
            {
                var off = item.A;
                var slot = item.B;
                db.schedules.Add(new schedule
                {
                    offid = off,
                    slotid = slot,
                    occupied = 3,
                    StartDate = startdate,
                    EndDate = enddate,
                    SId = sid


                });
            }
            db.SaveChanges();

            return RedirectToAction("Schedule", "AcademicOfficer");
        }


        public ActionResult AddRoomCapacity()
        {

            CombineScheduleVM vm = new CombineScheduleVM();
                
                vm.dOffCourses = ListByCampus();
                vm.slots = db.slots.ToList();
            
            //Random rnd = new Random();
            // 1 <= month < 13

            //foreach (room room in db.rooms)
            //{
            //    int classcap = rnd.Next(30, 55);
            //    int labcap = rnd.Next(40, 60);

            //    if (room.roomtype == "class")
            //    {
            //        room.capacity = classcap;
            //    }
            //    else if (room.roomtype == "lab")
            //    {
            //        room.capacity = labcap;
            //    } 

            //}
            //foreach (offeredcourse off in db.offeredcourses)
            //{
            //    int classcap = rnd.Next(30, 50);
            //    int labcap = rnd.Next(40, 60);


            //    off.students_count = classcap;

            //}

            ////db.schedules.Find(schid).teacherid = teacherid;

            //db.SaveChanges();
            return View(vm);
        }


        public Dictionary<int, voffCoursesWithSectionsandTeacher> ListByCampus()
        {
            //Select s.secid,p.Program + '-' + c.Class + ' ' + s.sectionname as 'SectionName' from section s join 
            //Programs p on s.PID = p.PId join Class c on c.Id = s.ClassId
            //where STID = 1
            //List<Dictionary<string, int>> hSchedule = new List<Dictionary<string, int>>();

            Dictionary<int, voffCoursesWithSectionsandTeacher> dOffCourses = new Dictionary<int, voffCoursesWithSectionsandTeacher>();
                
            //Dictionary<string,int> dict = new Dictionary<string, int> {
            //     { "key1", 1 },
            //        { "key2", 2 } };

            //hSchedule.Add(dict);

            List<int> offids = new List<int>();
            Dictionary<int,slot> secids = new Dictionary<int, slot>();

            foreach (var slot in db.slots)
            {
                foreach (var section in db.voffCoursesWithSectionsandTeachers)
                {
                    if (slot.room.capacity + 4 > section.StudentCount)
                    {
                        if (offids.Contains(section.offid))
                        {
                            continue;
                        }
                        else
                        {
                            if (secids.ContainsKey(section.secid))
                            {
                                continue;
                            }
                            else
                            {
                                dOffCourses[slot.slotid] = section;
                                offids.Add(section.offid);
                                secids[section.secid] = slot;
                            }

                        }
                        
                    }
                    else
                    {
                        //list.Add(slot.slotid);

                    }
                    break;
                }
                

            }

            //Dictionary<string, vschedule> hSchedule1 = new Dictionary<string, vschedule>();

            //string key, pkey = null;

            //List<vschedule> alSch = (from a in db.vschedules where a.campus == "90" orderby a.dayid, a.roomid, a.slottypeid, a.occupied select a).ToList();

            //List<vschedule> alSch1 = new List<vschedule>();
            //foreach (var sch in alSch)
            //{
            //    key = sch.dayid.ToString() + "-" + sch.roomid.ToString() + "-" + sch.slottypeid.ToString();
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
            return dOffCourses;
        }

        public Dictionary<string, List<vschedule>> ListByCampus1()
        {

            Dictionary<string, List<vschedule>> hSchedule = new Dictionary<string, List<vschedule>>();

            string key, pkey = null;

            List<vschedule> alSch = (from a in db.vschedules where a.campus == "90" orderby a.dayid, a.roomid, a.slottypeid, a.occupied select a).ToList();

            List<vschedule> alSch1 = new List<vschedule>();
            foreach (var sch in alSch)
            {
                key = sch.dayid.ToString() + "-" + sch.roomid.ToString() + "-" + sch.slottypeid.ToString();
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

        public ActionResult Excel()
        {

            return View();

        }

        [HttpPost]
        public ActionResult Excel(ExcelVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            DataTable dt = GetDataTableFromSpreadsheet(model.MyExcelFile.InputStream, false);
            string strContent = "<p>Thanks for uploading the file</p>" + ConvertDataTableToHTMLTable(dt);
            model.MSExcelTable = strContent;
            return View(model);
        }
        public static DataTable GetDataTableFromSpreadsheet(Stream MyExcelStream, bool ReadOnly)
        {
            DataTable dt = new DataTable();
            using (SpreadsheetDocument sDoc = SpreadsheetDocument.Open(MyExcelStream, ReadOnly))
            {
                WorkbookPart workbookPart = sDoc.WorkbookPart;
                IEnumerable<Sheet> sheets = sDoc.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                string relationshipId = sheets.First().Id.Value;
                WorksheetPart worksheetPart = (WorksheetPart)sDoc.WorkbookPart.GetPartById(relationshipId);
                Worksheet workSheet = worksheetPart.Worksheet;
                SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                IEnumerable<Row> rows = sheetData.Descendants<Row>();

                foreach (Cell cell in rows.ElementAt(0))
                {
                    dt.Columns.Add(GetCellValue(sDoc, cell));
                }

                foreach (Row row in rows) //this will also include your header row...
                {
                    DataRow tempRow = dt.NewRow();

                    for (int i = 0; i < row.Descendants<Cell>().Count(); i++)
                    {
                        tempRow[i] = GetCellValue(sDoc, row.Descendants<Cell>().ElementAt(i));
                    }

                    dt.Rows.Add(tempRow);
                }
            }
            dt.Rows.RemoveAt(0);
            return dt;
        }
        public static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            string value = cell.CellValue.InnerXml;

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
            }
            else
            {
                return value;
            }
        }
        public static string ConvertDataTableToHTMLTable(DataTable dt)
        {
            string ret = "";
            ret = "<table id=" + (char)34 + "tblExcel" + (char)34 + ">";
            ret += "<tr>";
            foreach (DataColumn col in dt.Columns)
            {
                ret += "<td class=" + (char)34 + "tdColumnHeader" + (char)34 + ">" + col.ColumnName + "</td>";
            }
            ret += "</tr>";
            foreach (DataRow row in dt.Rows)
            {
                ret += "<tr>";
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    ret += "<td class=" + (char)34 + "tdCellData" + (char)34 + ">" + row[i].ToString() + "</td>";
                }
                ret += "</tr>";
            }
            ret += "</table>";
            return ret;
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