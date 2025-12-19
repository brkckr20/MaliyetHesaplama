using MaliyeHesaplama.helpers;
using MaliyeHesaplama.Interfaces;
using System.Data;
using System.Windows.Controls;

namespace MaliyeHesaplama.userControls
{
    public partial class UC_UretimGirisi : UserControl, IPageCommands
    {
        MiniOrm _orm = new MiniOrm();
        public int CompanyId = 0, Id,WareHouseId;
        private DataTable table;
        FilterGridHelpers fgh;
        public UC_UretimGirisi()
        {
            InitializeComponent();
            ButtonBar.PageCommands = this;
            LoadData();
        }

        public void Geri()
        {
            //
        }

        public void Ileri()
        {
            //
        }

        public void Kaydet()
        {
            var dict0 = new Dictionary<string, object>()
            {
                {"Id", Id},{"ReceiptNo",txtFisNo.Text},{"ReceiptType", Convert.ToInt32(Enums.Receipt.UretimGirisi)},{"ReceiptDate", dpTarih.SelectedDate.Value},{"CompanyId",CompanyId},{"WareHouseId",WareHouseId},{"Explanation",txtAciklama.Text},{"InvoiceNo",txtBelgeNo.Text}
            };

            // DataGrid'den gelen satırları item dictionary listesine çevir
            var dbColumns = new List<string> { "Id", "OperationType", "InventoryId", "NetMeter", "NetWeight", "Piece", "RowExplanation", "VariantId", "BatchNo", "OrderNo" };
            var items = new List<Dictionary<string, object>>();
            foreach (DataRow row in table.Rows)
            {
                if (row.RowState == DataRowState.Deleted) continue;
                var dict = new Dictionary<string, object>();
                foreach (var colName in dbColumns)
                {
                    if (table.Columns.Contains(colName))
                    {
                        var value = row[colName];
                        dict[colName] = value == DBNull.Value ? null : value;
                    }
                    else
                    {
                        dict[colName] = null;
                    }
                }
                // garanti: Id null ise 0 yap
                dict["Id"] = dict["Id"] ?? 0;
                items.Add(dict);
            }

            try
            {
                Id = _orm.SaveReceiptWithStock(dict0, items, WareHouseId, Properties.Settings.Default.RememberUserId);
                Bildirim.Bilgilendirme2("Kayıt ve stok güncelleme başarıyla tamamlandı.");
            }
            catch (Exception ex)
            {
                Bildirim.Uyari2("Kayıt sırasında hata: " + ex.Message);
            }
            #region Eski Kod
            //var dict0 = new Dictionary<string, object>()
            //{
            //    {"Id", Id},{"ReceiptNo",txtFisNo.Text},{"ReceiptType", Convert.ToInt32(Enums.Receipt.UretimGirisi)},{"ReceiptDate", dpTarih.SelectedDate.Value},{"CompanyId",CompanyId},{"WareHouseId",WareHouseId},{"Explanation",txtAciklama.Text},{"InvoiceNo",txtBelgeNo.Text}
            //};
            //Id = _orm.Save("Receipt", dict0);
            //var dbColumns = new List<string> { "Id", "OperationType", "InventoryId", "NetMeter", "NetWeight", "Piece","RowExplanation"}; // db'ye kayıt edilecek tablo alanları - gridi doğrudan aldığı için
            //foreach (DataRow row in table.Rows)
            //{
            //    if (row.RowState == DataRowState.Deleted) continue;
            //    var dict = new Dictionary<string, object>();
            //    foreach (var colName in dbColumns)
            //    {
            //        var value = row[colName];
            //        dict[colName] = value == DBNull.Value ? null : value;
            //    }
            //    dict["ReceiptId"] = Id;
            //    int newId = _orm.Save("ReceiptItem", dict, "Id");

            //    if (Convert.ToInt32(dict["Id"]) == 0)
            //        row["Id"] = newId;
            //}
            //Bildirim.Bilgilendirme2("Kayıt işlemi başarılı bir şekilde gerçekleştirildi");
            ////GetSumOrCount();
            #endregion;
        }

        public void Listele()
        {
            //
        }

        public void Sil()
        {
            //
        }

        public void Yazdir()
        {
            //
        }

        public void Yeni()
        {
            
        }

        private void btnFirmaListesi_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MainHelper.SetCompanyInformation(ref CompanyId, txtFirmaUnvan);
        }

        private void btnDepoListesi_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MainHelper.SetWareHouseInformation(ref WareHouseId, txtDepo);
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
        }

        private void MI_AcikSiparisler_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            wins.winAcikSiparisler win = new wins.winAcikSiparisler(12);
            var result = win.ShowDialog();
            if (result == true)
            {
                var selected = win.SelectedReceipts;
                foreach (var r in selected)
                {
                    var row = table.NewRow();
                    row["Id"] = 0;
                    row["InventoryId"] = r.InventoryId;
                    row["OperationType"] = "Üretim";
                    row["InventoryCode"] = r.InventoryCode ?? string.Empty;
                    row["InventoryName"] = r.InventoryName ?? string.Empty;
                    row["NetMeter"] = r.NetMeter;
                    row["NetWeight"] = 0m; // GetMovementList sorgusunda NetWeight yoksa 0 atandı
                    row["Piece"] = 0m;
                    row["RowExplanation"] = r.RowExplanation ?? string.Empty;
                    row["CustomerOrderNo"] = r.CustomerOrderNo ?? string.Empty;
                    row["ReceiptNo"] = r.ReceiptNo ?? string.Empty;
                    table.Rows.Add(row);
                }
            }
        }

        private void btnKumasListe_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MainHelper.SetInventoryInformation(sender, Enums.Inventory.Kumas);
        }
        void LoadData()
        {
            dpTarih.SelectedDate = DateTime.Now;
            //dpTermin.SelectedDate = DateTime.Now;
            txtFisNo.Text = _orm.GetRecordNo("Receipt", "ReceiptNo", "ReceiptType", Convert.ToInt32(Enums.Receipt.UretimGirisi));
            table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("InventoryId", typeof(int));
            table.Columns.Add("OperationType", typeof(string));
            table.Columns.Add("InventoryCode", typeof(string));
            table.Columns.Add("InventoryName", typeof(string));
            //table.Columns.Add("Variant", typeof(string));
            table.Columns.Add("NetMeter", typeof(decimal));
            table.Columns.Add("NetWeight", typeof(decimal));
            table.Columns.Add("Piece", typeof(decimal));
            //table.Columns.Add("CashPayment", typeof(decimal));
            //table.Columns.Add("DeferredPayment", typeof(decimal));
            //table.Columns.Add("Forex", typeof(string));
            table.Columns.Add("RowExplanation", typeof(string));
            table.Columns.Add("CustomerOrderNo", typeof(string));
            table.Columns.Add("ReceiptNo", typeof(string));
            //table.Columns.Add("VariantId", typeof(int));
            //table.Columns.Add("VariantCode", typeof(string));
            dataGrid.ItemsSource = table.DefaultView;
            //LoadCurrenciesFromDb();
        }
    }
}
