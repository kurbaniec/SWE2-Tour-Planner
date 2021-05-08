using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Model
{
    public class Tour
    {
        public int Id { get; set; }
        public string From { set; get; }
        public string To { set; get; }
        public string Name { set; get; }
        public double Distance { set; get; }
        public string Description { set; get; }
        public List<TourLog> Logs { set; get; }
        
        public Tour(string from, string to, string name, double distance, string description,
            List<TourLog> logs)
        {
            From = from;
            To = to;
            Name = name;
            Distance = distance;
            Description = description;
            Logs = logs;
        }

        public Tour(string from, string to, string name, double distance, string description)
        {
            From = from;
            To = to;
            Name = name;
            Distance = distance;
            Description = description;
            Logs = new List<TourLog>();
        }
        
        public Tour(int id, string @from, string to, string name, double distance, string description, List<TourLog> logs)
        {
            Id = id;
            From = @from;
            To = to;
            Name = name;
            Distance = distance;
            Description = description;
            Logs = logs;
        }

        public Tour()
        {
            From = string.Empty;
            To = string.Empty;
            Name = string.Empty;
            Description = string.Empty;
            Logs = new List<TourLog>();
        }
    }
}