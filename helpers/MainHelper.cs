using DocumentFormat.OpenXml.Office2010.Excel;
using MaliyeHesaplama.wins;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Data;
//using System.Windows.Forms;
using System.Windows.Threading;

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
        public static void SetRecordCount(ICollectionView _collectionView, Label count)
        {
            int visibleCount = _collectionView.Cast<dynamic>().Count();
            count.Content = $"Satır Sayısı: {visibleCount}";
        }

        public static void SearchWithColumnHeaderNoCollectionView(TextBox tb, DataTable table, string fieldName, Label lblCount, Label lblSumMeter)
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
        public static decimal SetFieldsSum(DataTable table, string field, Label lbl)
        {
            if (field == "KayıtNo")
            {
                int count = table.DefaultView.Count;
                lbl.Content = $"Toplam Kayıt: {count}";
                return count; // SAYIYI DÖN
            }

            decimal sum = 0;
            foreach (DataRowView rowView in table.DefaultView)
            {
                sum += rowView[field] != DBNull.Value ? Convert.ToDecimal(rowView[field]) : 0;
            }

            lbl.Content = $"Toplam Metre: {sum}";
            return sum;
        }
        public static void SearchWithCW(object sender, string fieldName, ICollectionView _collectionView, Label lblRecordCount)
        {
            var tb = sender as TextBox;
            SearchWithColumnHeader(tb, fieldName, _collectionView, lblRecordCount);
        }
        public static void SetCompanyInformation(ref int CompanyId, TextBox textBox)
        {
            wins.winFirmaListesi win = new wins.winFirmaListesi();
            win.ShowDialog();
            if (win.SecimYapildi)
            {
                CompanyId = win.Id;
                textBox.Text = win.FirmaUnvan;
            }
        }
        public static void SetWareHouseInformation(ref int CompanyId, TextBox textBox)
        {
            winDepoListesi win = new winDepoListesi();
            win.ShowDialog();
            if (win.SecimYapildi)
            {
                CompanyId = win.Id;
                textBox.Text = win.Kodu.ToString() + " - " + win.Adi;
            }
        }

        public static void SetInventoryInformation(object sender, Enums.Inventory _inventory)
        {
            Button btn = sender as Button;
            if (btn == null) return;
            DataRowView rowView = btn.DataContext as DataRowView;
            if (rowView == null) return;
            winMalzemeListesi win = new winMalzemeListesi(Convert.ToInt32(_inventory));
            if (win.ShowDialog() == true)
            {
                rowView["InventoryId"] = win.Id;
                rowView["InventoryCode"] = win.Code;
                rowView["InventoryName"] = win.Name;
            }
        }
        public static void OpenReportWindow(string screenName, int recordId)
        {
            if (recordId == 0)
            {
                Bildirim.Uyari2("Rapor alabilmek için lütfen bir kayıt seçiniz!");
            }
            else
            {
                wins.winRaporSecimi win = new winRaporSecimi(screenName, recordId);
                win.ShowDialog();
            }
        }

        public static string GetRecordStringQuery(int _receiptType)
        {
            return $@"SELECT 
                                ISNULL(R.Id,0) Id,ISNULL(R.ReceiptNo,'') ReceiptNo, ISNULL(R.ReceiptDate,'') ReceiptDate, ISNULL(R.CompanyId,0) CompanyId,ISNULL(R.Authorized,'') Authorized,ISNULL(R.CustomerOrderNo,'') CustomerOrderNo,
                                ISNULL(R.DuaDate,'') DuaDate,ISNULL(R.Explanation,'') Explanation,
                                ISNULL(RI.Id,0) [ReceiptItemId], ISNULL(RI.OperationType,'') OperationType,
                                ISNULL(RI.InventoryId,0) InventoryId, ISNULL(RI.NetMeter,0) NetMeter, ISNULL(RI.CashPayment,0) CashPayment, ISNULL(RI.DeferredPayment,0) DeferredPayment,
                                ISNULL(R.Maturity,0) Maturity, ISNULL(RI.RowExplanation,'') RowExplanation,ISNULL(RI.NetWeight,0) NetWeight,ISNULL(RI.Piece,0) Piece,
                                ISNULL(C.CompanyCode,'') CompanyCode, ISNULL(C.CompanyName,'') CompanyName,
                                ISNULL(I.InventoryCode,'') InventoryCode, ISNULL(I.InventoryName,'') InventoryName,
                                ISNULL(CO.Id,0) VariantId,ISNULL(CO.Code,'') VariantCode,ISNULL(CO.Name,'') Variant,ISNULL(RI.Forex,'') Forex,
                                ISNULL(RI.CustomerOrderNo,'') CustomerOrderNo_, ISNULL(RI.OrderNo,'') OrderNo_, ISNULL(RI.TrackingNumber,'') TrackingNumber
                                FROM Receipt R
                                INNER JOIN ReceiptItem RI ON R.Id = RI.ReceiptId
                                LEFT JOIN Company C ON C.Id = R.CompanyId
                                LEFT JOIN Inventory I ON I.Id = RI.InventoryId
                                LEFT JOIN Color CO on RI.VariantId = CO.Id
                                WHERE R.ReceiptType = {_receiptType} AND R.Id = @Id";
        }
        public static string GetEnumDisplayName(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attr = field.GetCustomAttribute<DisplayAttribute>();
            return attr?.Name ?? value.ToString();
        }
    }
}
