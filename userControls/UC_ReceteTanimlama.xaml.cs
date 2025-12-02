using MaliyeHesaplama.helpers;
using MaliyeHesaplama.Interfaces;
using System.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace MaliyeHesaplama.userControls
{
    public partial class UC_ReceteTanimlama : UserControl, IPageCommands
    {
        int _InventoryType, inventoryId, Id = 0;
        MiniOrm _orm = new MiniOrm();
        private DataTable table;
        public UC_ReceteTanimlama(int inventoryType)
        {
            InitializeComponent();
            _InventoryType = inventoryType;
            ButtonBar.PageCommands = this;
            BaslangicVerileri();
        }

        public void Geri()
        {

        }

        public void Ileri()
        {

        }

        public void Kaydet()
        {
            _Kaydet();
        }

        public void Listele()
        {

        }

        public void Sil()
        {

        }

        public void Yazdir()
        {

        }

        public void Yeni()
        {

        }
        void BaslangicVerileri()
        {
            txtFisNo.Text = _orm.GetRecordNo("InventoryReceipt", "ReceiptNo", "InventoryType", _InventoryType);
            table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("InventoryId", typeof(int));
            table.Columns.Add("InventoryReceiptId", typeof(int));
            table.Columns.Add("OperationType", typeof(string));
            table.Columns.Add("InventoryCode", typeof(string));
            table.Columns.Add("InventoryName", typeof(string));
            table.Columns.Add("Variant", typeof(string));
            table.Columns.Add("Quantity", typeof(decimal));
            table.Columns.Add("Price", typeof(decimal));
            table.Columns.Add("Forex", typeof(string));
            table.Columns.Add("RowExplanation", typeof(string));
            table.Columns.Add("VariantId", typeof(int));
            table.Columns.Add("VariantCode", typeof(string));
            dataGrid.ItemsSource = table.DefaultView;
            LoadOperationTypesFromDb();
        }
        private void LoadOperationTypesFromDb()
        {
            var data = _orm.GetById<dynamic>("ProductionManagementParams", 1);
            string list = data.ReceteOperasyonTipleri;
            string forexList = data.DovizKurlari;
            cmbOperasyonTipleri.ItemsSource = list.Split(',').ToList();
            cmbDoviz.ItemsSource = forexList.Split(',').ToList();
        }
        private void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column.Header.ToString() == "Kalem İşlem")
            {
                if (dataGrid.SelectedItem is DataRowView drv)
                {
                    drv["Id"] = 0;
                }
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    dataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                }), System.Windows.Threading.DispatcherPriority.Background);
            }
            if (e.Column.Header.ToString() == "Miktar (Kg)" || e.Column.Header.ToString() == "Fiyat")
            {
                if (e.EditAction == DataGridEditAction.Commit)
                {
                    var editedCell = e.EditingElement as TextBox;
                    if (editedCell != null)
                    {
                        string text = editedCell.Text;
                        text = text.Replace('.', ',');

                        if (decimal.TryParse(text,
                                             System.Globalization.NumberStyles.Any,
                                             new CultureInfo("tr-TR"),
                                             out decimal result))
                        {
                            var rowView = e.Row.Item as DataRowView;
                            if (rowView != null)
                            {
                                rowView["Quantity"] = result;
                            }
                        }
                    }
                }
            }
        }

        private void btnKumasListe_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            DataRowView rowView = btn.DataContext as DataRowView;
            if (rowView == null) return;

            wins.winMalzemeListesi win = new wins.winMalzemeListesi(Convert.ToInt32(Enums.Inventory.Iplik));
            if (win.ShowDialog() == true)
            {
                rowView["InventoryId"] = win.Id;
                rowView["InventoryCode"] = win.Code;
                rowView["InventoryName"] = win.Name;
            }
        }

        private void btnVariantListe_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            DataRowView rowView = btn.DataContext as DataRowView;
            if (rowView == null) return;

            wins.winRenkListesi win = new wins.winRenkListesi(false);
            if (win.ShowDialog() == true)
            {
                rowView["VariantId"] = win.Id;
                rowView["VariantCode"] = win.Kodu;
                rowView["Variant"] = win.Adi;
            }
        }

        private void btnKumasKodu_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            wins.winMalzemeListesi win = new wins.winMalzemeListesi(_InventoryType);
            win.ShowDialog();
            if (win.DialogResult == true)
            {
                inventoryId = win.Id;
                txtKumasKodu.Text = win.Code;
                lblKumasAdi.Content = win.Name;
            }
        }
        void _Kaydet()
        {
            var dict0 = new Dictionary<string, object>
            {
                {"Id",Id },{"ReceiptNo",txtFisNo.Text},{"RawWidth",txtHamEn.Text},{"RawHeight",txtHamBoy.Text},{"ProductWidth",txtMamulEn.Text},{"ProductHeight",txtMamulBoy.Text}, {"RawGrammage",txtHamGramaj.Text},{"ProductGrammage",txtMamulGramaj.Text},{"ReceiptType", Convert.ToInt32(Enums.Receipt.KumasRecetesi)},{"InventoryId",inventoryId},{"InventoryType",Convert.ToInt32(Enums.Inventory.Kumas)}
            };
            Id = _orm.Save("InventoryReceipt", dict0);
            var dbColumns = new List<string> { "Id", "OperationType", "InventoryId", "VariantId", "Quantity", "Forex", "Price", "RowExplanation" };
            foreach (DataRow row in table.Rows)
            {
                if (row.RowState == DataRowState.Deleted) continue;
                var dict = new Dictionary<string, object>();
                foreach (var colName in dbColumns)
                {
                    var value = row[colName];
                    dict[colName] = value == DBNull.Value ? null : value;
                }
                dict["InventoryReceiptId"] = Id;
                int newId = _orm.Save("InventoryReceiptItem", dict, "Id");

                if (Convert.ToInt32(dict["Id"]) == 0)
                    row["Id"] = newId;
            }
            Bildirim.Bilgilendirme2("Kayıt işlemi başarılı");
        }
    }
}
