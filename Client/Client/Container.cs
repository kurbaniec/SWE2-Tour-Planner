﻿using Client.Utils.Mediators;
using Client.Utils.Navigation;
using Client.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Client
{
    public class Container
    {
        private readonly ServiceProvider serviceProvider;

        /// <summary>
        /// Builds the service provider for IoC.
        /// instantiated in App.xml.
        /// See: https://github.com/kienboec/EventListerInCSharp/blob/main/EventListerInCSharp/IoCContainerConfig.cs
        /// </summary>
        public Container()
        {
            var services = new ServiceCollection();

            services.AddSingleton<Mediator>();
            services.AddSingleton<ContentNavigation>();
            // Provide dependencies
            // See: https://stackoverflow.com/a/53884452/12347616
            services.AddSingleton<MainViewModel>(x =>
                new MainViewModel(x.GetService<Mediator>()!, x.GetService<ContentNavigation>()!));
            services.AddSingleton<MenuViewModel>(x =>
                new MenuViewModel(x.GetService<Mediator>()!, x.GetService<ContentNavigation>()!));
            services.AddSingleton<ListViewModel>(x =>
                new ListViewModel(x.GetService<Mediator>()!, x.GetService<ContentNavigation>()!));
            services.AddSingleton<InfoViewModel>(x =>
                new InfoViewModel(x.GetService<Mediator>()!, x.GetService<ContentNavigation>()!));
            serviceProvider = services.BuildServiceProvider();

            // Instance all singletons
            serviceProvider.GetService<MainViewModel>();
            serviceProvider.GetService<MenuViewModel>();
            serviceProvider.GetService<ListViewModel>();
            serviceProvider.GetService<InfoViewModel>();
        }

        public MainViewModel MainViewModel
            => serviceProvider.GetService<MainViewModel>()!;
        
        public MenuViewModel MenuViewModel
            => serviceProvider.GetService<MenuViewModel>()!;

        public ListViewModel ListViewModel
            => serviceProvider.GetService<ListViewModel>()!;

        public InfoViewModel InfoViewModel
            => serviceProvider.GetService<InfoViewModel>()!;
    }
}