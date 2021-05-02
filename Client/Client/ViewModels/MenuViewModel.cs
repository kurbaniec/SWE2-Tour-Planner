using System.Windows.Input;
using Client.Utils.Commands;
using Client.Utils.Mediators;
using Client.Utils.Navigation;

namespace Client.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        private readonly Mediator mediator;
        private readonly ContentNavigation nav;
        
        private string filter;
        public string Filter
        {
            get => filter;
            set
            {
                if (value == filter) return;
                filter = value;
                OnPropertyChanged();
                mediator.NotifyColleagues(ViewModelMessages.FilterChange, filter);
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

        private TourWrapper? selectedTour;
        public TourWrapper? SelectedTour
        {
            get => selectedTour;
            set
            {
                if (value == selectedTour || value == null) return;
                selectedTour = value;
                OnPropertyChanged();
            }
        }

        private bool busy;

        public bool Busy
        {
            get => busy;
            set
            {
                if (value == busy) return;
                busy = value;
                OnPropertyChanged();
            }
        }
        
        // Mediator events
        
        private void TransactionBegin(object o)
        {
            Busy = true;
        }

        private void TransactionEnd(object o)
        {
            Busy = false;
        }

        private void SelectedTourChange(object o)
        {
            var tour = (TourWrapper) o;
            SelectedTour = tour;
        }
        
        public MenuViewModel(Mediator mediator, ContentNavigation nav)
        {
            this.mediator = mediator;
            this.nav = nav;
            filter = "";
            mediator.Register(TransactionBegin, ViewModelMessages.TransactionBegin);
            mediator.Register(TransactionEnd, ViewModelMessages.TransactionEnd);
            mediator.Register(SelectedTourChange, ViewModelMessages.SelectedTourChange);
        }
    }
}