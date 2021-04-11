using System;
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
                "http://localhost:8080/api/route/1000",
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
                "http://localhost:8080/api/route/1001",
                new List<TourLog>()
            );
            tours.Add(tour);
            tours.Add(tour2);
        }
        
        public List<Tour> GetTours()
        {
            return tours;
        }

        public (Tour?, string) AddTour(Tour tour)
        {
            var id = tours.OrderByDescending(t => t.Id).Take(1).First().Id + 1;
            tour.Id = id;
            
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
                t.Image = tour.Image;
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

        // TODO delete this?
        
        public int? AddTourLog(int tourId, TourLog log)
        {
            var tour = tours.SingleOrDefault(t => t.Id == tourId);
            
            if (tour is null) return null;
            
            var id = tour.Logs.OrderByDescending(t => t.Id).Take(1).First().Id + 1;
            log.Id = id;
            tour.Logs.Add(log);
            return id;
        }

        public bool UpdateTourLog(int tourId, TourLog log)
        {
            var tour = tours.SingleOrDefault(t => t.Id == tourId);
            
            if (tour is null) return false;
            
            tour.Logs.ForEach(t =>
            {
                if (t.Id != tour.Id) return;
                t.Date = log.Date;
                t.Distance = log.Distance;
                t.Duration = log.Duration;
                t.Rating = log.Rating;
                t.Report = log.Report;
                t.Stops = log.Stops;
                t.Type = log.Type;
                t.AvgSpeed = log.AvgSpeed;
                t.HeightDifference = log.HeightDifference;
                t.MaxSpeed = log.MaxSpeed;
            });
            return true;
        }

        public bool DeleteTourLog(int tourId, TourLog log)
        {
            var tour = tours.SingleOrDefault(t => t.Id == tourId);

            if (tour is null) return false;
            
            tour.Logs = tour.Logs.Where(l => l.Id != log.Id).ToList();
            return true;
        }
    }
}