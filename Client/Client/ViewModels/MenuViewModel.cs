using System.Windows.Input;
using Client.Utils.Commands;
using Client.Utils.Mediators;
using Client.Utils.Navigation;

namespace Client.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        private readonly Mediator mediator;

        private ContentNavigation nav;
        
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

        public MenuViewModel(Mediator mediator, ContentNavigation nav)
        {
            this.mediator = mediator;
            this.nav = nav;
            filter = "";
        }
    }
}