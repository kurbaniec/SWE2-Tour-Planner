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
        public int Distance { set; get; }
        public string Description { set; get; }
        public string Image { set; get; }
        public List<TourLog> Logs { set; get; }
        
        public Tour(string from, string to, string name, int distance, string description, string image,
            List<TourLog> logs)
        {
            From = from;
            To = to;
            Name = name;
            Distance = distance;
            Description = description;
            Image = image;
            Logs = logs;
        }

        public Tour(string from, string to, string name, int distance, string description, string image)
        {
            From = from;
            To = to;
            Name = name;
            Distance = distance;
            Description = description;
            Image = image;
            Logs = new List<TourLog>();
        }
        
        public Tour(int id, string @from, string to, string name, int distance, string description, string image, List<TourLog> logs)
        {
            Id = id;
            From = @from;
            To = to;
            Name = name;
            Distance = distance;
            Description = description;
            Image = image;
            Logs = logs;
        }
        
        public Tour() {}
    }
}