using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Windows.Data;
using System.Windows.Input;
using Client.Logic.BL;
using Client.Utils.Commands;
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
        
        public ObservableCollection<TourWrapper> Tours { get; }
        private readonly ICollectionView toursView;

        private TourWrapper? selectedTour;

        public TourWrapper? SelectedTour
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
                        Trace.WriteLine("Adding tour...");
                    });
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
                            tours.ForEach(t => Tours.Add(new TourWrapper(t)));
                        }
                        mediator.NotifyColleagues(ViewModelMessages.TransactionEnd, null!);
                    });
                return loadData;
            }
        }

        public ListViewModel(TourPlannerClient tp, Mediator mediator, ContentNavigation nav)
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
            mediator.Register(o =>
            {
                Disabled = true;
            }, ViewModelMessages.TransactionBegin);
            mediator.Register(o =>
            {
                Disabled = false;
            }, ViewModelMessages.TransactionEnd);
        }
    }
}