using System.Windows.Controls;
using System.Windows.Input;

namespace Client.Views
{
    public partial class AppInfo : Page
    {
        public AppInfo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Allows to scroll over DataGrids in the view.
        /// See: https://stackoverflow.com/a/6693503/12347616
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UIElement_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}