using Avalonia.Data;
using Avalonia.Data.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ElementsEditor
{
    public class CollectionAnyConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var castedValue = value as ICollection;
            if (castedValue != null)
            {
                return castedValue.Count > 0;
            }
            return new BindingNotification(new InvalidCastException(),
                                           BindingErrorType.Error);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
