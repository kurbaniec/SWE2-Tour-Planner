﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Client.Utils.Navigation
{
    /// <summary>
    /// Used to disable navigation via shortcuts.
    /// See: https://stackoverflow.com/a/11037082/12347616
    /// </summary>
    public static class DisableNavigation
    {
        public static bool GetDisable(DependencyObject o)
        {
            return (bool) o.GetValue(DisableProperty);
        }

        public static void SetDisable(DependencyObject o, bool value)
        {
            o.SetValue(DisableProperty, value);
        }

        public static readonly DependencyProperty DisableProperty =
            DependencyProperty.RegisterAttached("Disable", typeof(bool), typeof(DisableNavigation),
                new PropertyMetadata(false, DisableChanged));


        public static void DisableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var frame = (Frame) sender;
            frame.Navigated += DontNavigate;
            frame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
        }

        public static void DontNavigate(object sender, NavigationEventArgs e)
        {
            ((Frame) sender).NavigationService.RemoveBackEntry();
        }
    }
}