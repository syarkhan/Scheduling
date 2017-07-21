using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Scheduling.Domain;
using Scheduling.Models;

namespace Scheduling.GA
{


    public class NewSchedule
    {
        private List<NewClass> classes;
        private bool isFitnessChanged = true;
        private double fitness = -1;
        private int classNumb = 0;
        private int numbOfConflicts = 0;
        private Data data;


        public virtual Data Data
        {
            get
            {
                return data;
            }
        }
        public NewSchedule(Data data)
        {
            this.data = data;
            classes = new List<NewClass>(data.NumberOfClasses);
        }

        public virtual NewSchedule initialize()
        {
            (new List<offeredcourse>(data.OfferedCourses)).ForEach(off =>
            {

                NewClass newClass = new NewClass(classNumb++, off);
                newClass.Slot = data.Slots[(int)(data.Slots.Count * GlobalRandom.NextDouble)];


                //newClass.Teacher = course.Teachers.ElementAt((int)(course.Teachers.Count * GlobalRandom.NextDouble));
                classes.Add(newClass);

            });
            return this;
        }

        public virtual int NumbOfConflicts
        {
            get
            {
                return numbOfConflicts;
            }
        }
        public virtual List<NewClass> Classes
        {
            get
            {
                isFitnessChanged = true;
                return classes;
            }
        }
        public virtual double Fitness
        {
            get
            {
                if (isFitnessChanged == true)
                {
                    fitness = calculateFitness();
                    isFitnessChanged = false;
                }
                return fitness;
            }
        }


        private double calculateFitness()
        {
            numbOfConflicts = 0;

            classes.ForEach(x =>
            {
                var abc = classes.IndexOf(x);
                if (x.Slot.room.capacity < x.OfferedCourse.students_count)
                {
                    numbOfConflicts++;
                }

                foreach (var y in classes.Where(y => classes.IndexOf(y) >= classes.IndexOf(x)).ToList())
                {
                    if (x.Id != y.Id)
                    {
                        var abc2 = classes.IndexOf(y);
                        if (x.Slot.slotid == y.Slot.slotid) numbOfConflicts++;
                        if (x.OfferedCourse.secid == y.OfferedCourse.secid && x.Slot.dayid == y.Slot.dayid && x.Slot.slottypeid == y.Slot.slottypeid) numbOfConflicts++;
                        if (x.OfferedCourse.teacherid == y.OfferedCourse.teacherid && x.Slot.dayid == y.Slot.dayid && x.Slot.slottypeid == y.Slot.slottypeid) numbOfConflicts++;
                        //if (x.Slot.dayid == y.Slot.dayid && x.Slot.slottypeid == y.Slot.slottypeid) numbOfConflicts++;
                    }
                }

            });
            return 1 / (double)(numbOfConflicts + 1);
        }

        public override string ToString()
        {
            string returnValue = "";
            for (int x = 0; x < classes.Count - 1; x++)
            {
                returnValue += classes[x] + ",";
            }
            returnValue += classes[classes.Count - 1];
            return returnValue;
        }

    }

}