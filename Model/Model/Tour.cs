using System;
using System.Collections.Generic;

namespace Model
{
    public class Tour
    {
        public string Name { set; get;  }
        public int Distance { set; get; }
        public string Description { set; get; }
        public string Image { set; get; }
        public List<TourLog> Logs { set; get; }

        public Tour(string name, int distance, string description, string image, List<TourLog> logs)
        {
            Name = name;
            Distance = distance;
            Description = description;
            Image = image;
            Logs = logs;
        }

        public Tour(string name, int distance, string description, string image)
        {
            Name = name;
            Distance = distance;
            Description = description;
            Image = image;
            Logs = new List<TourLog>();
        }
    }
}