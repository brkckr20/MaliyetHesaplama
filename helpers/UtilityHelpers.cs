using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace MaliyeHesaplama.helpers
{
    public class UtilityHelpers
    {
        MiniOrm _orm = new MiniOrm();
        public void GetOperationTypeList(string _field, DataGridComboBoxColumn combobox)
        {
            var data = _orm.GetById<dynamic>("ProductionManagementParams", 1);
            var dict = (IDictionary<string, object>)data;
            string list = dict[_field].ToString();
            combobox.ItemsSource = list.Split(',').ToList();
        }
        public void RemoveRow(RoutedEventArgs e, ref DataGrid dataGrid)
        {
            if (dataGrid.SelectedItem == null)
            {
                e.Handled = true;
                Bildirim.Uyari2("Lütfen silinecek satırı seçiniz!");
            }
            if (dataGrid.SelectedItem is DataRowView drv)
            {
                int id = Convert.ToInt32(drv["Id"]);
                if (_orm.Delete("ReceiptItem", id, true) > 0)
                {
                    drv.Row.Delete();
                }
            }
        }
    }
}
