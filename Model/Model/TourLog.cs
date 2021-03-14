using System;

namespace Model
{
    public class TourLog
    {
        public Type Type { set; get; }
        public DateTime Duration { set; get; }
        public int Distance { set; get; }
        public int Rating { set; get; }
        public string Report { set; get; }
        public double AvgSpeed { set; get; }
        public double MaxSpeed { set; get; }
        public double HeightDifference { set; get; }
        public int Stops { set; get; }

        public TourLog(Type type, DateTime duration, int distance, int rating, string report, double avgSpeed, double maxSpeed, double heightDifference, int stops)
        {
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
    }
}