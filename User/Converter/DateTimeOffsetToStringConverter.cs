using System;
using System.Globalization;
using System.Windows.Data;

public class DateTimeOffsetToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTimeOffset dto)
        {
            return dto.ToString("yyyy-MM-dd HH:mm:ss"); // 24-часовой формат
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (DateTimeOffset.TryParse(value as string, out DateTimeOffset result))
        {
            return result;
        }
        return DateTimeOffset.Now;
    }
}