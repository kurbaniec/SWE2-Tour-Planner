using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Client.Logic.BL;
using Client.Utils.Commands;
using Client.Utils.Mediators;
using Client.Utils.Navigation;
using Model;

namespace Client.ViewModels
{
    public class AddViewModel : BaseViewModel
    {
        private readonly TourPlannerClient tp;
        private readonly Mediator mediator;
        private readonly ContentNavigation nav;
        private TourWrapper tour = new TourWrapper(new Tour(), string.Empty);

        public TourWrapper Tour
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
                    _ => true,
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

        private ICommand? addTour;

        public ICommand AddTour
        {
            get
            {
                if (addTour != null) return addTour;
                addTour = new RelayCommand(
                    _ => !WaitingForResponse && Tour.IsValid,
                    async _ =>
                    {
                        WaitingForResponse = true;
                        Edit = false;
                        mediator.NotifyColleagues(ViewModelMessages.TransactionBegin, null!);
                        var (responseTour, errorMessage) = await tp.AddTour(tour.GetRequestTour());
                        if (responseTour is { } newTour)
                        {
                            tour = new TourWrapper(new Tour(), string.Empty);
                            mediator.NotifyColleagues(ViewModelMessages.TourAddition, newTour);
                        }
                        else
                        {
                            nav.ShowErrorDialog(
                                $"Encountered error while adding tour: \n{errorMessage}",
                                "Tour Planner - Add Tour");
                        }

                        WaitingForResponse = false;
                        Edit = true;
                        mediator.NotifyColleagues(ViewModelMessages.TransactionEnd, null!);
                    }
                );
                return addTour;
            }
        }


        public AddViewModel(TourPlannerClient tp, Mediator mediator, ContentNavigation nav)
        {
            this.tp = tp;
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