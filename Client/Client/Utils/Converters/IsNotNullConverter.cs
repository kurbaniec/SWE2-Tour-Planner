using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Client.Utils.Converters
{
    public class IsNotNullConverter : MarkupExtension, IValueConverter
    {
        private static IsNotNullConverter? instance;
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value != null);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("IsNullConverter can only be used OneWay.");
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return instance ??= new IsNotNullConverter();
        }
    }
}