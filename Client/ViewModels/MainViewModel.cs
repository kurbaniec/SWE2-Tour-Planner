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

        private TourListModelLegacy tourList;
        public TourListModelLegacy TourList => tourList;

        private WelcomeViewModel welcomeViewModel;
        public WelcomeViewModel WelcomeViewModel => welcomeViewModel;

        public MainViewModel(Mediator mediator, ContentNavigation nav)
        {
            this.mediator = mediator;
            this.nav = nav;
            //tourList = new TourListModelLegacy(this);
            //welcomeViewModel = new WelcomeViewModel(this);
            //currentPage = new Welcome {DataContext = welcomeViewModel};

            mediator.Register(o =>
            {
                var model = (TourViewModel) o;
                Console.WriteLine($"Selected {model.Name}");
            }, ViewModelMessages.SelectedTourChange);
        }
    }
}