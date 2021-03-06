using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using Client.Model;

namespace Client.ViewModel
{
    public class TourListModel : INotifyPropertyChanged
    {
        // Declare event
        // See: https://docs.microsoft.com/en-us/dotnet/desktop/wpf/data/how-to-implement-property-change-notification?view=netframeworkdesktop-4.8
        public event PropertyChangedEventHandler? PropertyChanged = null!;
        
        public ObservableCollection<TourViewModel> Tours { get; private set; }
        private readonly ICollectionView toursView;

        private string filter;
        public string Filter
        {
            get => filter;
            set
            {
                if (value == filter) return;
                filter = value;
                toursView.Refresh();
                OnPropertyChanged();
            }
        }

        public TourListModel()
        {
            // Initialize properties
            filter = "";
            Tours = new ObservableCollection<TourViewModel>();
            // Add Dummy Tours
            var tour = new TourViewModel("TourA");
            var tour2 = new TourViewModel("A long tour");
            Tours.Add(tour);
            Tours.Add(tour2);
            // Setup Filter
            // See: https://markheath.net/post/list-filtering-in-wpf-with-m-v-vm
            toursView = CollectionViewSource.GetDefaultView(Tours);
            toursView.Filter = o => string.IsNullOrEmpty(Filter) || 
                                    ((TourViewModel) o).Name.ToLower().Contains(filter.ToLower());

        }
        
        // Create the OnPropertyChanged method to raise the event
        // The calling member's name will be used as the parameter.
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        
    }
}