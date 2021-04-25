using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;


namespace Client.Utils.Converters
{
    /// <summary>
    /// Returns a list of associated values from an enum.
    /// </summary>
    public class EnumListConverter : MarkupExtension, IValueConverter
    {
        private static EnumListConverter? instance;
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Check enum
            // See: https://stackoverflow.com/a/2918826/12347616
            if (value.GetType().IsEnum)
                return GetEnumList(value);
            throw new InvalidOperationException("EnumListConverter used on non enum value");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return instance ??= new EnumListConverter();
        }
        
        private static List<T> GetEnumList<T>(T value)
        {
            // Get enum list
            // See: https://stackoverflow.com/a/1167367/12347616
            return Enum.GetValues(value!.GetType()).Cast<T>().ToList();
        }   
    }
}