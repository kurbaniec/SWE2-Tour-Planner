using System;
using System.Windows;
using System.Windows.Navigation;
using Client.Views;

namespace Client.Utils.Navigation
{
    public class ContentNavigation
    {
        // Bind navigation service
        // See: https://stackoverflow.com/a/52459022/12347616
        // Frame is the name of the `Frame`-Element
        private static NavigationService navigation 
            = ((Application.Current.MainWindow as MainWindow)!).Frame.NavigationService;

        public void Navigate(ContentPage page)
        {
            var url = $"Views/{page.ToString()}.xaml";
            navigation.Navigate(new Uri(url, UriKind.Relative));
            Console.WriteLine(navigation.Content);
        }
    }

    public enum ContentPage
    {
        AppWelcome,
        AppInfo,
    }
}