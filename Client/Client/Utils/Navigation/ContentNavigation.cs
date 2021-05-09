using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Navigation;
using Client.Utils.Logging;
using Client.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;

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

        public void ShowInfoDialog(string body, string header = "")
        {
            MessageBox.Show(body, header, 
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
        
        public bool ShowInfoDialogWithQuestion(string body, string header = "")
        {
            var result = MessageBox.Show(body, header, 
                MessageBoxButton.OKCancel, MessageBoxImage.Information);
            return result is MessageBoxResult.OK or MessageBoxResult.Yes;
        }
        
        public void ShowErrorDialog(string body, string header = "")
        {
            MessageBox.Show(body, header, 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        
        public bool ShowErrorDialogWithQuestion(string body, string header = "")
        {
            var result = MessageBox.Show(body, header, 
                MessageBoxButton.OKCancel, MessageBoxImage.Error);
            return result is MessageBoxResult.OK or MessageBoxResult.Yes;
        }
        
        public string? ShowOpenFileDialog(string? filter = null)
        {
            var openFileDialog = new OpenFileDialog {Filter = filter};
            return openFileDialog.ShowDialog() == true ? openFileDialog.FileName : null;
        }

        public string? ShowSaveFileDialog(string? filter = null)
        {
            var saveFileDialog = new SaveFileDialog {Filter = filter};
            return saveFileDialog.ShowDialog() == true ? saveFileDialog.FileName : null;
        }

        public void ShowHelpPage(string url = "https://github.com/kurbaniec/SWE2-Tour-Planner")
        {
            // See: https://stackoverflow.com/a/502204/12347616
            // And: https://github.com/dotnet/runtime/issues/28005#issuecomment-442214248
            try
            {
                ProcessStartInfo psi = new()
                {
                    FileName = url,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, "Could not locate browser to open url");
                logger.Log(LogLevel.Error, ex.StackTrace);
                ShowErrorDialog(
                    "Could not locate a browser to open the help page.\n" +
                    $"Please visit '{url}' manually.", "Tour Planner - Help");
            }
        }
    }

    public enum ContentPage
    {
        AppAdd,
        AppInfo,
        AppWelcome,
    }
}