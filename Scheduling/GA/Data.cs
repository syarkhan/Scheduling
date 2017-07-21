using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Scheduling.Models;

namespace Scheduling.GA
{

    public class Data
    {
        private DBEntities db = new DBEntities();
        private List<offeredcourse> offeredCourses = new List<offeredcourse>();
        private List<slot> slots = new List<slot>();

        private int numberOfClasses = 0;


        public Data()
        {
            initialize();
        }


        private Data initialize()
        {

            // offcourses = db.offeredcourses.Where(o => o.section.PId == 2 && o.section.STID == 1).Take(50).ToList();

            var offcourses = db.offeredcourses.Where(o => o.section.PId == 2 && o.section.STID == 3).Take(50).ToList();

            //var offcourses = db.offeredcourses.Take(10).ToList();
            foreach (var off in offcourses)
            {
                //var offcourse = offcourses.ElementAt(0);
                offeredCourses.Add(off);
            }


            numberOfClasses = offeredCourses.Count;
            //offeredCourses.ForEach(x => numberOfClasses += x.offid);


            var slotss = db.slots.Where(s => s.dayid != 7 && s.room.campus == "100" && s.room.roomtype == "class").ToList();
            //var slotss = db.slots.Where(s => s.dayid != 7).ToList();
            foreach (var slot in slotss)
            {
                slots.Add(slot);
            }

            return this;
        }


        public virtual List<offeredcourse> OfferedCourses
        {
            get
            {
                return offeredCourses;
            }
        }

        public virtual List<slot> Slots
        {
            get
            {
                return slots;
            }
        }


        public virtual int NumberOfClasses
        {
            get
            {
                return this.numberOfClasses;
            }
        }
    }

}