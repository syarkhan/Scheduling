using Scheduling.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scheduling.Models.ViewModels
{
    
    public class CombineScheduleVM
    {

        DBEntities db = new DBEntities();

        public IEnumerable<NewClass> NewClasses { get; set; }

        public IEnumerable<room> rooms { get; set; }
        public IEnumerable<slottype> slottypes { get; set; }
        public IEnumerable<day> days { get; set; }
        // public IEnumerable<section> sections { get; set; }
        public IEnumerable<vschedule> vschedule { get; set; }

        public IEnumerable<vslottype> vslottypes { get; set; }

        public IEnumerable<section> sections { get; set; }

        public IEnumerable<WeekWise> WeekWise { get; set; }

        public List<teacher> teachers { get; set; }

        public List<slot> slots { get; set; }

        public int schid { get; set; }
        public int offid { get; set; }
        
        public int slotid { get; set; }
        public Nullable<int> slotno { get; set; }
        public int dayid { get; set; }
        public int slottypeid { get; set; }

        

        public int CurrentWeek { get; set; }
        public IEnumerable<SelectListItem> TotalWeeks { get; set; }

        public Dictionary<string, List<vschedule>> hSchedule { get; set; }

        public Dictionary<int, voffCoursesWithSectionsandTeacher> dOffCourses = new Dictionary<int, voffCoursesWithSectionsandTeacher>();



        public Dictionary<string, List<vschedule>> ListByCampus()
        {

            Dictionary<string, List<vschedule>> hSchedule = new Dictionary<string, List<vschedule>>();
          
            string key, pkey = null;

            List<vschedule> alSch = (from a in db.vschedules where a.campus=="90" orderby a.dayid, a.roomid, a.slottypeid, a.occupied select a ).ToList(); 
            
            List<vschedule> alSch1 = new List<vschedule>();
            foreach(var sch in alSch)
            {
                key = sch.dayid.ToString()+"-"+sch.roomid.ToString()+"-"+sch.slottypeid.ToString();
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


      








        public Dictionary<string, List<vschedule>> ListByFilter(int durid, int dayid, string campus)
        {

            Dictionary<string, List<vschedule>> hSchedule = new Dictionary<string, List<vschedule>>();

            string key, pkey = null;

            List<vschedule> alSch = (from a in db.vschedules where a.dayid == dayid && a.slottypeid == durid && a.campus == campus orderby a.roomid, a.occupied select a).ToList();

            List<vschedule> alSch1 = new List<vschedule>();
            foreach (var sch in alSch)
            {
                key = sch.roomid.ToString();
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







        public Dictionary<string, List<vschedule>> Filter(int durid, int dayid, string campus)
        {

            Dictionary<string, List<vschedule>> hSchedule = new Dictionary<string, List<vschedule>>();

            string key, pkey = null;

            List<vschedule> alSch = (from a in db.vschedules where a.dayid == dayid && a.slottypeid == durid && a.campus == campus orderby a.roomid, a.occupied select a).ToList();

            List<vschedule> alSch1 = new List<vschedule>();
            foreach (var sch in alSch)
            {
                key = sch.roomid.ToString();
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





        public Dictionary<string, List<vschedule>> ListByCampus(int slottypeid,int dayid)
        {

            Dictionary<string, List<vschedule>> hSchedule = new Dictionary<string, List<vschedule>>();

            string key, pkey = null;

            List<vschedule> alSch = (from a in db.vschedules where a.dayid == dayid && a.slottypeid == slottypeid && a.campus == "90" orderby a.roomid, a.occupied select a).ToList();

            List<vschedule> alSch1 = new List<vschedule>();
            foreach (var sch in alSch)
            {
                key = sch.roomid.ToString();
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

        public List<vschedule> ShowDataByRoom(int dayid,int durid,int roomid)
        {
            List<vschedule> schedule = new List<vschedule>();

            schedule = (from a in db.vschedules where a.dayid == dayid && a.slottypeid == durid && a.roomid == roomid orderby a.roomid, a.occupied select a).ToList();

            return schedule;
        }


    }
}