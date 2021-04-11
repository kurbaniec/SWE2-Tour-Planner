using System;
using System.Text.Json.Serialization;

namespace Model
{
    public class TourLog
    {
        public int Id { set; get; }
        public DateTime Date { set; get; } 
        public Type Type { set; get; }
        public TimeSpan Duration { set; get; }
        public int Distance { set; get; }
        public int Rating { set; get; }
        public string Report { set; get; }
        public double AvgSpeed { set; get; }
        public double MaxSpeed { set; get; }
        public double HeightDifference { set; get; }
        public int Stops { set; get; }

        public TourLog(DateTime date, Type type, TimeSpan duration, int distance, int rating, string report, double avgSpeed, double maxSpeed, double heightDifference, int stops)
        {
            Date = date;
            Type = type;
            Duration = duration;
            Distance = distance;
            Rating = rating;
            Report = report;
            AvgSpeed = avgSpeed;
            MaxSpeed = maxSpeed;
            HeightDifference = heightDifference;
            Stops = stops;
        }
        
        public TourLog(int id, DateTime date, Type type, TimeSpan duration, int distance, int rating, string report, double avgSpeed, double maxSpeed, double heightDifference, int stops)
        {
            Id = id;
            Date = date;
            Type = type;
            Duration = duration;
            Distance = distance;
            Rating = rating;
            Report = report;
            AvgSpeed = avgSpeed;
            MaxSpeed = maxSpeed;
            HeightDifference = heightDifference;
            Stops = stops;
        }
        
        public TourLog() {}
    }
}