﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace MaimaiConsulationCenter.Assets.Convertor
{
    public class GenderConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null) return false;
            return value.ToString() == parameter.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return parameter;
        }
    }
}
