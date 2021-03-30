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

            services.AddSingleton<Mediator>();
            services.AddSingleton<ContentNavigation>();
            // Provide dependencies
            // See: https://stackoverflow.com/a/53884452/12347616
            services.AddSingleton<MainViewModel>(x =>
                new MainViewModel(x.GetService<Mediator>()!, x.GetService<ContentNavigation>()!));
            services.AddSingleton<ListViewModel>(x =>
                new ListViewModel(x.GetService<Mediator>()!, x.GetService<ContentNavigation>()!));
            serviceProvider = services.BuildServiceProvider();
        }

        public MainViewModel MainViewModel
            => serviceProvider.GetService<MainViewModel>()!;

        public ListViewModel ListViewModel
            => serviceProvider.GetService<ListViewModel>()!;
    }
}