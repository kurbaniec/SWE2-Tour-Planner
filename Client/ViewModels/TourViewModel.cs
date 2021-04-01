using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Model;

namespace Client.ViewModels
{
    public class TourViewModel : BaseViewModel
    {
        private readonly Tour tour;

        private string from;
        public string From
        {
            get => from;
            set
            {
                if (from == value) return;
                from = value;
                OnPropertyChanged();
            }
        }

        private string to;
        public string To
        {
            get => to;
            set
            {
                if (to == value) return;
                to = value;
                OnPropertyChanged();
            }
        }

        private string name;
        public string Name
        {
            get => name;
            set
            {
                if (name == value) return;
                name = value;
                OnPropertyChanged();
            }
        }

        public int distance;
        public int Distance
        {
            get => distance;
            set
            {
                if (distance == value) return;
                distance = value;
                OnPropertyChanged();
            }
        }

        public string description;
        public string Description
        {
            get => description;
            set
            {
                if (description == value) return;
                description = value;
                OnPropertyChanged();
            }
        }

        public string image;
        public string Image
        {
            get => image;
            set
            {
                if (image == value) return;
                image = value;
                OnPropertyChanged();
            }
        }

        // TODO reset logs?
        public List<TourLog> Logs
        {
            get => tour.Logs;
            set
            {
                if (tour.Logs == value) return;
                tour.Logs = value;
                OnPropertyChanged();
            }
        }

        public TourViewModel(string from, string to, string name, int distance, string description, string image)
        {
            tour = new Tour(from, to, name, distance, description, image);
            this.from = tour.From;
            this.to = tour.To;
            this.name = tour.Name;
            this.distance = tour.Distance;
            this.description = tour.Description;
            this.image = tour.Image;
        }

        public TourViewModel(string from, string to, string name, int distance, string description, string image, List<TourLog> logs)
        {
            tour = new Tour(from, to, name, distance, description, image, logs);
            this.from = tour.From;
            this.to = tour.To;
            this.name = tour.Name;
            this.distance = tour.Distance;
            this.description = tour.Description;
            this.image = tour.Image;
        }

        public Tour GetRequestTour()
        {
            return new Tour(from, to, name, distance, description, image, Logs);
        }
        
        public void SaveChanges()
        {
            tour.From = from;
            tour.To = to;
            tour.Name = name;
            tour.Distance = distance;
            tour.Description = description;
            tour.Image = image;
        }

        public void DiscardChanges()
        {
            From = tour.From;
            To = tour.To;
            Name = tour.Name;
            Distance = tour.Distance;
            Description = tour.Description;
            Image = tour.Image;
        }
    }
}