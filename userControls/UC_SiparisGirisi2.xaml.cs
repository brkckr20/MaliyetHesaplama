using MaliyeHesaplama.helpers;
using MaliyeHesaplama.Interfaces;
using MaliyeHesaplama.mvvm;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace MaliyeHesaplama.userControls
{
    public partial class UC_SiparisGirisi2 : UserControl, IPageCommands
    {
        MiniOrm _orm = new MiniOrm();
        public int CompanyId = 0, Id;
        MVM vm = new MVM();
        private DataTable table;
        public ObservableCollection<string> Currencies { get; set; }
        public UC_SiparisGirisi2()
        {
            InitializeComponent();
            ButtonBar.PageCommands = this;
            Currencies = new ObservableCollection<string>();
            LoadData();
        }
        public void KayitlariGetir(string KayitTipi)
        {
            try
            {
                int id = this.Id;
                int? istenenId = _orm.GetIdForAfterOrBeforeRecord(KayitTipi, "Receipt", id, "ReceiptItem", "ReceiptId", Convert.ToInt32(Enums.Receipt.Siparis));
                if (istenenId == null)
                {
                    Bildirim.Uyari2("Başka bir kayıt bulunamadı!");
                    return;
                }

                string query = $@"SELECT 
                                ISNULL(R.Id,0) Id,ISNULL(R.ReceiptNo,'') ReceiptNo, ISNULL(R.ReceiptDate,'') ReceiptDate, ISNULL(R.CompanyId,0) CompanyId,ISNULL(R.Authorized,'') Authorized,ISNULL(R.CustomerOrderNo,'') CustomerOrderNo,
                                ISNULL(R.DuaDate,'') DuaDate,ISNULL(R.Explanation,'') Explanation,
                                ISNULL(RI.Id,0) [ReceiptItemId], ISNULL(RI.OperationType,'') OperationType,
                                ISNULL(RI.InventoryId,0) InventoryId, ISNULL(RI.NetMeter,0) NetMeter, ISNULL(RI.CashPayment,0) CashPayment, ISNULL(RI.DeferredPayment,0) DeferredPayment,
                                ISNULL(R.Maturity,0) Maturity, ISNULL(RI.RowExplanation,'') RowExplanation,
                                ISNULL(C.CompanyCode,'') CompanyCode, ISNULL(C.CompanyName,'') CompanyName,
                                ISNULL(I.InventoryCode,'') InventoryCode, ISNULL(I.InventoryName,'') InventoryName,
                                ISNULL(CO.Id,0) VariantId,ISNULL(CO.Code,'') VariantCode,ISNULL(CO.Name,'') Variant,ISNULL(RI.Forex,'') Forex
                                FROM Receipt R
                                INNER JOIN ReceiptItem RI ON R.Id = RI.ReceiptId
                                LEFT JOIN Company C ON C.Id = R.CompanyId
                                LEFT JOIN Inventory I ON I.Id = RI.InventoryId
                                LEFT JOIN Color CO on RI.VariantId = CO.Id
                                WHERE R.ReceiptType = {Convert.ToInt32(Enums.Receipt.Siparis)} AND R.Id = @Id";

                var liste = _orm.GetAfterOrBeforeRecord(query, istenenId.Value);

                if (liste != null && liste.Count > 0)
                {
                    // Üst bilgileri doldur
                    var item = liste[0];
                    this.Id = Convert.ToInt32(item.Id);
                    this.CompanyId = Convert.ToInt32(item.CompanyId);
                    dpTarih.SelectedDate = Convert.ToDateTime(item.ReceiptDate);
                    dpTermin.SelectedDate = Convert.ToDateTime(item.DuaDate);
                    txtFirmaUnvan.Text = item.CompanyName.ToString();
                    txtYetkili.Text = item.Authorized.ToString();
                    txtVade.Text = item.Maturity.ToString();
                    txtMusteriOrderNo.Text = item.CustomerOrderNo.ToString();
                    txtAciklama.Text = item.Explanation;
                    txtFisNo.Text = item.ReceiptNo;

                    table.Clear();
                    foreach (var i in liste)
                    {
                        DataRow row = table.NewRow();
                        row["Id"] = i.ReceiptItemId;
                        row["OperationType"] = i.OperationType;
                        row["InventoryId"] = i.InventoryId;
                        row["NetMeter"] = i.NetMeter;
                        row["CashPayment"] = i.CashPayment;
                        row["DeferredPayment"] = i.DeferredPayment;
                        row["RowExplanation"] = i.RowExplanation;
                        row["Forex"] = i.Forex;
                        row["InventoryCode"] = i.InventoryCode;
                        row["InventoryName"] = i.InventoryName;
                        row["VariantId"] = i.VariantId;
                        row["VariantCode"] = i.VariantCode;
                        row["Variant"] = i.Variant;
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
            GetSumOrCount();
        }

        public void Geri()
        {
            KayitlariGetir("Önceki");
        }
        public void Ileri()
        {
            KayitlariGetir("");
        }
        public void Kaydet()
        {
            var dict0 = new Dictionary<string, object>()
            {
                {"Id", Id},{"ReceiptNo",txtFisNo.Text},{"ReceiptType", Convert.ToInt32(Enums.Receipt.Siparis)},{"ReceiptDate", dpTarih.SelectedDate.Value},{"CompanyId",CompanyId},{"DuaDate",dpTermin.SelectedDate.Value},{"Maturity",txtVade.Text},{"CustomerOrderNo",txtMusteriOrderNo.Text},{"Authorized",txtYetkili.Text},{"WareHouseId",Convert.ToInt32(Enums.Depo.HamKumasDepo)},{"Explanation",txtAciklama.Text}
            };
            Id = _orm.Save("Receipt", dict0);
            var dbColumns = new List<string> { "Id", "OperationType", "InventoryId", "NetMeter", "CashPayment", "DeferredPayment", "Forex", "RowExplanation", "VariantId" }; // db'ye kayıt edilecek tablo alanları - gridi doğrudan aldığı için
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
            GetSumOrCount();
        }

        public void Listele()
        {
            wins.winFisHareketleriListesi win = new wins.winFisHareketleriListesi(Convert.ToInt32(Enums.Depo.HamKumasDepo), Enums.Receipt.Siparis);
            win.ShowDialog();
            //if (win.secimYapildi)
            //{
            //    this.Id = win.Id;
            //    txtFisNo.Text = win.ReceiptNo;
            //    dpTarih.SelectedDate = win._Date;
            //    this.CompanyId = win.CompanyId;
            //    txtFirmaUnvan.Text = win.CompanyName;
            //    txtYetkili.Text = win.Authorized;
            //    dpTermin.SelectedDate = win.DuaDate;
            //    txtVade.Text = win.Maturity;
            //    txtMusteriOrderNo.Text = win.CustomerOrderNo;
            //    txtAciklama.Text = win.Explanation;
            //    table.Clear();
            //    foreach (var h in win.HareketlerListesi)
            //    {
            //        DataRow row = table.NewRow();
            //        row["Id"] = h.ReceiptItemId; // kalem kayıt no
            //        row["InventoryId"] = h.InventoryId;
            //        row["OperationType"] = h.OperationType;
            //        row["InventoryCode"] = h.InventoryCode;
            //        row["InventoryName"] = h.InventoryName;
            //        row["Variant"] = h.Variant;
            //        row["NetMeter"] = h.NetMeter;
            //        row["CashPayment"] = h.CashPayment;
            //        row["DeferredPayment"] = h.DeferredPayment;
            //        row["Forex"] = h.Forex;
            //        row["RowExplanation"] = h.RowExplanation;
            //        row["VariantId"] = h.VariantId;
            //        row["VariantCode"] = h.VariantCode;
            //        table.Rows.Add(row);
            //    }
            //    dataGrid.ItemsSource = table.DefaultView;
            //}
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
            wins.winRaporSecimi win = new wins.winRaporSecimi("Sipariş Girişi", Id);
            win.ShowDialog();

        }
        void Temizle()
        {
            this.Id = 0; this.CompanyId = 0;
            txtFisNo.Text = _orm.GetRecordNo("Receipt", "ReceiptNo", "ReceiptType", Convert.ToInt32(Enums.Receipt.Siparis));
            dpTarih.SelectedDate = DateTime.Now;
            dpTermin.SelectedDate = DateTime.Now;
            txtFirmaUnvan.Text = string.Empty;
            txtYetkili.Text = string.Empty;
            txtVade.Text = string.Empty;
            txtMusteriOrderNo.Text = string.Empty;
            txtAciklama.Text = string.Empty;
            table.Clear();

        }
        public void Yeni()
        {
            Temizle();
        }
        private void LoadCurrenciesFromDb()
        {
            var data = _orm.GetById<dynamic>("ProductionManagementParams", 1);
            string list = data.DovizKurlari;
            cmbDoviz.ItemsSource = list.Split(',').ToList();
        }
        void LoadData()
        {
            dpTarih.SelectedDate = DateTime.Now;
            dpTermin.SelectedDate = DateTime.Now;
            txtFisNo.Text = _orm.GetRecordNo("Receipt", "ReceiptNo", "ReceiptType", Convert.ToInt32(Enums.Receipt.Siparis));
            table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("InventoryId", typeof(int));
            table.Columns.Add("OperationType", typeof(string));
            table.Columns.Add("InventoryCode", typeof(string));
            table.Columns.Add("InventoryName", typeof(string));
            table.Columns.Add("Variant", typeof(string));
            table.Columns.Add("NetMeter", typeof(decimal));
            table.Columns.Add("CashPayment", typeof(decimal));
            table.Columns.Add("DeferredPayment", typeof(decimal));
            table.Columns.Add("Forex", typeof(string));
            table.Columns.Add("RowExplanation", typeof(string));
            table.Columns.Add("VariantId", typeof(int));
            table.Columns.Add("VariantCode", typeof(string));
            dataGrid.ItemsSource = table.DefaultView;
            LoadCurrenciesFromDb();
        }
        private void btnFirmaListesi_Click(object sender, RoutedEventArgs e)
        {
            wins.winFirmaListesi win = new wins.winFirmaListesi();
            win.ShowDialog();
            if (win.SecimYapildi)
            {
                CompanyId = win.Id;
                txtFirmaUnvan.Text = win.FirmaUnvan;
            }
        }
        private void btnYetkiliListesi_Click(object sender, RoutedEventArgs e)
        {
            wins.winYetkiliListesi win = new wins.winYetkiliListesi(vm.Receipt.CompanyId);
            win.ShowDialog();
            if (win.SecimYapildi)
            {
                txtYetkili.Text = win.Yetkili;
            }
        }
        private void btnKumasListe_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            DataRowView rowView = btn.DataContext as DataRowView;
            if (rowView == null) return;

            wins.winMalzemeListesi win = new wins.winMalzemeListesi(Convert.ToInt32(Enums.Inventory.Kumas));
            if (win.ShowDialog() == true)
            {
                rowView["InventoryId"] = win.Id;
                rowView["InventoryCode"] = win.Code;
                rowView["InventoryName"] = win.Name;
            }
        }

        private void btnVariantListe_Click(object sender, RoutedEventArgs e)
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

        private void srcKalemIslem_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;
            MainHelper.SearchWithColumnHeaderNoCollectionView(tb, table, "OperationType",lblRecordCount,lblSumMeter);
        }

        private void srcCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;
            MainHelper.SearchWithColumnHeaderNoCollectionView(tb, table,"InventoryCode", lblRecordCount, lblSumMeter);
        }

        private void srcName_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;
            MainHelper.SearchWithColumnHeaderNoCollectionView(tb, table, "InventoryName",lblRecordCount,lblSumMeter);
        }
        void GetSumOrCount()
        {
            MainHelper.SetFieldsSum(table, "NetMeter", lblSumMeter);
            MainHelper.SetFieldsSum(table, "KayıtNo", lblRecordCount);
        }
        private void RootControl_Loaded(object sender, RoutedEventArgs e)
        {
            GetSumOrCount();
        }

        void Search(object sender,string fieldName)
        {
            var tb = sender as TextBox;
            MainHelper.SearchWithColumnHeaderNoCollectionView(tb, table, fieldName, lblRecordCount, lblSumMeter);
        }

        private void srcVariantCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search(sender,"VariantCode");
        }

        private void srcVariantName_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search(sender, "Variant");
        }

        private void srcMeter_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search(sender, "NetMeter");
        }

        private void srcForex_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search(sender, "Forex");
        }

        private void srcCash_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search(sender, "CashPayment");
        }

        private void srcDeferred_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search(sender, "DeferredPayment");
        }

        private void srcRowExplanation_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search(sender, "RowExplanation");
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
            if (e.Column.Header.ToString() == "Metre" || e.Column.Header.ToString() == "Peşin Ödeme" || e.Column.Header.ToString() == "Vadeli Ödeme")
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
                                rowView["NetMeter"] = result;
                            }
                        }
                    }
                }
            }
            GetSumOrCount();
        }
    }
}
