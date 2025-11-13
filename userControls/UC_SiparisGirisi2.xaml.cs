using MaliyeHesaplama.helpers;
using MaliyeHesaplama.Interfaces;
using MaliyeHesaplama.mvvm;
using System.Data;
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
        public UC_SiparisGirisi2()
        {
            InitializeComponent();
            ButtonBar.PageCommands = this;
            LoadData();
        }
        public void Geri()
        {
            Bildirim.Bilgilendirme2("Yeni implementasyon");
        }

        public void Ileri()
        {

        }

        public void Kaydet()
        {
            var dict0 = new Dictionary<string, object>()
            {
                {"Id", Id},{"ReceiptNo",txtFisNo.Text},{"ReceiptType", Convert.ToInt32(Enums.Receipt.Siparis)},{"ReceiptDate", dpTarih.SelectedDate.Value},{"CompanyId",CompanyId},{"DuaDate",dpTermin.SelectedDate.Value},{"Maturity",txtVade.Text},{"CustomerOrderNo",txtMusteriOrderNo.Text},{"Authorized",txtYetkili.Text}
            };
            var _receiptId = _orm.Save("Receipt", dict0);
            var dbColumns = new List<string> { "Id", "OperationType", "InventoryId" }; // dbye kayıt edilecek tablo alanları - gridi doğrudan aldığı için
            foreach (DataRow row in table.Rows)
            {
                if (row.RowState == DataRowState.Deleted) continue;
                var dict = new Dictionary<string, object>();
                foreach (var colName in dbColumns)
                {
                    dict[colName] = row[colName];
                }
                dict["ReceiptId"] = _receiptId;
                int newId = _orm.Save("ReceiptItem", dict, "Id");

                if (Convert.ToInt32(dict["Id"]) == 0)
                    row["Id"] = newId;
            }
            Bildirim.Bilgilendirme2("Kayıt işlemi başarılı bir şekilde gerçekleştirildi");
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
            Bildirim.Bilgilendirme2("Yeni implementasyon");
        }// kayıt işlemi tamamlandı. listeleme ve diğer crud işlemlerinden devam edilecek
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
            dataGrid.ItemsSource = table.DefaultView;
        }
        private void btnFirmaListesi_Click(object sender, RoutedEventArgs e)
        {
            wins.winFirmaListesi win = new wins.winFirmaListesi();
            win.ShowDialog();
            if (win.SecimYapildi)
            {
                vm.Receipt.CompanyId = win.Id;
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

    }
}
