using System.ComponentModel;
using System.Data;
using System.Windows.Controls;
using System.Windows.Data;

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
        public static void SearchWithColumnHeader(TextBox aranacakTextbox, string fieldAdi, ICollectionView _collectionView, Label lblRecordCount)
        {
            string filterText = aranacakTextbox.Text.ToLower();

            if (_collectionView != null)
            {
                _collectionView.Filter = item =>
                {
                    var dict = (IDictionary<string, object>)item;

                    if (dict.ContainsKey(fieldAdi) && dict[fieldAdi] != null)
                    {
                        string companyName = dict[fieldAdi].ToString().ToLower();
                        return companyName.Contains(filterText);
                    }
                    return false;
                };
                _collectionView.Refresh();
                int visibleCount = _collectionView.Cast<dynamic>().Count();
                lblRecordCount.Content = $"Toplam Kayıt: {visibleCount}";
            }
        }
        public static void SetRecordCount(ICollectionView _collectionView,Label count)
        {
            int visibleCount = _collectionView.Cast<dynamic>().Count();
            count.Content = $"Toplam Kayıt: {visibleCount}";
        }

        public static void SearchWithColumnHeaderNoCollectionView(TextBox tb, DataTable table,string fieldName,Label lblCount,Label lblSumMeter)
        {
            string filterText = tb.Text.Replace("'", "''");

            if (string.IsNullOrWhiteSpace(filterText))
            {
                table.DefaultView.RowFilter = "";
            }
            else
            {
                table.DefaultView.RowFilter = $"Convert({fieldName}, 'System.String') LIKE '%{filterText}%'";
            }
            lblCount.Content = $"Toplam Kayıt: {table.DefaultView.Count}";
            decimal sum = 0;
            foreach (DataRowView rowView in table.DefaultView)
            {
                sum += rowView["NetMeter"] != DBNull.Value ? Convert.ToDecimal(rowView["NetMeter"]) : 0;
            }
            lblSumMeter.Content = $"Toplam Metre: {sum}";
        }
        public static decimal SetFieldsSum(DataTable table,string field,Label lbl)
        {
            decimal sum = 0;
            if (field== "KayıtNo")
            {
                lbl.Content = $"Toplam Kayıt: {table.DefaultView.Count}";
            }
            else
            {
                foreach (DataRowView rowView in table.DefaultView)
                {
                    sum += rowView[field] != DBNull.Value ? Convert.ToDecimal(rowView[field]) : 0; // hata verdi
                }
                if (lbl.Content == "NetMeter")
                {
                    lbl.Content = $"Toplam Metre: {sum}";
                }
                //else if (true)
                //{

                //}             
            }
            return sum;

        }

    }
}
