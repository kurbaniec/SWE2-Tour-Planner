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
            private set
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

        private ICommand? cancelEdit;
        public ICommand CancelEdit
        {
            get
            {
                if (cancelEdit != null) return cancelEdit;
                cancelEdit = new RelayCommand(
                    _ => !Busy,
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
                    _ => !Busy &&
                         (SelectedTour == null || SelectedTour.IsValid),
                    async _ =>
                    {
                        if (selectedTour is null) return;
                        Busy = true;
                        Edit = false;
                        mediator.NotifyColleagues(ViewModelMessages.TransactionBegin, null!);
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
                        Busy = false;
                        mediator.NotifyColleagues(ViewModelMessages.TransactionEnd, null!);
                    }
                );
                return acceptEdit;
            }
        }

        private ICommand? deleteTour;
        public ICommand DeleteTour
        {
            get
            {
                if (deleteTour != null) return deleteTour;
                deleteTour = new RelayCommand(
                    _ => !Busy,
                    async _ =>
                    {
                        if (selectedTour is null) return;
                        logger.Log(LogLevel.Information, 
                            $"Asking Users if Tour with id {selectedTour.Model.Id} should be really deleted");
                        var ok = nav.ShowInfoDialogWithQuestion(
                            "Do you really want to delete this Tour?\nThis process is not reversible.",
                            "Tour Planner - Delete Tour");
                        if (!ok) return;
                        Busy = true;
                        Edit = false;
                        mediator.NotifyColleagues(ViewModelMessages.TransactionBegin, null!);
                        logger.Log(LogLevel.Information, "Starting deletion process");
                        var (isDeleted, errorMsg) = await tp.DeleteTour(selectedTour.Model.Id);
                        if (isDeleted)
                        {
                            logger.Log(LogLevel.Information,
                                $"Deletion of Tour with id {selectedTour.Model.Id} was successful");
                            mediator.NotifyColleagues(ViewModelMessages.TourDeletion, selectedTour.Model.Id);
                            SelectedTour = null;
                            nav.Navigate(ContentPage.AppWelcome);
                            nav.ShowInfoDialog(
                                "Deletion of Tour was successful", 
                                "Tour Planner - Delete Tour");
                        }
                        else
                        {
                            logger.Log(LogLevel.Warning,
                                $"Deletion of Tour with id {selectedTour.Model.Id} was not successful");
                            nav.ShowErrorDialog(
                                $"Encountered error while deleting Tour: \n{errorMsg}", 
                                "Tour Planner - Delete Tour");
                        }
                        Busy = false;
                        mediator.NotifyColleagues(ViewModelMessages.TransactionEnd, null!);
                    }
                );
                return deleteTour;
            }
        }
        
        private ICommand? addLog;
        public ICommand AddLog
        {
            get
            {
                if (addLog != null) return addLog;
                addLog = new RelayCommand(
                    _ => !Busy, _ =>
                    {
                        if (SelectedTour is null) return;
                        Edit = true;
                        SelectedTour.AddNewLog();
                    }
                );
                return addLog;
            }
        }

        private void SelectedTourChange(object o)
        {
            selectedTour?.DiscardChanges();
            if (Edit) Edit = false;
            var tour = (TourWrapper) o;
            SelectedTour = tour;
        }

        public InfoViewModel(TourPlannerClient tp, Mediator mediator, ContentNavigation nav)
        {
            this.tp = tp;
            this.mediator = mediator;
            this.nav = nav;
            // Register to changes
            mediator.Register(SelectedTourChange, ViewModelMessages.SelectedTourChange);
        }
    }
}