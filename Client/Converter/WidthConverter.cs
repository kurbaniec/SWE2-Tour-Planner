using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Client.Converter
{
    public class WidthConverter : MarkupExtension, IValueConverter
    {
        private static WidthConverter? instance;

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Get screen size https://stackoverflow.com/a/25427320/12347616
            var screenSize = System.Windows.SystemParameters.PrimaryScreenWidth;
            var currentWidth = System.Convert.ToDouble(value);
            if (currentWidth < screenSize / 2)
            {
                return currentWidth;
            }
            return screenSize * System.Convert.ToDouble(parameter,
                new System.Globalization.CultureInfo("en-US"));
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