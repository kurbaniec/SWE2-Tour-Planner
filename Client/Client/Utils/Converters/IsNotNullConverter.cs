using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Client.Utils.Converters
{
    /// <summary>
    /// Returns boolean value that answers question if the given object is not null.
    /// </summary>
    public class IsNotNullConverter : MarkupExtension, IValueConverter
    {
        private static IsNotNullConverter? instance;
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
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