﻿using System.Windows.Input;
using Client.Utils.Commands;

namespace Client.ViewModels
{
    public class WelcomeViewModel : BaseViewModel
    {
        private readonly MainViewModel callback;
        
        private ICommand? navigate;
        public ICommand Navigate
        {
            get
            {
                if (navigate != null) return navigate;
                navigate = new RelayCommand(
                    p => true, o => {}
                    // Filter instead of filter is used to trigger
                    // Filter's set function
                    //p => callback.NavigateSomeWhere()
                );
                return navigate;
            }
        }

        public WelcomeViewModel(MainViewModel callback)
        {
            this.callback = callback;
        }
    }
}