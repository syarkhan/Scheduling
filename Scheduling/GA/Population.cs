using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Scheduling.Models;

namespace Scheduling.GA
{

    public class Population
    {
        private List<NewSchedule> schedules;
        public Population(int size, Data data)
        {
            schedules = new List<NewSchedule>(size);
            Enumerable.Range(0, size).ToList().ForEach(x => schedules.Add((new NewSchedule(data)).initialize()));
        }
        public virtual List<NewSchedule> Schedules
        {
            get
            {
                return this.schedules;
            }
        }
        public virtual Population sortByFitness()
        {
            schedules.Sort((schedule1, schedule2) =>
            {
                int returnValue = 0;
                if (schedule1.Fitness > schedule2.Fitness)
                {
                    returnValue = -1;
                }
                else if (schedule1.Fitness < schedule2.Fitness)
                {
                    returnValue = 1;
                }
                return returnValue;
            });
            return this;
        }
    }

}