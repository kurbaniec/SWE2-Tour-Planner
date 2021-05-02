using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Client.Logic.BL;
using Client.Utils.Commands;
using Client.Utils.Logging;
using Client.Utils.Mediators;
using Client.Utils.Navigation;
using Microsoft.Extensions.Logging;

namespace Client.ViewModels
{
    public class InfoViewModel : BaseViewModel
    {
        private readonly TourPlannerClient tp;
        private readonly Mediator mediator;
        private readonly ContentNavigation nav;
        private readonly ILogger logger = ApplicationLogging.CreateLogger<InfoViewModel>();

        private TourWrapper? selectedTour;
        public TourWrapper? SelectedTour
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
                    _ => true,
                    _ => Edit = !Edit
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
                    _ => !WaitingForResponse,
                    _ =>
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
                    _ => !WaitingForResponse &&
                         (SelectedTour == null || SelectedTour.IsValid),
                    async _ =>
                    {
                        WaitingForResponse = true;
                        Edit = false;
                        mediator.NotifyColleagues(ViewModelMessages.TransactionBegin, null!);
                        if (selectedTour is { })
                        {
                            logger.Log(LogLevel.Information, 
                                $"Trying to update Tour with id {selectedTour.Model.Id}");
                            var (updatedTour, errorMsg) = await tp.UpdateTour(selectedTour.GetRequestTour());
                            if (updatedTour is { })
                            {
                                logger.Log(LogLevel.Information,
                                    $"Update for Tour with id {selectedTour.Model.Id} was successful");
                                mediator.NotifyColleagues(ViewModelMessages.TourAddition, updatedTour);
                                nav.ShowInfoDialog("Update was successful", "Tour Planer - Update Tour");
                            }
                            else
                            {
                                logger.Log(LogLevel.Warning,
                                    $"Update for Tour with id {selectedTour.Model.Id} was not successful");
                                nav.ShowErrorDialog(
                                    $"Encountered error while updating Tour: \n{errorMsg}", 
                                    "Tour Planner - Update Tour");
                            }
                        }
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
                    _ => !WaitingForResponse, _ =>
                    {
                        if (SelectedTour is null) return;
                        Edit = true;
                        SelectedTour.AddNewLog();
                    }
                );
                return addLog;
            }
        }


        public InfoViewModel(TourPlannerClient tp, Mediator mediator, ContentNavigation nav)
        {
            this.tp = tp;
            this.mediator = mediator;
            this.nav = nav;
            // Register to changes
            mediator.Register(o =>
            {
                selectedTour?.DiscardChanges();
                if (Edit)
                    Edit = false;
                var tour = (TourWrapper) o;
                SelectedTour = tour;
            }, ViewModelMessages.SelectedTourChange);
        }
    }
}