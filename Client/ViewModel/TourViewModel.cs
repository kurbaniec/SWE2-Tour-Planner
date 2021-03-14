using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Model;

namespace Client.ViewModel
{
    public class TourViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        
        private Tour tour;

        public string Name
        {
            get => tour.Name;
            set
            {
                if (tour.Name == value) return;
                tour.Name = value;
                OnPropertyChanged();
            }
        }

        public int Distance
        {
            get => tour.Distance;
            set
            {
                if (tour.Distance == value) return;
                tour.Distance = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => tour.Description;
            set
            {
                if (tour.Description == value) return;
                tour.Description = value;
                OnPropertyChanged();
            }
        }

        public string Image
        {
            get => tour.Image;
            set
            {
                if (tour.Image == value) return;
                tour.Image = value;
                OnPropertyChanged();
            }
        }

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

        public TourViewModel(string name, int distance, string description, string image)
        {
            tour = new Tour(name, distance, description, image);
        }
        
        // Create the OnPropertyChanged method to raise the event
        // The calling member's name will be used as the parameter.
        private void OnPropertyChanged([CallerMemberName] string name = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}