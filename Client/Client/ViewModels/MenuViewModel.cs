using System.Windows.Input;
using Client.Logic.BL;
using Client.Utils.Commands;
using Client.Utils.Mediators;
using Client.Utils.Navigation;

namespace Client.ViewModels
{
    /// <summary>
    /// ViewModel for the <c>AppMenu</c> view.
    /// </summary>
    public class MenuViewModel : BaseViewModel
    {
        private readonly TourPlannerClient tp;
        private readonly Mediator mediator;
        private readonly ContentNavigation nav;

        private string filter;

        public string Filter
        {
            get => filter;
            set
            {
                if (value == filter) return;
                filter = value;
                OnPropertyChanged();
                mediator.NotifyColleagues(ViewModelMessages.FilterChange, filter);
            }
        }

        private ICommand? clearFilter;

        public ICommand ClearFilter
        {
            get
            {
                if (clearFilter != null) return clearFilter;
                clearFilter = new RelayCommand(
                    _ => !string.IsNullOrEmpty(filter),
                    // Filter instead of filter is used to trigger
                    // Filter's set function
                    _ => Filter = ""
                );
                return clearFilter;
            }
        }

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

        private ICommand? import;

        public ICommand Import
        {
            get
            {
                // Command starts Tour import process
                if (import != null) return import;
                import = new RelayCommand(
                    _ => true,
                    _ =>
                    {
                        var path = nav.ShowOpenFileDialog("Tour Data (*.td)|*.td");
                        if (path is { })
                        {
                            mediator.NotifyColleagues(ViewModelMessages.Import, path);
                        }
                    }
                );
                return import;
            }
        }

        private ICommand? exportThis;

        public ICommand ExportThis
        {
            get
            {
                // Command starts Tour export process for one Tour
                if (exportThis != null) return exportThis;
                exportThis = new RelayCommand(
                    _ => true,
                    _ =>
                    {
                        var path = nav.ShowSaveFileDialog("Tour Data (*.td)|*.td");
                        if (path is { })
                        {
                            mediator.NotifyColleagues(ViewModelMessages.ExportThis, path);
                        }
                    }
                );
                return exportThis;
            }
        }

        private ICommand? exportAll;

        public ICommand ExportAll
        {
            get
            {
                // Command starts Tour export process for all Tours
                if (exportAll != null) return exportAll;
                exportAll = new RelayCommand(
                    _ => true,
                    _ =>
                    {
                        var path = nav.ShowSaveFileDialog("Tour Data (*.td)|*.td");
                        if (path is { })
                        {
                            mediator.NotifyColleagues(ViewModelMessages.ExportAll, path);
                        }
                    }
                );
                return exportAll;
            }
        }

        private ICommand? print;

        public ICommand Print
        {
            get
            {
                // Command starts Tour print process
                if (print != null) return print;
                print = new RelayCommand(
                    _ => true,
                    async p =>
                    {
                        var isSummary = (bool) (p ?? false);
                        var path = nav.ShowSaveFileDialog("Pdf (*.pdf)|*.pdf");
                        if (path is null) return;
                        if (selectedTour is null) return;
                        var (ok, errorMsg) = await tp.Print(path, selectedTour.Model.Id, isSummary);
                        if (ok)
                            nav.ShowInfoDialog("Print of Tour was successful", "Tour Planner - Print");
                        else
                            nav.ShowErrorDialog($"Encountered error while printing Tour: \n{errorMsg}",
                                "Tour Planner - Print");
                    }
                );
                return print;
            }
        }

        private ICommand? showHelp;

        public ICommand ShowHelp
        {
            get
            {
                // Command should invoke the display of a help page in the default browser
                if (showHelp != null) return showHelp;
                showHelp = new RelayCommand(
                    _ => true,
                    _ => nav.ShowHelpPage()
                );
                return showHelp;
            }
        }

        private ICommand? copyTour;

        public ICommand CopyTour
        {
            get
            {
                // Command starts Tour copy process
                if (copyTour != null) return copyTour;
                copyTour = new RelayCommand(
                    _ => true,
                    _ => mediator.NotifyColleagues(ViewModelMessages.TourCopy, null)
                );
                return copyTour;
            }
        }


        // Mediator events

        /// <summary>
        /// Triggered when app controls should be disabled.
        /// </summary>
        /// <param name="o"></param>
        private void TransactionBegin(object? o)
        {
            Busy = true;
        }

        /// <summary>
        /// Triggered when app control should be enabled again.
        /// </summary>
        /// <param name="o"></param>
        private void TransactionEnd(object? o)
        {
            Busy = false;
        }

        /// <summary>
        /// Changes the current selected Tour.
        /// </summary>
        /// <param name="o">
        /// The new tour.
        /// </param>
        private void SelectedTourChange(object? o)
        {
            var tour = (TourWrapper?) o;
            SelectedTour = tour;
        }

        public MenuViewModel(TourPlannerClient tp, Mediator mediator, ContentNavigation nav)
        {
            this.tp = tp;
            this.mediator = mediator;
            this.nav = nav;
            filter = "";
            mediator.Register(TransactionBegin, ViewModelMessages.TransactionBegin);
            mediator.Register(TransactionEnd, ViewModelMessages.TransactionEnd);
            mediator.Register(SelectedTourChange, ViewModelMessages.SelectedTourChange);
        }
    }
}