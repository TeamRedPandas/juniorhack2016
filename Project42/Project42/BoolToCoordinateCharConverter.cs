using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Data;

namespace Project42
{
    public sealed class BoolToCoordinateCharConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView();

            if ((string)parameter == "latitude")
            {
                return resourceLoader.GetString((bool)value ? "N" : "S");
            }
            else
            {
                return resourceLoader.GetString((bool)value ? "E" : "W");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
