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
                "A", "B",
                "TourA", 
                22, 
                "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.",
                "https://upload.wikimedia.org/wikipedia/commons/thumb/6/63/A_large_blank_world_map_with_oceans_marked_in_blue.svg/4500px-A_large_blank_world_map_with_oceans_marked_in_blue.svg.png",
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
                "U6", "U4",
                "TourB", 
                202, 
                "Better Tour Than A",
                "https://homepage.univie.ac.at/horst.prillinger/ubahn/m/largemap-s-wien.png"
            );
            tours.Add(tour);
            tours.Add(tour2);
        }
        
        public List<Tour> GetTours()
        {
            return tours;
        }

        public int? AddTour(Tour tour)
        {
            var id = tours.OrderByDescending(t => t.Id).Take(1).First().Id + 1;
            tour.Id = id;
            return id;
        }

        public bool UpdateTour(Tour tour)
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
            });
            return true;
        }

        public bool DeleteTour(int id)
        {
            tours = tours.Where(tour => tour.Id != id).ToList();
            return true;
        }

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