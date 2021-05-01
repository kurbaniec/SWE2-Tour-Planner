using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Client.Logic.BL;
using Client.Logic.Setup;
using Client.Utils.Commands;
using Client.Utils.Logging;
using Client.Utils.Mediators;
using Client.Utils.Navigation;
using Microsoft.Extensions.Logging;
using Model;

namespace Client.ViewModels
{
    public class ListViewModel : BaseViewModel
    {
        private readonly TourPlannerClient tp;
        private readonly Mediator mediator;
        private readonly ContentNavigation nav;
        private readonly ILogger logger = ApplicationLogging.CreateLogger<ListViewModel>();

        public ObservableCollection<TourWrapper> Tours { get; private set; }
        private readonly ICollectionView toursView;

        private TourWrapper? selectedTour;

        public TourWrapper? SelectedTour
        {
            get => selectedTour;
            set
            {
                if (value == selectedTour || value == null) return;
                selectedTour = value;
                // Load route information image if not already done
                // Based on https://stackoverflow.com/a/6613751/12347616
                // And: https://stackoverflow.com/a/23443359/12347616
                if (!selectedTour.ImageLoaded)
                {
                    Application.Current.Dispatcher.InvokeAsync(async () =>
                    {
                        if (SelectedTour != null)
                        {
                            var id = SelectedTour.Model.Id;
                            logger.Log(LogLevel.Information, 
                                $"Requesting route information image for Tour with id {id}");
                            var (routeImg, _) 
                                = await tp.GetRouteImage(id);
                            
                            if (routeImg is { } && selectedTour is {} && selectedTour.Model.Id == id)
                            {
                                selectedTour.ImageLoaded = true;
                                selectedTour.Image = routeImg;
                                logger.Log(LogLevel.Information, 
                                    "Route information successfully loaded");
                            }
                            else
                            {
                                logger.Log(LogLevel.Warning, "Route information not successfully loaded");
                                logger.Log(LogLevel.Warning, 
                                    $"Is Route Image null: {routeImg == null}");
                                logger.Log(LogLevel.Warning, 
                                    $"Is Selected Tour null: {selectedTour == null}");
                                logger.Log(LogLevel.Warning, 
                                    $"Is id mismatch: {selectedTour != null && id == selectedTour.Model.Id}");
                            }
                        }
                    });
                }
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
                    _ => true,
                    _ => nav.Navigate(ContentPage.AppAdd)
                );
                return openAddTour;
            }
        }

        private bool disabled;

        public bool Disabled
        {
            get => disabled;
            set
            {
                if (disabled == value) return;
                disabled = value;
                OnPropertyChanged();
            }
        }

        private ICommand? loadData;

        public ICommand LoadData
        {
            get
            {
                if (loadData != null) return loadData;
                loadData = new RelayCommand(
                    _ => true,
                    async _ =>
                    {
                        logger.Log(LogLevel.Information, "Trying to request all Tour data");
                        mediator.NotifyColleagues(ViewModelMessages.TransactionBegin, null!);
                        var success = false;
                        while (!success)
                        {
                            var (tourValues, errorMsg) = await tp.GetTours();
                            if (tourValues is { } tours)
                            {
                                tours.ForEach(async t =>
                                {
                                    var (routeImg, dummyImg) = await tp.GetRouteImage(t.Id);
                                    TourWrapper tour = routeImg is { }
                                        ? new TourWrapper(t, routeImg)
                                        : new TourWrapper(t, dummyImg!, imageLoaded: false);
                                    Tours.Add(tour);
                                });
                                success = true;
                                logger.Log(LogLevel.Information, "Tour data successfully fetched");
                            }
                            else
                            {
                                logger.Log(LogLevel.Warning, "Could not fetch Tour data");
                                nav.ShowErrorDialog(errorMsg, "Tour Planner");
                            }
                        }
                        mediator.NotifyColleagues(ViewModelMessages.TransactionEnd, null!);
                    });
                return loadData;
            }
        }

        public ListViewModel(
            TourPlannerClient tp, Mediator mediator, ContentNavigation nav, Configuration cfg)
        {
            this.tp = tp;
            this.mediator = mediator;
            this.nav = nav;

            filter = "";
            Tours = new ObservableCollection<TourWrapper>();
            selectedTour = null;

            // Setup Filter
            // See: https://markheath.net/post/list-filtering-in-wpf-with-m-v-vm
            toursView = CollectionViewSource.GetDefaultView(Tours);
            toursView.Filter = o => string.IsNullOrEmpty(Filter) ||
                                    ((TourWrapper) o).Name.ToLower().Contains(Filter.ToLower());

            mediator.Register(o =>
            {
                var newFilter = (string) o;
                Filter = newFilter;
            }, ViewModelMessages.FilterChange);
            mediator.Register(_ => { Disabled = true; }, ViewModelMessages.TransactionBegin);
            mediator.Register(_ => { Disabled = false; }, ViewModelMessages.TransactionEnd);
            mediator.Register(async o =>
            {
                // Create new Wrapper
                var tour = (Tour) o;
                // Fetch Route Information
                var (routeImg, dummyImg) = await tp.GetRouteImage(tour.Id);
                TourWrapper newTour = routeImg is { }
                    ? new TourWrapper(tour, routeImg)
                    : new TourWrapper(tour, dummyImg!, imageLoaded: false);
                // Update Tour List
                // Why Remove + Add? Reference to toursView should not change
                if (Tours.FirstOrDefault(t => t.Model.Id == newTour.Model.Id) is { } existingTour)
                    Tours.Remove(existingTour);
                Tours.Add(newTour);
                logger.Log(LogLevel.Information, $"Successfully added or updated Tour with id {newTour.Model.Id}");
                // Navigate to new tour info page
                SelectedTour = newTour;
                nav.Navigate(ContentPage.AppInfo);
            }, ViewModelMessages.TourAddition);
        }
    }
}