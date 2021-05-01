using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Client.Logic.BL;
using Client.Logic.Setup;
using Client.Utils.Commands;
using Client.Utils.Extensions;
using Client.Utils.Mediators;
using Client.Utils.Navigation;
using Model;
using Type = Model.Type;

namespace Client.ViewModels
{
    public class ListViewModel : BaseViewModel
    {
        private readonly TourPlannerClient tp;
        private readonly Mediator mediator;
        private readonly ContentNavigation nav;
        private readonly string baseUrl;

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
                            var (routeImg, _) 
                                = await tp.GetRouteImage(id);
                            
                            if (routeImg is { } && selectedTour is {} && selectedTour.Model.Id == id)
                            {
                                selectedTour.ImageLoaded = true;
                                selectedTour.Image = routeImg;
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
                    p => true,
                    async p =>
                    {
                        mediator.NotifyColleagues(ViewModelMessages.TransactionBegin, null!);
                        var response = await tp.GetTours();
                        if (response.Item1 is { } tours)
                        {
                            tours.ForEach(async t =>
                            {
                                var (routeImg, dummyImg) = await tp.GetRouteImage(t.Id);
                                TourWrapper tour = routeImg is { }
                                    ? new TourWrapper(t, routeImg)
                                    : new TourWrapper(t, dummyImg!, imageLoaded: false);
                                Tours.Add(tour);
                            });
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
            this.baseUrl = cfg.BaseUrl;

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
            mediator.Register(o => { Disabled = true; }, ViewModelMessages.TransactionBegin);
            mediator.Register(o => { Disabled = false; }, ViewModelMessages.TransactionEnd);
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
                // Navigate to new tour info page
                SelectedTour = newTour;
                nav.Navigate(ContentPage.AppInfo);
            }, ViewModelMessages.TourAddition);
        }
    }
}