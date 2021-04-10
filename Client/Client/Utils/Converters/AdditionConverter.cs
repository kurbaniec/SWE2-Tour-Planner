using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace Client.Utils.Converters
{
    public class AdditionConverter : MarkupExtension, IValueConverter
    {
        private static AdditionConverter? instance;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var baseValue = System.Convert.ToDouble(value);
            var additionValue = System.Convert.ToDouble(parameter,
                new System.Globalization.CultureInfo("en-US"));
            return baseValue + additionValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return instance ??= new AdditionConverter();
        }
    }
}