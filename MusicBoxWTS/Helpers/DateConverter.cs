using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicBoxWTS.Helpers
{
    class DateConverter : Windows.UI.Xaml.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(value is DateTime)
            {

                DateTime v = (DateTime)value;
                return v.ToString("dddd MM/dd/yyyy",CultureInfo.CreateSpecificCulture("en-US"));
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
