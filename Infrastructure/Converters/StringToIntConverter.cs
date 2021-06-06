using KantorLr13.Infrastructure.Converters.Base;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace KantorLr13.Infrastructure.Converters
{
	public class StringToIntConverter : Converter
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is int)
			{
				return value.ToString();
			}
			return "";
		}
		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (int.TryParse((string)value, out int number))
			{
				return number;
			}
			else return 0;
		}
	}
}
