using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;


namespace Client.Utils.Converters
{
    /// <summary>
    /// Returns the single given item as a List so it can be used inside a DataGrid.
    /// Inspired by https://stackoverflow.com/a/34752584/12347616.
    /// </summary>
    public class ItemSourceConverter : MarkupExtension, IValueConverter
    {
        private static ItemSourceConverter? instance;
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new List<object>(){ value };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null!;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return instance ??= new ItemSourceConverter();
        }
    }
}