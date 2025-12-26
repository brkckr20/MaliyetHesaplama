using MaliyeHesaplama.helpers;
using MaliyeHesaplama.Interfaces;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace MaliyeHesaplama.userControls
{
    public partial class UC_UretimGirisi : UserControl, IPageCommands
    {
        MiniOrm _orm = new MiniOrm();
        public int CompanyId = 0, Id, WareHouseId;
        private DataTable table;
        FilterGridHelpers fgh;
        int _receiptType = Convert.ToInt32(Enums.Receipt.UretimGirisi);
        public UC_UretimGirisi()
        {
            InitializeComponent();
            ButtonBar.PageCommands = this;
            LoadData();
        }

        public void Geri()
        {
            KayitlariGetir("Önceki");
        }

        public void Ileri()
        {
            KayitlariGetir("Sonraki");
        }

        public void Kaydet() 
        {
            #region kontrol edilecek - ai verdi
            //var dict0 = new Dictionary<string, object>()
            //{
            //    {"Id", Id},{"ReceiptNo",txtFisNo.Text},{"ReceiptType", Convert.ToInt32(Enums.Receipt.UretimGirisi)},{"ReceiptDate", dpTarih.SelectedDate.Value},{"CompanyId",CompanyId},{"WareHouseId",WareHouseId},{"Explanation",txtAciklama.Text},{"InvoiceNo",txtBelgeNo.Text}
            //};

            //// DataGrid'den gelen satırları item dictionary listesine çevir
            //var dbColumns = new List<string> { "Id", "OperationType", "InventoryId", "NetMeter", "NetWeight", "Piece", "RowExplanation", "VariantId", "BatchNo", "OrderNo" };
            //var items = new List<Dictionary<string, object>>();
            //foreach (DataRow row in table.Rows)
            //{
            //    if (row.RowState == DataRowState.Deleted) continue;
            //    var dict = new Dictionary<string, object>();
            //    foreach (var colName in dbColumns)
            //    {
            //        if (table.Columns.Contains(colName))
            //        {
            //            var value = row[colName];
            //            dict[colName] = value == DBNull.Value ? null : value;
            //        }
            //        else
            //        {
            //            dict[colName] = null;
            //        }
            //    }
            //    // garanti: Id null ise 0 yap
            //    dict["Id"] = dict["Id"] ?? 0;
            //    items.Add(dict);
            //}

            //try
            //{
            //    if (CompanyId == 0)
            //    {
            //        Bildirim.Uyari2("Kayıt sırasında hata:\nFirma seçilmeden kayıt işlemi yapılamaz!");
            //        return;
            //    }
            //    int _userId = Properties.Settings.Default.RememberUserId;
            //    Id = _orm.SaveReceiptWithStock(dict0, items, WareHouseId, _userId, "Receipt", "ReceiptItem");
            //    Bildirim.Bilgilendirme2("Kayıt işlemi başarıyla gerçekleştirildi.");
            //}
            //catch (Exception ex)
            //{
            //    Bildirim.Uyari2("Kayıt sırasında hata: " + ex.Message);
            //}
            #endregion
            var dict0 = new Dictionary<string, object>()
            {
                {"Id", Id},{"ReceiptNo",txtFisNo.Text},{"ReceiptType", Convert.ToInt32(Enums.Receipt.UretimGirisi)},{"ReceiptDate", dpTarih.SelectedDate.Value},{"CompanyId",CompanyId},{"WareHouseId",WareHouseId},{"Explanation",txtAciklama.Text},{"InvoiceNo",txtBelgeNo.Text}
            };
            Id = _orm.Save("Receipt", dict0);
            var dbColumns = new List<string> { "Id", "OperationType", "InventoryId", "NetMeter", "NetWeight", "Piece", "RowExplanation", "TrackingNumber", "CustomerOrderNo", "OrderNo" }; // db'ye kayıt edilecek tablo alanları - gridi doğrudan aldığı için
            foreach (DataRow row in table.Rows)
            {
                if (row.RowState == DataRowState.Deleted) continue;
                var dict = new Dictionary<string, object>();
                foreach (var colName in dbColumns)
                {
                    var value = row[colName];
                    dict[colName] = value == DBNull.Value ? null : value;
                }
                dict["ReceiptId"] = Id;
                int newId = _orm.Save("ReceiptItem", dict, "Id");

                if (Convert.ToInt32(dict["Id"]) == 0)
                    row["Id"] = newId;
            }
            Bildirim.Bilgilendirme2("Kayıt işlemi başarılı bir şekilde gerçekleştirildi");
            //GetSumOrCount();
        }

        public void Listele()
        {
            wins.winFisHareketleriListesi win = new wins.winFisHareketleriListesi(WareHouseId, Enums.Receipt.UretimGirisi, false);
            win.ShowDialog();
            if (win.secimYapildi)
            {
                this.Id = win.Id;
                txtFisNo.Text = win.ReceiptNo;
                dpTarih.SelectedDate = win._Date;
                this.CompanyId = win.CompanyId;
                txtFirmaUnvan.Text = win.CompanyName;
                WareHouseId = win._depoId;
                txtDepo.Text = win.WareHouseCode + " - " + win.WareHouseName;
                //dpTermin.SelectedDate = win.DuaDate;
                //txtVade.Text = win.Maturity;
                //txtMusteriOrderNo.Text = win.CustomerOrderNo;
                txtAciklama.Text = win.Explanation;
                table.Clear();
                foreach (var h in win.HareketlerListesi)
                {
                    DataRow row = table.NewRow();
                    row["Id"] = h.ReceiptItemId; // kalem kayıt no
                    row["InventoryId"] = h.InventoryId;
                    row["OperationType"] = h.OperationType;
                    row["InventoryCode"] = h.InventoryCode;
                    row["InventoryName"] = h.InventoryName;
                    //row["Variant"] = h.Variant;
                    row["NetMeter"] = h.NetMeter;
                    row["NetWeight"] = h.NetWeight;
                    row["Piece"] = h.Piece;
                    //row["CashPayment"] = h.CashPayment;
                    //row["DeferredPayment"] = h.DeferredPayment;
                    //row["Forex"] = h.Forex;
                    row["RowExplanation"] = h.RowExplanation;
                    //row["VariantId"] = h.VariantId;
                    //row["VariantCode"] = h.VariantCode;
                    row["CustomerOrderNo"] = h.CustomerOrderNo;
                    row["OrderNo"] = h.OrderNo;
                    row["TrackingNumber"] = h.TrackingNumber != null ? h.TrackingNumber : 0;
                    table.Rows.Add(row);
                }
                dataGrid.ItemsSource = table.DefaultView;
            }
            //GetSumOrCount();
        }

        public void Sil()
        {
            if (_orm.Delete("Receipt", Id, true) > 0)
            {
                _orm.Delete("ReceiptItem", Id, false, "ReceiptId");
                Temizle();
            }
        }

        public void Yazdir()
        {
            MainHelper.OpenReportWindow("Üretim Girişi", Id);
        }

        public void Yeni()
        {
            Temizle();
        }
        void Temizle()
        {
            this.Id = 0; this.CompanyId = 0;
            txtFisNo.Text = _orm.GetRecordNo("Receipt", "ReceiptNo", "ReceiptType", Convert.ToInt32(Enums.Receipt.UretimGirisi));
            dpTarih.SelectedDate = DateTime.Now;
            //dpTermin.SelectedDate = DateTime.Now;
            txtFirmaUnvan.Text = string.Empty;
            txtBelgeNo.Text = string.Empty;
            WareHouseId = 0;
            txtDepo.Text = string.Empty;
            //txtVade.Text = string.Empty;
            //txtMusteriOrderNo.Text = string.Empty;
            txtAciklama.Text = string.Empty;
            table.Clear();

        }
        public void KayitlariGetir(string KayitTipi)
        {
            try
            {
                int id = this.Id;
                int? istenenId = _orm.GetIdForAfterOrBeforeRecord(KayitTipi, "Receipt", id, "ReceiptItem", "ReceiptId", _receiptType);
                if (istenenId == null)
                {
                    Bildirim.Uyari2("Başka bir kayıt bulunamadı!");
                    return;
                }

                string query = MainHelper.GetRecordStringQuery(_receiptType);

                var liste = _orm.GetAfterOrBeforeRecord(query, istenenId.Value);

                if (liste != null && liste.Count > 0)
                {
                    // Üst bilgileri doldur
                    var item = liste[0];
                    this.Id = Convert.ToInt32(item.Id);
                    this.CompanyId = Convert.ToInt32(item.CompanyId);
                    dpTarih.SelectedDate = Convert.ToDateTime(item.ReceiptDate);
                    //dpTermin.SelectedDate = Convert.ToDateTime(item.DuaDate);
                    txtFirmaUnvan.Text = item.CompanyName.ToString();
                    //txtYetkili.Text = item.Authorized.ToString();
                    //txtVade.Text = item.Maturity.ToString();
                    //txtMusteriOrderNo.Text = item.CustomerOrderNo.ToString();
                    txtAciklama.Text = item.Explanation;
                    txtFisNo.Text = item.ReceiptNo;
                    txtBelgeNo.Text = item.InvoiceNo;

                    table.Clear();
                    foreach (var i in liste)
                    {
                        DataRow row = table.NewRow();
                        row["Id"] = i.ReceiptItemId;
                        row["OperationType"] = i.OperationType;
                        row["InventoryId"] = i.InventoryId;
                        row["NetMeter"] = i.NetMeter;
                        //row["CashPayment"] = i.CashPayment;
                        //row["DeferredPayment"] = i.DeferredPayment;
                        row["RowExplanation"] = i.RowExplanation;
                        //row["Forex"] = i.Forex;
                        row["InventoryCode"] = i.InventoryCode;
                        row["InventoryName"] = i.InventoryName;
                        //row["VariantId"] = i.VariantId;
                        //row["VariantCode"] = i.VariantCode;
                        //row["Variant"] = i.Variant;
                        row["CustomerOrderNo"] = i.CustomerOrderNo_; // alt çizgi eklendi çünkü çakışma vardı - ReceiptItem ve Receipt tablosunda aynı isimde alan var
                        row["OrderNo"] = i.OrderNo_;
                        row["TrackingNumber"] = i.TrackingNumber;
                        row["NetWeight"] = i.NetWeight;
                        row["Piece"] = i.Piece;
                        table.Rows.Add(row);
                    }

                    // DataGrid artık DataTable üzerinden çalışıyor
                    dataGrid.ItemsSource = table.DefaultView;
                }
                else
                {
                    Bildirim.Uyari2("Başka bir kayıt bulunamadı.");
                }
            }
            catch (Exception ex)
            {
                Bildirim.Uyari2("Hata: " + ex.Message);
            }
            //GetSumOrCount();
        }

        private void btnFirmaListesi_Click(object sender, RoutedEventArgs e)
        {
            MainHelper.SetCompanyInformation(ref CompanyId, txtFirmaUnvan);
        }

        private void btnDepoListesi_Click(object sender, RoutedEventArgs e)
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

        private void MI_AcikSiparisler_Click(object sender, RoutedEventArgs e)
        {
            string condition = $"R.ReceiptType = {Convert.ToInt32(Enums.Receipt.Siparis)} and R.Approved = 1";
            var data = _orm.GetById<dynamic>("ProductionManagementParams", 1);
            if (Convert.ToBoolean(data.UretimGirisiDepoZorunlu) && WareHouseId == 0)
            {
                condition += $" AND WareHouseId = {WareHouseId}";
                Bildirim.Uyari2("Lütfen bir depo seçimi yapınız.");
                return;
            }
            if (Convert.ToBoolean(data.UretimGirisiDepoZorunlu))
            {
                condition += $" AND WareHouseId = {WareHouseId}";
            }

            wins.winAcikSiparisler win = new wins.winAcikSiparisler(condition);
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
                    row["NetWeight"] = 0m;
                    row["Piece"] = 0m;
                    row["RowExplanation"] = r.RowExplanation ?? string.Empty;
                    row["CustomerOrderNo"] = r.CustomerOrderNo ?? string.Empty;
                    row["ReceiptNo"] = r.ReceiptNo ?? string.Empty;
                    row["TrackingNumber"] = r.ReceiptItemId;
                    row["OrderNo"] = r.OrderNo;
                    table.Rows.Add(row);
                }
            }
        }

        private void MI_SatirSil_Click(object sender, RoutedEventArgs e)
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
            table.Columns.Add("OrderNo", typeof(string));
            table.Columns.Add("ReceiptNo", typeof(string));
            //table.Columns.Add("VariantId", typeof(int));
            //table.Columns.Add("VariantCode", typeof(string));
            table.Columns.Add("TrackingNumber", typeof(int));
            dataGrid.ItemsSource = table.DefaultView;
            //LoadCurrenciesFromDb();
        }
    }
}
