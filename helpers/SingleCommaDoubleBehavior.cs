using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MaliyeHesaplama.helpers
{
    public static class SingleCommaDoubleBehavior
    {
        public static bool GetIsEnabled(DependencyObject obj) => (bool)obj.GetValue(IsEnabledProperty);
        public static void SetIsEnabled(DependencyObject obj, bool value) => obj.SetValue(IsEnabledProperty, value);

        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached(
                "IsEnabled",
                typeof(bool),
                typeof(SingleCommaDoubleBehavior),
                new UIPropertyMetadata(false, OnIsEnabledChanged));

        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is System.Windows.Controls.TextBox tb)
            {
                if ((bool)e.NewValue)
                {
                    tb.PreviewTextInput += Tb_PreviewTextInput;
                    //DataObject.AddPastingHandler(tb, OnPaste);
                }
                else
                {
                    tb.PreviewTextInput -= Tb_PreviewTextInput;
                    //DataObject.RemovePastingHandler(tb, OnPaste);
                }
            }
        }

        private static void Tb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var tb = sender as System.Windows.Controls.TextBox;
            string newText = tb.Text.Insert(tb.CaretIndex, e.Text);

            e.Handled = !IsValidSingleCommaNumber(newText);
        }

        private static void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(System.Windows.DataFormats.Text))
            {
                string pasteText = e.DataObject.GetData(System.Windows.DataFormats.Text) as string;
                var tb = sender as System.Windows.Controls.TextBox;
                string newText = tb.Text.Insert(tb.CaretIndex, pasteText);

                if (!IsValidSingleCommaNumber(newText)) e.CancelCommand();
            }
            else e.CancelCommand();
        }

        private static bool IsValidSingleCommaNumber(string text)
        {
            if (string.IsNullOrEmpty(text)) return true;

            // Sadece bir virgül olabilir
            int commaCount = text.Split(',').Length - 1;
            if (commaCount > 1) return false;

            // Virgülü noktaya çevirip parse edelim
            string parseText = text.Replace(',', '.');
            return double.TryParse(parseText, NumberStyles.Any, CultureInfo.InvariantCulture, out _);
        }
    }
}
