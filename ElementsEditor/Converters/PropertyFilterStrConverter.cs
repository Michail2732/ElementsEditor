using Avalonia.Data;
using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ElementsEditor
{
    public class PropertyFilterStrConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is null)
                return string.Empty;
            var filter = value as IPropertyFilter;
            if (filter != null)
            {
                return $"{filter.Logic} {filter.PropertyName} {filter.Operation} {filter.GetValue()}";
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
