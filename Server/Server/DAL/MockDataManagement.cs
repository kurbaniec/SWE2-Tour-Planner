﻿using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using WebService_Lib.Attributes;
using Type = Model.Type;

namespace Server.DAL
{
    // TODO remove later 
    [Component]
    public class MockDataManagement : IDataManagement
    {
        private List<Tour> tours;

        public MockDataManagement()
        {
            tours = new List<Tour>();
            var tour = new Tour(
                1000,
                "A", "B",
                "TourA", 
                22, 
                "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.",
                new List<TourLog>() {
                        new TourLog(
                            DateTime.Now, 
                            Type.Car,
                            TimeSpan.Zero, 
                            100,
                            10,
                            "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.",
                            55.0,
                            60,
                            100,
                            2
                        ), new TourLog(
                            DateTime.Now, 
                            Type.Car,
                            TimeSpan.Zero, 
                            100,
                            10,
                            "Great",
                            55.0,
                            60,
                            100,
                            2
                    )}
            );
            var tour2 = new Tour(
                1001,
                "U6", "U4",
                "TourB", 
                202, 
                "Better Tour Than A",
                new List<TourLog>()
            );
            tours.Add(tour);
            tours.Add(tour2);
        }
        
        public List<Tour> GetTours()
        {
            return tours;
        }

        public Tour? GetTour(int id)
        {
            return tours.FirstOrDefault(t => t.Id == id);
        }

        public (Tour?, string) AddTour(Tour tour)
        {
            if (tours.Count > 0)
            {
                var id = tours.OrderByDescending(t => t.Id).Take(1).First().Id + 1;
                tour.Id = id;
            }
            else
            {
                tour.Id = 1000;
            }

            var logHighest = tour.Logs.OrderByDescending(l => l.Id).Take(1).FirstOrDefault();
            var logId = logHighest?.Id ?? 1000;
            tour.Logs.ForEach(l =>
            {
                if (l.Id == 0) l.Id = logId++;
            });
                
            tours.Add(tour);
            return (tour, string.Empty);
        }

        public (Tour?, string) UpdateTour(Tour tour)
        {
            tours.ForEach(t =>
            {
                if (t.Id != tour.Id) return;
                t.Description = tour.Description;
                t.Distance = tour.Distance;
                t.From = tour.From;
                t.To = tour.To;
                t.Logs = tour.Logs;
                t.Name = tour.Name;
                t.Logs = tour.Logs;
                // Index new logs with id
                var logHighest = tour.Logs.OrderByDescending(l => l.Id).Take(1).FirstOrDefault();
                var logId = logHighest?.Id ?? 1000;
                t.Logs.ForEach(l =>
                {
                    if (l.Id == 0) l.Id = logId++;
                });
            });
            return (tour, string.Empty);
        }

        public (bool, string) DeleteTour(int id)
        {
            tours = tours.Where(tour => tour.Id != id).ToList();
            return (true, string.Empty);
        }
        
    }
}