using Avalonia.Data;
using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ElementsEditor
{
    public class BoolInverseConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                return !((bool)value);
            }
            return new BindingNotification(new InvalidCastException(),
                                           BindingErrorType.Error);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                return !((bool)value);
            }
            return new BindingNotification(new InvalidCastException(),
                                           BindingErrorType.Error);
        }
    }
}
