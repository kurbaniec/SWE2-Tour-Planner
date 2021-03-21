using System.Windows.Controls;

namespace Client.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private Page currentPage;
        public Page CurrentPage
        {
            get => currentPage;
            set
            {
                currentPage = value;
                OnPropertyChanged();
            }
        }

        private TourListModel tourList;
        public TourListModel TourList => tourList;

        private WelcomeViewModel welcomeViewModel;
        public WelcomeViewModel WelcomeViewModel => welcomeViewModel;
        
        public MainViewModel()
        {
            tourList = new TourListModel();
            welcomeViewModel = new WelcomeViewModel();
        }
    }
}