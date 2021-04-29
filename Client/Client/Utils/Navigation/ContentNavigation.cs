using System;
using System.Windows;
using System.Windows.Navigation;
using Client.Utils.Logging;
using Client.Views;
using Microsoft.Extensions.Logging;

namespace Client.Utils.Navigation
{
    // Inspired by https://paulstovell.com/wpf-navigation/
    public class ContentNavigation
    {
        // Bind navigation service
        // See: https://stackoverflow.com/a/52459022/12347616
        // Frame is the name of the `Frame`-Element
        private static readonly NavigationService Navigation 
            = ((Application.Current.MainWindow as MainWindow)!).Frame.NavigationService;

        private readonly ILogger logger = ApplicationLogging.CreateLogger<ContentNavigation>();

        public void Navigate(ContentPage page)
        {
            logger.Log(LogLevel.Information, $"Navigating to {page}");
            var url = $"Views/{page.ToString()}.xaml";
            Navigation.Navigate(new Uri(url, UriKind.Relative));
            logger.Log(LogLevel.Information, $"Current page: {Navigation.Content}");
        }
    }

    public enum ContentPage
    {
        AppAdd,
        AppInfo,
    }
}