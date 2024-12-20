﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace MaimaiConsulationCenter.Assets.Convertor
{
    public class BoolToArrowConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && bool.Parse(value.ToString()))
            {
                return "↑";
            }
            return "↓";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
