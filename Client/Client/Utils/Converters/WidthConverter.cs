using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace Client.Utils.Converters
{
    public class WidthConverter : MarkupExtension, IValueConverter
    {
        private static WidthConverter? instance;

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Get current window size
            // See: https://stackoverflow.com/a/7981366/12347616
            var windowHeight = ((Panel)Application.Current.MainWindow!.Content).ActualHeight;
            var windowWidth = ((Panel)Application.Current.MainWindow.Content).ActualWidth;
            // Get screen size
            // See: https://stackoverflow.com/a/25427320/12347616
            var screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            // Parse panel width (Element with the two resizable grids)
            var panelWidth = System.Convert.ToDouble(value);
            // Make grid "responsive"
            if (windowWidth >= screenWidth / 2)
            {
                return panelWidth / 2;
            }
            return panelWidth;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return instance ??= new WidthConverter();
        }
    }
}