using System;
using Client.Logic.BL;
using Client.Logic.DAL;
using Client.Logic.Setup;
using Client.Utils.Mediators;
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

            // Provide dependencies
            // See: https://stackoverflow.com/a/53884452/12347616
            services.AddSingleton<Configuration>();
            services.AddSingleton<Mediator>();
            services.AddSingleton<ContentNavigation>();
            services.AddSingleton<ITourApi>(x =>
                new TourApi(x.GetService<Configuration>()!));
            services.AddSingleton<TourPlannerClient>(x =>
                new TourPlannerClient(x.GetService<ITourApi>()!));
            services.AddSingleton<MainViewModel>(x =>
                new MainViewModel(x.GetService<Mediator>()!, x.GetService<ContentNavigation>()!));
            services.AddSingleton<MenuViewModel>(x =>
                new MenuViewModel(x.GetService<Mediator>()!, x.GetService<ContentNavigation>()!));
            services.AddSingleton<ListViewModel>(x =>
                new ListViewModel(x.GetService<TourPlannerClient>()!, x.GetService<Mediator>()!,
                    x.GetService<ContentNavigation>()!, x.GetService<Configuration>()!));
            services.AddSingleton<InfoViewModel>(x =>
                new InfoViewModel(x.GetService<TourPlannerClient>()!, x.GetService<Mediator>()!,
                    x.GetService<ContentNavigation>()!));
            services.AddSingleton<AddViewModel>(x =>
                new AddViewModel(x.GetService<TourPlannerClient>()!, x.GetService<Mediator>()!,
                    x.GetService<ContentNavigation>()!));
            serviceProvider = services.BuildServiceProvider();

            // Instance Logic & ViewModels
            serviceProvider.GetService<TourPlannerClient>();
            serviceProvider.GetService<MainViewModel>();
            serviceProvider.GetService<MenuViewModel>();
            serviceProvider.GetService<ListViewModel>();
            serviceProvider.GetService<InfoViewModel>();
            serviceProvider.GetService<AddViewModel>();
        }

        public MainViewModel MainViewModel
            => serviceProvider.GetService<MainViewModel>()!;

        public MenuViewModel MenuViewModel
            => serviceProvider.GetService<MenuViewModel>()!;

        public ListViewModel ListViewModel
            => serviceProvider.GetService<ListViewModel>()!;

        public InfoViewModel InfoViewModel
            => serviceProvider.GetService<InfoViewModel>()!;

        public AddViewModel AddViewModel
            => serviceProvider.GetService<AddViewModel>()!;
    }
}