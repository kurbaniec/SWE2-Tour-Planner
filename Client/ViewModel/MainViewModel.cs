using System;
using System.Windows.Controls;
using Client.View;

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

        public void NavigateSomeWhere()
        {
            var layout = new Layout();
            currentPage.NavigationService.Navigate(layout);
            Console.WriteLine(currentPage);
        }
        
        public MainViewModel()
        {
            tourList = new TourListModel(this);
            welcomeViewModel = new WelcomeViewModel(this);
            currentPage = new Welcome {DataContext = welcomeViewModel};
        }
    }
}