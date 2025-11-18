using System.Windows.Controls;

namespace MaliyeHesaplama.helpers
{
    public static class MainHelper
    {
        public static void SetControls(Dictionary<Control, object> controlValues)
        {
            foreach (var item in controlValues)
            {
                if (item.Key is TextBox textBox)
                {
                    textBox.Text = item.Value?.ToString() ?? "";
                }
                else if (item.Key is CheckBox checkBox)
                {
                    if (item.Value is bool b)
                        checkBox.IsChecked = b;
                }
                else if (item.Key is ComboBox comboBox)
                {
                    comboBox.SelectedIndex = item.Value is int index ? index : -1;
                }
                else if (item.Key is RadioButton radioButton)
                {
                    if (item.Value is bool b)
                        radioButton.IsChecked = b;
                }
                else if (item.Key is Label label)
                {
                    label.Content = item.Value?.ToString();
                }
            }
        }

    }
}
