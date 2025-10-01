using MaliyeHesaplama.helpers;
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
                {"Id", CPIId },{"CostId",Id}
            };
            CPIId = _orm.Save("CostProductionInformation", dict2);
            Bildirim.Bilgilendirme2("Veri kayıt işlemi başarıyla gerçekleştirildi");

        }
    }
}
