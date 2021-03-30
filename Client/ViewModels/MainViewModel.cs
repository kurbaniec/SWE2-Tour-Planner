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

        public MainViewModel(Mediator mediator, ContentNavigation nav)
        {
            this.mediator = mediator;
            this.nav = nav;

            mediator.Register(o =>
            {
                var model = (TourViewModel) o;
                Console.WriteLine($"Selected {model.Name}");
            }, ViewModelMessages.SelectedTourChange);
        }
    }
}