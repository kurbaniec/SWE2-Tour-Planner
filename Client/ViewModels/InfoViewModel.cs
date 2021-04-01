using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Client.Utils.Commands;
using Client.Utils.Mediators;
using Client.Utils.Navigation;

namespace Client.ViewModels
{
    public class InfoViewModel : BaseViewModel
    {
        private readonly Mediator mediator;

        private readonly ContentNavigation nav;

        private TourViewModel? selectedTour;

        public TourViewModel? SelectedTour
        {
            get => selectedTour;
            set
            {
                if (value == selectedTour || value == null) return;
                selectedTour = value;
                OnPropertyChanged();
            }
        }

        private bool edit;

        public bool Edit
        {
            get => edit;
            set
            {
                if (value == edit) return;
                edit = value;
                OnPropertyChanged();
            }
        }

        private ICommand? changeEditMode;

        public ICommand ChangeEditMode
        {
            get
            {
                if (changeEditMode != null) return changeEditMode;
                changeEditMode = new RelayCommand(
                    p => true,
                    p => Edit = !Edit
                );
                return changeEditMode;
            }
        }

        private bool waitingForResponse;

        public bool WaitingForResponse
        {
            get => waitingForResponse;
            set
            {
                if (value == waitingForResponse) return;
                waitingForResponse = value;
                OnPropertyChanged();
            }
        }

        private ICommand? cancelEdit;

        public ICommand CancelEdit
        {
            get
            {
                if (cancelEdit != null) return cancelEdit;
                cancelEdit = new RelayCommand(
                    p => !WaitingForResponse,
                    p =>
                    {
                        selectedTour?.DiscardChanges();
                        Edit = false;
                    }
                );
                return cancelEdit;
            }
        }

        private ICommand? acceptEdit;

        public ICommand AcceptEdit
        {
            get
            {
                if (acceptEdit != null) return acceptEdit;
                acceptEdit = new RelayCommand(
                    p => !WaitingForResponse,
                    async (p) =>
                    {
                        WaitingForResponse = true;
                        await Task.Run(() =>
                        {
                            Thread.Sleep(5000);
                        });
                        selectedTour?.SaveChanges();
                        WaitingForResponse = false;
                        Edit = false;
                    }
                );
                return acceptEdit;
            }
        }


        public InfoViewModel(Mediator mediator, ContentNavigation nav)
        {
            this.mediator = mediator;
            this.nav = nav;
            // Register to changes
            mediator.Register(o =>
            {
                var tour = (TourViewModel) o;
                SelectedTour = tour;
                Console.WriteLine("Baum");
            }, ViewModelMessages.SelectedTourChange);
        }
    }
}