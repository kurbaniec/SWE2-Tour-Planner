using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Client.Utils.Commands;
using Client.Utils.Mediators;
using Client.Utils.Navigation;
using Model;

namespace Client.ViewModels
{
    public class AddViewModel : BaseViewModel
    {
        private readonly Mediator mediator;

        private readonly ContentNavigation nav;

        private TourWrapper? tour = new TourWrapper(new Tour(), string.Empty);

        public TourWrapper? Tour
        {
            get => tour;
            set
            {
                if (value == tour) return;
                tour = value;
                OnPropertyChanged();
            }
        }

        private bool edit = true;

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
                        tour?.DiscardChanges();
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
                    p => !WaitingForResponse && 
                         (Tour == null || Tour.IsValid),
                    async (p) =>
                    {
                        WaitingForResponse = true;
                        Edit = false;
                        mediator.NotifyColleagues(ViewModelMessages.TransactionBegin, null!);
                        await Task.Run(() =>
                        {
                            Thread.Sleep(50);
                        });
                        tour?.SaveChanges();
                        WaitingForResponse = false;
                        mediator.NotifyColleagues(ViewModelMessages.TransactionEnd, null!);
                    }
                );
                return acceptEdit;
            }
        }

        private ICommand? addLog;

        public ICommand AddLog
        {
            get
            {
                if (addLog != null) return addLog;
                addLog = new RelayCommand(
                    p => !WaitingForResponse,
                    async (p) =>
                    {
                        if (Tour is null) return;
                        Edit = true;
                        Tour.AddNewLog();
                    }
                );
                return addLog;
            }
        }


        public AddViewModel(Mediator mediator, ContentNavigation nav)
        {
            this.mediator = mediator;
            this.nav = nav;
            // Register to changes
            /*
            mediator.Register(o =>
            {
                this.tour?.DiscardChanges();
                if (Edit) 
                    Edit = false;
                var tour = (TourWrapper) o;
                Tour = tour;
            }, ViewModelMessages.SelectedTourChange);*/
        }
    }
}