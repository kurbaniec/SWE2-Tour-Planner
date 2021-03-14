using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Input;
using Client.Command;
using Client.View;

namespace Client.ViewModel
{
    public class TourListModel : INotifyPropertyChanged
    {
        // Declare event
        // See: https://docs.microsoft.com/en-us/dotnet/desktop/wpf/data/how-to-implement-property-change-notification?view=netframeworkdesktop-4.8
        public event PropertyChangedEventHandler? PropertyChanged;
        
        public ObservableCollection<TourViewModel> Tours { get; }
        private readonly ICollectionView toursView;

        private TourViewModel? selectedTour;

        public TourViewModel? SelectedTour
        {
            get => selectedTour;
            set
            {
                if (value == selectedTour || value == null) return;
                selectedTour = value;
                OnPropertyChanged();
            }
        }

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

        private ICommand? clearFilter;
        public ICommand ClearFilter
        {
            get
            {
                if (clearFilter != null) return clearFilter;
                clearFilter = new RelayCommand(
                    p => !string.IsNullOrEmpty(filter),
                    // Filter instead of filter is used to trigger
                    // Filter's set function
                    p => Filter = "" 
                );
                return clearFilter;
            }
        }

        private AppAddTour? addDialog;
        private ICommand? openAddTour;
        public ICommand? OpenAddTour
        {
            get
            {
                if (openAddTour != null) return openAddTour;
                openAddTour = new RelayCommand(
                    p => true,
                    p =>
                    {
                        // Set DataContext to this
                        // See: https://stackoverflow.com/a/26426111/12347616
                        addDialog = new AppAddTour {DataContext = this};
                        addDialog.ShowDialog();
                    });
                return openAddTour;
            }
        }

        private ICommand? addTour;
        public ICommand AddTour
        {
            get
            {
                if (addTour != null) return addTour;
                addTour = new RelayCommand(
                   p => true,
                   p =>
                   {
                       if (addDialog is not { } dialog) return;
                       // TODO call API
                       var name = addDialog.TourName.Text;
                       // Dummy data
                       var tour = new TourViewModel(
                           name, 
                           100, 
                           "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.",
                           "https://upload.wikimedia.org/wikipedia/commons/thumb/6/63/A_large_blank_world_map_with_oceans_marked_in_blue.svg/4500px-A_large_blank_world_map_with_oceans_marked_in_blue.svg.png"
                       );
                       Tours.Add(tour);
                       Console.WriteLine($"Added tour {name}");
                       // Close Dialog properly
                       // See: https://stackoverflow.com/a/41325121/12347616
                       addDialog.DialogResult = true;

                   });
                return addTour;
            }
        }

        public TourListModel()
        {
            // Initialize properties
            filter = "";
            Tours = new ObservableCollection<TourViewModel>();
            selectedTour = null;
            addDialog = null;
            // Add Dummy Tours
            var tour = new TourViewModel(
                "TourA", 
                22, 
                "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.",
                "https://upload.wikimedia.org/wikipedia/commons/thumb/6/63/A_large_blank_world_map_with_oceans_marked_in_blue.svg/4500px-A_large_blank_world_map_with_oceans_marked_in_blue.svg.png"
            );
            var tour2 = new TourViewModel(
                "TourB", 
                202, 
                "Better Tour Than A",
                "https://homepage.univie.ac.at/horst.prillinger/ubahn/m/largemap-s-wien.png"
            );
            Tours.Add(tour);
            Tours.Add(tour2);
            // Setup Filter
            // See: https://markheath.net/post/list-filtering-in-wpf-with-m-v-vm
            toursView = CollectionViewSource.GetDefaultView(Tours);
            toursView.Filter = o => string.IsNullOrEmpty(Filter) || 
                                    ((TourViewModel) o).Name.ToLower().Contains(Filter.ToLower());

        }
        
        // Create the OnPropertyChanged method to raise the event
        // The calling member's name will be used as the parameter.
        private void OnPropertyChanged([CallerMemberName] string name = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        
    }
}