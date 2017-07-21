using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scheduling.GA
{
    public class Driver
    {
        public const int POPULATION_SIZE = 3;
        public const double MUTATION_RATE = 0.015;
        public const double CROSSOVER_RATE = 0.9;
        public const int TOURNAMENT_SELECTION_SIZE = 3;
        public const int NUMB_OF_ELITE_SCHEDULES = 1;
        public int scheduleNumb = 0;
        public int classNumb = 1;
        public Data data;
    }
}