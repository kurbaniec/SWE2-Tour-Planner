using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Client.Utils.Mediators;
using Client.Utils.Navigation;
using Client.Views;
using Client.ViewModels;

namespace Client.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly Mediator mediator;
        
        private readonly ContentNavigation nav;
        
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
            nav.Navigate(ContentPage.Layout);
        }
        
        public MainViewModel(Mediator mediator, ContentNavigation nav)
        {
            this.mediator = mediator;
            this.nav = nav;
            tourList = new TourListModel(this);
            welcomeViewModel = new WelcomeViewModel(this);
            currentPage = new Welcome {DataContext = welcomeViewModel};
        }
    }
}