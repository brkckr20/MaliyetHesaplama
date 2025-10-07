using MaliyeHesaplama.helpers;
using System.Windows.Controls;
using System.Windows;

namespace MaliyeHesaplama.userControls
{
    public partial class Uc_MaliyetHesaplama : UserControl
    {
        int Id, InventoryId, CompanyId, CPIId, CPCId, CCCId;
        bool _receteOlacak = false;
        MiniOrm _orm = new MiniOrm();
        // diğer alanların listeden seçildikten sonra ilgili controllere yansıtılması işlemini kontrol et - 07.10.2025
        private void btnListe_Click(object sender, RoutedEventArgs e)
        {
            wins.winMaliyetCalismasiListesi win = new wins.winMaliyetCalismasiListesi();
            win.ShowDialog();
            this.Id = win.Id;
            txtFisNo.Text = win.OrderNo;
            dpTarih.SelectedDate = win.Date;
            this.CompanyId = win.CompanyId;
            txtFirmaKodu.Text = win.CompanyCode;
            txtFirmaUnvan.Content = win.CompanyName;
            this.InventoryId = win.InventoryId;
            txtMalzemeKodu.Text = win.InventoryCode;
            lblMalzemeAdi.Content = win.InventoryName;
        }

        private void btnSil_Click(object sender, RoutedEventArgs e)
        {
            if (_orm.Delete("Cost", this.Id, true) > 0)
            {
                _orm.Delete("CostCostCalculate", Id, false, "CostId");
                _orm.Delete("CostProductionCalculate", Id, false, "CostId");
                _orm.Delete("CostProductionInformation", Id, false, "CostId");
                txtFisNo.Text = _orm.GetRecordNo("Cost", "OrderNo", "Type", 1);
                FormVerileriniTemizle();
            }
        }

        private void cmRaporAc_Click(object sender, RoutedEventArgs e)
        {

        }
        void FormVerileriniTemizle()
        {
            MainHelper.SetControls(new Dictionary<Control, object>
            {
                { txtCozgu1IpBilBolen,"1" },{ txtCozgu1IpBilBolunen,"1" },{ txtCozgu2IpBilBolen,"1" },{ txtCozgu2IpBilBolunen,"1" },{ txtAtki1IpBilBolen,"1" },{ txtAtki1IpBilBolunen,"1" },{ txtAtki2IpBilBolen,"1" },{ txtAtki2IpBilBolunen,"1" },{ txtAtki3IpBilBolen,"1" },{ txtAtki3IpBilBolunen,"1" },{ txtAtki4IpBilBolen,"1" },{ txtAtki4IpBilBolunen,"1" },{txtAtki1Siklik,"0" },{txtAtki2Siklik,"0" },{txtAtki3Siklik,"0" },{txtAtki4Siklik,"0" },{txtTarakNo1Carpan,"0" },{txtTarakNo1Carpim,"0" },{txtTarakNo2Carpan,"0" },{txtTarakNo2Carpim,"0" },{txtTarakEn,"0" },{txtHamBoy,"0" },{txtBoySacakText,"0" }, {txtEnSacakText,"0" },{txtMamulBoy,"0" }, {txtMamulEn,"0" }, {txtCozgu1IpBoyText,"0" }, {txtCozgu2IpBoyText,"0" }, {txtAtki1IpBoyText,"0" }, {txtAtki2IpBoyText,"0" }, {txtAtki3IpBoyText,"0" }, {txtAtki4IpBoyText,"0" }, {txtCozgu1IpFiyText,"0" }, {txtCozgu2IpFiyText,"0" }, {txtAtki1IpFiyText,"0" }, {txtAtki2IpFiyText,"0" }, {txtAtki3IpFiyText,"0" }, {txtAtki4IpFiyText,"0" }, {txtAtkiUrFiyText,"0" }, {txtCozguUrFiyText,"0" }, {txtParcaYikamaUrFiyText,"0" }, {txtKumasBoyamaUrFiyText,"0" }, {txtDokumaFiresiUrFiyText,"0" }, {txtBoyaFiresiUrFiyText,"0" },{txtKonfMaliyetiUrFiyText,"0" },{txtIkinciKaliyeMaliyetiUrFiyText,"0" },{txtKarUrFiyText,"0" },{txtKdvUrFiyText,"0" },{txtKurUrFiyText,_orm.GetEURCurrency()},{txtPariteUrFiyText,"0" },{txtEurUrFiyText,"0" },{txtBelirlenenFiyatText,"0" },
                {txtFirmaKodu,"" },{txtFirmaUnvan,"" },{txtMalzemeKodu,"" },{lblMalzemeAdi,"" },
            });
            Id = 0; InventoryId = 0; CompanyId = 0; CPIId = 0; CPCId = 0; CCCId = 0;
            txtFisNo.Text = _orm.GetRecordNo("Cost", "OrderNo", "Type", 1);
        }
        private void btnYeni_Click(object sender, RoutedEventArgs e)
        {
            FormVerileriniTemizle();
        }

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
            };
            CPIId = _orm.Save("CostProductionInformation", dict2);
            var dict3 = new Dictionary<string, object>
            {
                {"Id",CPCId },{"CostId",Id},{"WC_Warp1", txtCozgu1Gramaj.Text},{"WC_Warp2", txtCozgu2Gramaj.Text},{"WC_Scarf1", txtAtki1Gramaj.Text},{"WC_Scarf2", txtAtki2Gramaj.Text},{"WC_Scarf3", txtAtki3Gramaj.Text},{"WC_Scarf4", txtAtki4Gramaj.Text},{"WC_Total", txtToplamGramaj.Text},{"YD_Warp1", R(txtCozgu1IpBoyText)},{"YD_Warp2", R(txtCozgu2IpBoyText)},{"YD_Scarf1", R(txtAtki1IpBoyText)},{"YD_Scarf2", R(txtAtki2IpBoyText)},{"YD_Scarf3", R(txtAtki3IpBoyText)},{"YD_Scarf4", R(txtAtki4IpBoyText)},{"YD_Result", R(txtIpBoySonuc)},{"YP_Warp1", R(txtCozgu1IpFiyText)},{"YP_Warp2", R(txtCozgu2IpFiyText)},{"YP_Scarf1", R(txtAtki1IpFiyText)},{"YP_Scarf2", R(txtAtki2IpFiyText)},{"YP_Scarf3", R(txtAtki3IpFiyText)},{"YP_Scarf4", R(txtAtki4IpFiyText)},{"YC_Warp1", txtCozgu1IpMal.Text},{"YC_Warp2", txtCozgu2IpMal.Text},{"YC_Scarf1", txtAtki1IpMal.Text},{"YC_Scarf2", txtAtki2IpMal.Text},{"YC_Scarf3", txtAtki3IpMal.Text},{"YC_Scarf4", txtAtki4IpMal.Text},{"YC_Result", txtToplamIpMal.Text}
            };
            CPCId = _orm.Save("CostProductionCalculate", dict3);
            var dict4 = new Dictionary<string, object>
            {
                {"Id",CCCId },{"CostId",Id},{"PP_Scarf",R(txtAtkiUrFiyText)},{"PP_Warp",R(txtCozguUrFiyText)},{"PP_PartsWashing",R(txtParcaYikamaUrFiyText)},{"PP_FabricWashing",R(txtKumasBoyamaUrFiyText)},{"PP_WeavingWaste",R(txtDokumaFiresiUrFiyText)},{"PP_DyehouseWaster",R(txtBoyaFiresiUrFiyText)},{"PP_GarmentCost",R(txtKonfMaliyetiUrFiyText)},{"PP_2QualityCost",R(txtIkinciKaliyeMaliyetiUrFiyText)},{"PP_Profit",R(txtKarUrFiyText)},{"PP_Vat",R(txtKdvUrFiyText)},{"PP_Currency",R(txtKurUrFiyText)},{"PP_Parity",R(txtPariteUrFiyText)},{"PP_Euro",R(txtEurUrFiyText)},{"WC_Weaving",R(txtDokumaDokMal)},{"WC_Warp",R(txtCozguDokMal)},{"WC_YarnCost",R(txtIplikMaliyetDokMal)},{"PC_Total",R(txtToplamUrMal)},{"PC_Wasted",R(txtFireliUrMal)},{"RFC_ProfitableForex",R(txtKarliHamKumMal)},{"RFC_Profitable",R(txtKarliHamKumMalTL)},{"WDC_PartsWashing",R(txtParcaYikamaYBM)},{"WDC_DyedFabric",R(txtBoyanmisKumasYBM)},{"WDC_DyedFabricTL",R(txtBoyanmisKumasTlYBM)},{"WDC_Wasted",R(txtFireliYBM)},{"WDC_ProfitableForex",R(txtKarliYBM)},{"SP_DyedFabric",R(txtBoyaliKumasDikUr)},{"SP_GarmentCost",R(txtKonfMaliyetiDikUr)},{"SP_2QualityCost",R(txtIkinciKaliteMaliyetDikUr)},{"SP_ProfitableForex",R(txtKarliDikUr)},{"SP_Profitable",R(txtKarliTLDikUr)},{"SP_VatIncludeForex",R(txtKdvliDikUr)},{"SP_VatInclude",R(txtKdvliTLDikUr)},{"PriceDeterminedForex",R(txtBelirlenenFiyatText)},{"PriceDetermined",R(txtBelirlenenFiyatTL)},{"VatIncludedPriceForex",R(txtKdvliBelirlenFiyat)},{"VatIncluded",R(txtKdvliBelirlenenFiyatTL)}
            };
            CCCId = _orm.Save("CostCostCalculate", dict4);
            Bildirim.Bilgilendirme2("Veri kayıt işlemi başarıyla gerçekleştirildi");
        }

        string R(TextBox tb) // kayıt esnasında hata alındığı için virgüllü değerler nokta ile değiştirildi. -- replace metodunu kullanıyor
        {
            return tb.Text.Replace(',', '.');
        }
    }
}
