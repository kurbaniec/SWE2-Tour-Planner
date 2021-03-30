using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using Client.Utils.Commands;
using Client.Utils.Mediators;
using Client.Utils.Navigation;

namespace Client.ViewModels
{
    public class ListViewModel : BaseViewModel
    {
        private readonly Mediator mediator;
        
        private readonly ContentNavigation nav;
        
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
                mediator.NotifyColleagues(ViewModelMessages.SelectedTourChange, selectedTour);
                nav.Navigate(ContentPage.AppInfo);
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
                        //addDialog = new AppAddTour {DataContext = this};
                        //addDialog.ShowDialog();
                        // TODO
                        Console.WriteLine("Adding tour...");
                    });
                return openAddTour;
            }
        }

        public ListViewModel(Mediator mediator, ContentNavigation nav)
        {
            this.mediator = mediator;
            this.nav = nav;

            mediator.Register(o =>
            {
                if (selectedTour != null)
                    mediator.NotifyColleagues(ViewModelMessages.SelectedTourChange, selectedTour);
            }, ViewModelMessages.GetSelectedTour);

            filter = "";
            
            Tours = new ObservableCollection<TourViewModel>();
            selectedTour = null;
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
    }
}