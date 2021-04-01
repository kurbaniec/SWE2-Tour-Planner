﻿using System;
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
            // Request latest Tour
            // Only needed the first time
            mediator.NotifyColleagues(ViewModelMessages.GetSelectedTour, null!);
        }
    }
}