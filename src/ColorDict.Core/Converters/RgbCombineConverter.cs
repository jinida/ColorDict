using System.Globalization;
using System.Windows.Data;

namespace ColorDict.Core.Converters
{
    public class RgbCombineConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is int && values[1] is int && values[2] is int)
                return "RGB(" + ((int)values[0]).ToString("000") + ", " + 
                    ((int)values[1]).ToString("000") + ", " + 
                    ((int)values[2]).ToString("000") + ")";
            return "RGB(000, 000, 000)";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}