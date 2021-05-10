using System.Windows.Controls;
using Client.Utils.Mediators;
using Client.Utils.Navigation;

namespace Client.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly Mediator mediator;

        private readonly ContentNavigation nav;

        private Page? currentPage;

        public Page? CurrentPage
        {
            get => currentPage;
            set
            {
                currentPage = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel(Mediator mediator, ContentNavigation nav)
        {
            this.mediator = mediator;
            this.nav = nav;
        }
    }
}