using System;
using System.Globalization;
using System.Windows.Data;

namespace DogsRUs.Converters
{
	public class CapitalizeFirstLetterConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			// this will be called after getting the value from your backing property
			// and before displaying it in the label, so we just pass it as-is
			if (value is string)
			{
				var castValue = (string)value;
				return char.ToUpper(castValue[0]) + castValue.Substring(1);
			}
			else
			{
				return value;
			}
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}
	}
}
