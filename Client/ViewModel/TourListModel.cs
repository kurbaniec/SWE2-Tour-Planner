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
        // Declare the event
        public event PropertyChangedEventHandler PropertyChanged;
        
        // Update lists in WPF
        // See: https://markheath.net/post/list-filtering-in-wpf-with-m-v-vm

        public ObservableCollection<TourViewModel> Tours { get; private set; }
        private ICollectionView toursView;
        
        
        
        
        private string filter;

        public string Filter
        {
            get
            {
                return filter;
            }
            set
            {
                if (value != filter)
                {
                    filter = value;
                    toursView.Refresh();
                    OnPropertyChanged();
                }
            }
        }

        public TourListModel()
        {
            filter = "";
            Tours = new ObservableCollection<TourViewModel>();
            var tour = new TourViewModel("TourA");
            Tours.Add(tour);
            toursView = CollectionViewSource.GetDefaultView(Tours);
            toursView.Filter = o => string.IsNullOrEmpty(Filter) || ((string)o).Contains(Filter);

        }
        
        // Create the OnPropertyChanged method to raise the event
        // The calling member's name will be used as the parameter.
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        
    }
}