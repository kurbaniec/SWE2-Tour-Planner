using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Client.View;

namespace Client.ViewModel
{
    public class MainViewModel : BaseViewModel
    {

        // Bind navigation service
        // See: https://stackoverflow.com/a/52459022/12347616
        // Frame is the name of the `Frame`-Element
        private static NavigationService navigation 
            = ((Application.Current.MainWindow as MainWindow)!).Frame.NavigationService;
        
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
            //var layout = new Layout();
            //navigation.Navigate(new Uri("View/Layout.xaml", UriKind.RelativeOrAbsolute));
            navigation.Navigate(new Layout());
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