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
        private TourWrapper tour = new(new Tour(), null!);

        public TourWrapper Tour
        {
            get => tour;
            private set
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

        private bool busy;
        public bool Busy
        {
            get => busy;
            set
            {
                if (value == busy) return;
                busy = value;
                OnPropertyChanged();
            }
        }

        private ICommand? addTour;
        public ICommand AddTour
        {
            get
            {
                if (addTour != null) return addTour;
                addTour = new RelayCommand(
                    _ => !Busy && Tour.IsValid,
                    async _ =>
                    {
                        Busy = true;
                        Edit = false;
                        mediator.NotifyColleagues(ViewModelMessages.TransactionBegin, null!);
                        var (responseTour, errorMessage) = await tp.AddTour(tour.GetRequestTour());
                        if (responseTour is { } newTour)
                        {
                            Tour = new TourWrapper(new Tour(), null!);
                            mediator.NotifyColleagues(ViewModelMessages.TourAddition, newTour);
                            nav.ShowInfoDialog(
                                "Tour Addition was successful",
                                "Tour Planner - Add Tour");
                        }
                        else
                        {
                            nav.ShowErrorDialog(
                                $"Encountered error while adding Tour: \n{errorMessage}",
                                "Tour Planner - Add Tour");
                        }
                        Busy = false;
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
        }
    }
}