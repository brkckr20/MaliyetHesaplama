using System.Windows;
using System.Windows.Controls;

namespace MaliyeHesaplama.helpers
{
    public static class FocusHelper
    {
        public static readonly DependencyProperty SelectAllTextOnFocusProperty =
            DependencyProperty.RegisterAttached(
                "SelectAllTextOnFocus",
                typeof(bool),
                typeof(FocusHelper),
                new UIPropertyMetadata(false, OnSelectAllTextOnFocusChanged));

        public static bool GetSelectAllTextOnFocus(DependencyObject obj)
        {
            return (bool)obj.GetValue(SelectAllTextOnFocusProperty);
        }

        public static void SetSelectAllTextOnFocus(DependencyObject obj, bool value)
        {
            obj.SetValue(SelectAllTextOnFocusProperty, value);
        }

        private static void OnSelectAllTextOnFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox && (bool)e.NewValue)
            {
                textBox.GotFocus += TextBox_GotFocus;
            }
            else if (d is TextBox textBox2)
            {
                textBox2.GotFocus -= TextBox_GotFocus;
            }
        }

        private static void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }
    }
}
