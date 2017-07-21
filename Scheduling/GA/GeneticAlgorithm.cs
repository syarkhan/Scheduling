using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Scheduling.Models;
using Scheduling.Domain;

namespace Scheduling.GA
{
    public class GeneticAlgorithm
    {
        private Data data;
        public GeneticAlgorithm(Data data)
        {
            this.data = data;
        }
        public virtual Population evolve(Population population)
        {
            return mutatePopulation(crossoverPopulation(population));
        }
        internal virtual Population crossoverPopulation(Population population)
        {
            Population crossoverPopulation = new Population(population.Schedules.Count, data);
            Enumerable.Range(0, Driver.NUMB_OF_ELITE_SCHEDULES).ToList().ForEach(x => crossoverPopulation.Schedules[x] = population.Schedules[x]);
            Enumerable.Range(Driver.NUMB_OF_ELITE_SCHEDULES, population.Schedules.Count - 1).ToList().ForEach(x =>
            {
                if (Driver.CROSSOVER_RATE > GlobalRandom.NextDouble)
                {
                    NewSchedule schedule1 = selectTournamentPopulation(population).sortByFitness().Schedules[0];
                    NewSchedule schedule2 = selectTournamentPopulation(population).sortByFitness().Schedules[0];
                    crossoverPopulation.Schedules[x] = crossoverSchedule(schedule1, schedule2);
                }
                else
                {
                    crossoverPopulation.Schedules[x] = population.Schedules[x];
                }
            });
            return crossoverPopulation;
        }
        internal virtual NewSchedule crossoverSchedule(NewSchedule schedule1, NewSchedule schedule2)
        {
            NewSchedule crossoverSchedule = (new NewSchedule(data)).initialize();
            Enumerable.Range(0, crossoverSchedule.Classes.Count).ToList().ForEach(x =>
            {
                if (GlobalRandom.NextDouble > 0.5)
                {
                    crossoverSchedule.Classes[x] = schedule1.Classes[x];
                }
                else
                {
                    crossoverSchedule.Classes[x] = schedule2.Classes[x];
                }
            });
            return crossoverSchedule;
        }
        internal virtual Population mutatePopulation(Population population)
        {
            Population mutatePopulation = new Population(population.Schedules.Count, data);
            List<NewSchedule> schedules = mutatePopulation.Schedules;
            Enumerable.Range(0, Driver.NUMB_OF_ELITE_SCHEDULES).ToList().ForEach(x => schedules[x] = population.Schedules[x]);
            Enumerable.Range(Driver.NUMB_OF_ELITE_SCHEDULES, population.Schedules.Count - 1).ToList().ForEach(x =>
            {
                schedules[x] = mutateSchedule(population.Schedules[x]);
            });
            return mutatePopulation;
        }
        internal virtual NewSchedule mutateSchedule(NewSchedule mutateSchedule)
        {
            NewSchedule schedule = (new NewSchedule(data)).initialize();
            Enumerable.Range(0, mutateSchedule.Classes.Count).ToList().ForEach(x =>
            {
                if (Driver.MUTATION_RATE > GlobalRandom.NextDouble)
                {
                    mutateSchedule.Classes[x] = schedule.Classes[x];
                }
            });
            return mutateSchedule;
        }
        internal virtual Population selectTournamentPopulation(Population population)
        {
            Population tournamentPopulation = new Population(Driver.TOURNAMENT_SELECTION_SIZE, data);
            Enumerable.Range(0, Driver.TOURNAMENT_SELECTION_SIZE).ToList().ForEach(x =>
            {
                tournamentPopulation.Schedules[x] = population.Schedules[(int)(GlobalRandom.NextDouble * population.Schedules.Count)];
            });
            return tournamentPopulation;
        }
    }


}