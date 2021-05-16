using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;
using Client.Utils.Logging;
using Client.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;

namespace Client.Utils.Navigation
{
    /// <summary>
    /// Provides a way to navigate through various <c>Pages</c> of a <c>Frame</c> in
    /// the WPF application.
    /// Also provides ways to interact with users through message dialog or file prompts.
    /// Inspired by https://paulstovell.com/wpf-navigation/
    /// </summary>
    public class ContentNavigation
    {
        // Bind navigation service
        // See: https://stackoverflow.com/a/52459022/12347616
        // Frame is the name of the `Frame`-Element
        private static readonly NavigationService Navigation
            = ((Application.Current.MainWindow as MainWindow)!).Frame.NavigationService;

        private readonly ILogger logger = ApplicationLogging.CreateLogger<ContentNavigation>();

        /// <summary>
        /// Navigate to a given <c>Page</c>.
        /// </summary>
        /// <param name="page">
        /// Enum that represents a concrete <c>Page</c>.
        /// </param>
        public void Navigate(ContentPage page)
        {
            logger.Log(LogLevel.Information, $"Navigating to {page}");
            var url = $"Views/{page.ToString()}.xaml";
            Navigation.Navigate(new Uri(url, UriKind.Relative));
            logger.Log(LogLevel.Information, $"Current page: {Navigation.Content}");
        }

        /// <summary>
        /// Shows user a simple info dialog.
        /// </summary>
        /// <param name="body">
        /// Text for the body of the dialog.
        /// </param>
        /// <param name="header">
        /// Text for the header of the dialog.
        /// </param>
        public void ShowInfoDialog(string body, string header = "")
        {
            MessageBox.Show(body, header,
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Shows user a simple info dialog and provides Ok / Cancel Feedback.
        /// </summary>
        /// <param name="body">
        /// Text for the body of the dialog.
        /// </param>
        /// <param name="header">
        /// Text for the header of the dialog.
        /// </param>
        /// <returns>
        /// True, if the users presses Ok, else false.
        /// </returns>
        public bool ShowInfoDialogWithQuestion(string body, string header = "")
        {
            var result = MessageBox.Show(body, header,
                MessageBoxButton.OKCancel, MessageBoxImage.Information);
            return result is MessageBoxResult.OK or MessageBoxResult.Yes;
        }

        /// <summary>
        /// Shows user a simple error dialog.
        /// </summary>
        /// <param name="body">
        /// Text for the body of the dialog.
        /// </param>
        /// <param name="header">
        /// Text for the header of the dialog.
        /// </param>
        public void ShowErrorDialog(string body, string header = "")
        {
            MessageBox.Show(body, header,
                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Shows user a simple error dialog and provides Ok / Cancel Feedback.
        /// </summary>
        /// <param name="body">
        /// Text for the body of the dialog.
        /// </param>
        /// <param name="header">
        /// Text for the header of the dialog.
        /// </param>
        /// <returns>
        /// True, if the users presses Ok, else false.
        /// </returns>
        public bool ShowErrorDialogWithQuestion(string body, string header = "")
        {
            var result = MessageBox.Show(body, header,
                MessageBoxButton.OKCancel, MessageBoxImage.Error);
            return result is MessageBoxResult.OK or MessageBoxResult.Yes;
        }

        /// <summary>
        /// Opens a file dialog in which a user can "open" a file (=select open path). 
        /// </summary>
        /// <param name="filter">
        /// Filter for files in the file manager.
        /// </param>
        /// <returns>
        /// Path to the file to open or null on error case.
        /// </returns>
        public string? ShowOpenFileDialog(string? filter = null)
        {
            var openFileDialog = new OpenFileDialog {Filter = filter};
            return openFileDialog.ShowDialog() == true ? openFileDialog.FileName : null;
        }

        /// <summary>
        /// Opens a file dialog in which a user can "save" a file (=select save path).
        /// </summary>
        /// <param name="filter">
        /// Filter for files in the file manager.
        /// </param>
        /// <returns>
        /// Path to the file to save or null on error case.
        /// </returns>
        public string? ShowSaveFileDialog(string? filter = null)
        {
            var saveFileDialog = new SaveFileDialog {Filter = filter};
            return saveFileDialog.ShowDialog() == true ? saveFileDialog.FileName : null;
        }

        /// <summary>
        /// Opens a help page in the default browser of the user.
        /// </summary>
        /// <param name="url">
        /// URL that will be opened.
        /// </param>
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

    /// <summary>
    /// States all <c>Pages</c> that are navigable.
    /// </summary>
    public enum ContentPage
    {
        AppAdd,
        AppInfo,
        AppWelcome,
    }
}