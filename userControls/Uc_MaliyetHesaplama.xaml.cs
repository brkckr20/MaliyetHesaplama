﻿using MaliyeHesaplama.helpers;
using System.Windows.Controls;
using System.Windows;

namespace MaliyeHesaplama.userControls
{
    public partial class Uc_MaliyetHesaplama : UserControl
    {
        int Id, InventoryId, CompanyId, CPIId;
        bool _receteOlacak = false;
        MiniOrm _orm = new MiniOrm();
        public Uc_MaliyetHesaplama()
        {
            InitializeComponent();
            dpTarih.SelectedDate = DateTime.Now;
            var _parametreler = _orm.GetById<dynamic>("ProductionManagementParams", 1);
            _receteOlacak = _parametreler.KumasRecetesiOlacak;
            dckRecete.Visibility = _receteOlacak ? Visibility.Visible : Visibility.Collapsed;
            txtFisNo.Text = _orm.GetRecordNo("Cost", "OrderNo", "Type", 1);
        }

        private void btnFirmaListesi_Click(object sender, RoutedEventArgs e)
        {
            wins.winFirmaListesi win = new wins.winFirmaListesi();
            win.ShowDialog();
            if (win.FirmaKodu != null)
            {
                this.CompanyId = win.Id;
                txtFirmaKodu.Text = win.FirmaKodu;
                txtFirmaUnvan.Content = win.FirmaUnvan;
            }
        }

        private void btnMalzemeListesi_Click(object sender, RoutedEventArgs e)
        {
            wins.winMalzemeListesi win = new wins.winMalzemeListesi(Convert.ToInt32(Enums.Inventory.Kumas));
            win.ShowDialog();
            if (win.Code != null)
            {
                this.InventoryId = win.Id;
                txtMalzemeKodu.Text = win.Code;
                lblMalzemeAdi.Content = win.Name;
            }
        }

        private void btnKayit_Click(object sender, RoutedEventArgs e)
        {
            var dict1 = new Dictionary<string, object>
            {
                { "Id", Id }, {"CompanyId",CompanyId}, {"Date",dpTarih.SelectedDate.Value}, {"InventoryId",this.InventoryId},{"OrderNo",txtFisNo.Text }
            };
            if (Id == 0) // bu alanlara kayıt eden güncelleyen vs diğer bilgiler eklenebilir
            {
                dict1.Add("InsertedDate", DateTime.Now);
            }
            else
            {
                dict1.Add("UpdatedDate", DateTime.Now);
            }
            Id = _orm.Save("Cost", dict1);
            var dict2 = new Dictionary<string, object>
            {
                {"Id", CPIId },{"CostId",Id}, {"YI_Warp1", R(txtCozgu1IpBilBolen)}, {"YI_Warp1Divider",R(txtCozgu1IpBilBolunen)}, {"YI_Warp1Result",R(txtCozgu1IpBilSonuc)}, {"YI_Warp2", R(txtCozgu2IpBilBolen)}, {"YI_Warp2Divider",R(txtCozgu2IpBilBolunen)}, {"YI_Warp2Result",R(txtCozgu2IpBilSonuc)}, {"YI_Scarf1", R(txtAtki1IpBilBolen)}, {"YI_Scarf1Divider",R(txtAtki1IpBilBolunen)}, {"YI_Scarf1Result",R(txtAtki1IpBilSonuc)}, {"YI_Scarf2", R(txtAtki2IpBilBolen)}, {"YI_Scarf2Divider",R(txtAtki2IpBilBolunen)}, {"YI_Scarf2Result",R(txtAtki2IpBilSonuc)}, {"YI_Scarf3", R(txtAtki3IpBilBolen)}, {"YI_Scarf3Divider",R(txtAtki3IpBilBolunen)}, {"YI_Scarf3Result",R(txtAtki3IpBilSonuc)}, {"YI_Scarf4", R(txtAtki4IpBilBolen)}, {"YI_Scarf4Divider",R(txtAtki4IpBilBolunen)}, {"YI_Scarf4Result",R(txtAtki4IpBilSonuc)} ,{"D_Warp1", R(txtCozgu1Siklik)},{"D_Warp2", R(txtCozgu2Siklik)},{"D_Scarf1", R(txtAtki1Siklik)},{"D_Scarf2", R(txtAtki2Siklik)},{"D_Scarf3", R(txtAtki3Siklik)},{"D_Scarf4", R(txtAtki4Siklik)},{"WI_CombNo1",R(txtTarakNo1Carpan)},{"WI_CombNo1Multiplier",R(txtTarakNo1Carpim)},{"WI_CombNo1Result",R(txtTarakNo1Sonuc)},{"WI_CombNo2",R(txtTarakNo2Carpan)},{"WI_CombNo2Multiplier",R(txtTarakNo2Carpim)},{"WI_CombNo2Result",R(txtTarakNo2Sonuc)},{"WI_CombWidth",R(txtTarakEn)},{"WI_RawHeight",R(txtHamBoy)},{"WI_HeightEaves",R(txtBoySacakText)},{"WI_WidthEaves",R(txtEnSacakText)},{"WI_RawWidth",R(txtHamEn)},{"WI_ProductHeight",R(txtMamulBoy)},{"WI_ProductWidth",R(txtMamulEn)},{"NW_Warp1",R(txtCozgu1TelSay)},{"NW_Warp2",R(txtCozgu2TelSay)},{"NW_Scarf1",R(txtAtki1TelSay)},{"NW_Scarf2",R(txtAtki2TelSay)},{"NW_Scarf3",R(txtAtki3TelSay)},{"NW_Scarf4",R(txtAtki4TelSay)}
                // üretim hesaplamadan devam edilecek - 02.10.2025
            };
            CPIId = _orm.Save("CostProductionInformation", dict2);
            Bildirim.Bilgilendirme2("Veri kayıt işlemi başarıyla gerçekleştirildi");
        }

        string R(TextBox tb) // kayıt esnasında hata alındığı için virgüllü değerler nokta ile değiştirildi. -- replace metorunu kullanıyor
        {
            return tb.Text.Replace(',', '.');
        }
    }
}
