using System.Windows.Controls;
using Client.Utils.Mediators;
using Client.Utils.Navigation;

namespace Client.ViewModels
{
    /// <summary>
    /// ViewModel for the <c>MainWindow</c> view.
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly Mediator mediator;

        // ReSharper disable once NotAccessedField.Local
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