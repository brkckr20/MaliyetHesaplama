using MaliyeHesaplama.helpers;
using MaliyeHesaplama.Interfaces;
using MaliyeHesaplama.models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MaliyeHesaplama.userControls
{
    public partial class UC_SiparisGirisi : UserControl, IPageCommands
    {
        MiniOrm _orm = new MiniOrm();
        int Id = 0, CompanyId = 0, DepoId = Convert.ToInt32(Enums.Depo.HamKumasDepo);
        public ObservableCollection<InventoryReceipt> Siparisler { get; set; }
        public UC_SiparisGirisi()
        {
            InitializeComponent();
            ButtonBar.CommandTarget = this;
            BaslangicVerileri();
            Siparisler = new ObservableCollection<InventoryReceipt>();
            this.DataContext = this;
        }
        void SetNewReceiptNo()
        {
            txtFisNo.Text = _orm.GetRecordNo("Receipt", "ReceiptNo", "ReceiptType", Convert.ToInt32(Enums.Receipt.Siparis));
        }
        void BaslangicVerileri()
        {
            dpTarih.SelectedDate = DateTime.Now;
            dpTermin.SelectedDate = DateTime.Now;
            SetNewReceiptNo();
        }
        void Temizle()
        {
            dpTarih.SelectedDate = DateTime.Now;
            dpTermin.SelectedDate = DateTime.Now;
            CompanyId = 0; Id = 0; txtYetkili.Text = string.Empty; txtFirmaUnvan.Text = string.Empty; txtVade.Text = string.Empty;
            SetNewReceiptNo();
        }
        private void btnGeri_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnIleri_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnSil_Click(object sender, RoutedEventArgs e)
        {

        }
        private T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);
            while (parent != null && !(parent is T))
                parent = VisualTreeHelper.GetParent(parent);
            return parent as T;
        }
        // TIKLANAN HÜCREYE AİT BİLGİLERİN İLGİLİ SATIRLARA YAZDIRILMASINDAN DEVAM EDİLECEK - 05.11.2025
        private void btnMalzemeListesi_Click(object sender, RoutedEventArgs e)
        {
            //var button = sender as Button;
            //if (button == null)
            //{

            //    MessageBox.Show("buton yok"); return;
            //}

            //var row = FindParent<DataGridRow>(button);
            //if (row == null)
            //{
            //    MessageBox.Show("DataGridRow bulunamadı");
            //    return;
            //}
            //var currentItem = row.Item as InventoryReceipt;
            //if (currentItem == null)
            //{
            //    MessageBox.Show("current item yok");
            //    return;
            //}

            //var win = new wins.winMalzemeListesi(Convert.ToInt32(Enums.Inventory.Kumas));
            //if (win.ShowDialog() == true)
            //{
            //    string secilenKod = win.Code;
            //    currentItem.InventoryCode = secilenKod;
            //    dgMalzemeListesi.Items.Refresh();
            //}

            //var siparis = row.Item as InventoryReceipt;
            //if (siparis == null)
            //{
            //    MessageBox.Show("InventoryReceipt null");
            //    return;
            //}

            //// Malzeme listesi aç
            //wins.winMalzemeListesi win = new wins.winMalzemeListesi(Convert.ToInt32(Enums.Inventory.Kumas));
            //if (win.ShowDialog() == true)
            //{
            //    siparis.InventoryCode = win.Code;
            //    siparis.InventoryName = win.Name;
            //}
        }

        private void btnRapor_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnFirmaListesi_Click(object sender, RoutedEventArgs e)
        {
            wins.winFirmaListesi win = new wins.winFirmaListesi();
            win.ShowDialog();
            if (win.SecimYapildi)
            {
                this.CompanyId = win.Id;
                txtFirmaUnvan.Text = win.FirmaUnvan;
            }
        }

        public void Yeni()
        {
            Temizle();
        }

        public void Kaydet()
        {
            var dict1 = new Dictionary<string, object>
            {
                {"Id",this.Id },{"ReceiptNo",txtFisNo.Text},{"ReceiptDate",dpTarih.SelectedDate.Value},{"CompanyId",this.CompanyId},{"Authorized",txtYetkili.Text},{"DuaDate",dpTermin.SelectedDate.Value},{"Maturity",txtVade.Text} ,{"ReceiptType",Convert.ToInt32(Enums.Receipt.Siparis)}, {"WareHouseId",DepoId}
            };
            this.Id = _orm.Save("Receipt", dict1);
            Bildirim.Bilgilendirme2("Kayıt işlemi başarılı bir şekilde gerçekleştirildi.");
        }

        public void Sil()
        {
            //throw new NotImplementedException();
        }

        public void Yazdir()
        {
            //throw new NotImplementedException();
        }

        public void Ileri()
        {
            //throw new NotImplementedException();
        }

        public void Geri()
        {
            //throw new NotImplementedException();
        }

        public void Listele()
        {
            wins.winFisHareketleriListesi win = new wins.winFisHareketleriListesi(0, Enums.Receipt.Siparis);
            win.ShowDialog();
            if (win.secimYapildi)
            {
                this.Id = win.Id;
                txtFisNo.Text = win.ReceiptNo;
                dpTarih.SelectedDate = win._Date;
                this.CompanyId = win.CompanyId;
                txtFirmaUnvan.Text = win.CompanyName;
                txtYetkili.Text = win.Authorized;
                dpTermin.SelectedDate = win.DuaDate;
                txtVade.Text = win.Maturity;
            }
        }
    }
}
