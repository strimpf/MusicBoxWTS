using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicBoxWTS.Helpers
{
    class DollarAmountConverter : Windows.UI.Xaml.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string)
            {
                double amt;
                if (Double.TryParse((string)value, out amt))
                {
                    return string.Format("$(0:0.00)", amt);
                }
            }
            else if (value is double)
            {
                double v = (double)value;
                return v.ToString("C", CultureInfo.CurrentCulture);
            }
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
