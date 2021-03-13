using System.ComponentModel;
using System.Runtime.CompilerServices;
using Client.Model;

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

        public TourViewModel(string name)
        {
            tour = new Tour(name);
        }
        
        // Create the OnPropertyChanged method to raise the event
        // The calling member's name will be used as the parameter.
        private void OnPropertyChanged([CallerMemberName] string name = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}