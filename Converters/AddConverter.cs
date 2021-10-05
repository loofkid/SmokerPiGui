using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;

namespace SmokerPiGui.Converters
{
    public class AddConverter : MarkupExtension, IValueConverter
    {
        private AddConverter? _instance;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value + double.Parse((string)parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ??= new AddConverter();
        }
    }
}