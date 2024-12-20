﻿using System.Windows;
using System.Windows.Controls;

namespace MaimaiConsulationCenter.Common
{
    public class PasswordHelper
    {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordHelper), new
                FrameworkPropertyMetadata("", new PropertyChangedCallback(OnPropertyChanged)));
        public static string GetPassword(DependencyObject e)
        {
            return (string)e.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject e, string value)
        {
            e.SetValue(PasswordProperty, value);
        }

        public static readonly DependencyProperty AttachProperty =
            DependencyProperty.RegisterAttached("Attach", typeof(bool), typeof(PasswordHelper), new
                FrameworkPropertyMetadata(default(bool), new PropertyChangedCallback(OnAttached)));
        public static bool GetAttach(DependencyObject e)
        {
            return (bool)e.GetValue(AttachProperty);
        }

        public static void SetAttach(DependencyObject e, bool value)
        {
            e.SetValue(AttachProperty, value);
        }

        static bool _isUpdating = false;

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox password = d as PasswordBox;
            password.PasswordChanged -= Password_PasswordChanged;
            if (!_isUpdating)
                password.Password = e.NewValue?.ToString();
            password.PasswordChanged += Password_PasswordChanged;
        }
        private static void OnAttached(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox password = d as PasswordBox;
            password.PasswordChanged += Password_PasswordChanged;
        }

        private static void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            _isUpdating = true;
            SetPassword(passwordBox, passwordBox.Password);
            _isUpdating = false;
        }
    }
}
