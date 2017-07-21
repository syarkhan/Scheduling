using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Scheduling.Models;

namespace Scheduling.Domain
{
    public class NewClass
    {

        private int id;
        private offeredcourse offeredcourse;
        private slot slot;


        public NewClass(int id, offeredcourse offeredcourse)
        {
            this.id = id;
            //this.course = course;
            this.offeredcourse = offeredcourse;

        }

        public virtual int Id
        {
            get
            {
                return id;
            }
        }

        public virtual slot Slot
        {
            set
            {
                this.slot = value;
            }
            get
            {
                return slot;
            }
        }

        public virtual offeredcourse OfferedCourse
        {
            set
            {
                this.offeredcourse = value;
            }
            get
            {
                return offeredcourse;
            }
        }

        public override string ToString()
        {
            //return "[" + dept.Name + "," + OfferedCourse.Course.courseid + "," + room.RoomNumber + "," + OfferedCourse.Teacher.teacherid + "," + meetingTime.Id + "]";

            return "[" + offeredcourse.course.title + "," + slot.slotid + " ," + slot.room.roomno + " ," + offeredcourse.teacher.teachername + "]";

        }

    }
}