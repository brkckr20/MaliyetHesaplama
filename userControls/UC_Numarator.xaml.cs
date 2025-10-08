using MaliyeHesaplama.helpers;
using System.Windows;
using System.Windows.Controls;

namespace MaliyeHesaplama.userControls
{
    public partial class UC_Numarator : UserControl
    {
        MiniOrm _orm = new MiniOrm();
        private int Id = 0;
        public UC_Numarator()
        {
            InitializeComponent();
        }

        private void btnKayit_Click(object sender, RoutedEventArgs e)
        {
            var dict = new Dictionary<string, object> {
                {"Id",Id },{"Prefix",txtOnEk.Text },{"Number",txtNumara.Text},{"Name",txtIsim.Text},{"IsActive", chckKullanimda.IsChecked.HasValue},{"InventoryType", cmbTur.SelectedIndex.ToString() }
            };
            if (txtOnEk.Text != string.Empty && txtNumara.Text != string.Empty && txtIsim.Text != string.Empty && cmbTur.SelectedIndex.ToString() != "0")
            {
                if (_orm.Save("Numerator", dict) > 0)
                {
                    Bildirim.Bilgilendirme2("Kayıt işlemi başarılı bir şekilde gerçekleştirildi");
                }
            }
            else
            {
                Bildirim.Uyari2("Kayıt işleminin yapılabilmesi için tüm (*) ile işaretlemniş alanları doldurunuz!");
            }
        }
        private void btnYeni_Click(object sender, RoutedEventArgs e)
        {
            Temizle();
        }
        void Temizle()
        {
            txtOnEk.Text = string.Empty;
            txtNumara.Text = string.Empty;
            txtIsim.Text = string.Empty;
            cmbTur.SelectedIndex = -1;
            chckKullanimda.IsChecked = true;
        }
        private void btnListe_Click(object sender, RoutedEventArgs e)
        {
            wins.winNumaratorListesi win = new wins.winNumaratorListesi(Enums.Inventory.Tumu);
            win.ShowDialog();
            if (win.SatirSecildi)
            {
                txtOnEk.Text = win.Prefix;
                txtNumara.Text = win.Number.ToString();
                txtIsim.Text = win.NameX;
                chckKullanimda.IsChecked = win.IsActive;
                cmbTur.SelectedIndex = win.InventoryType;
                this.Id = win.Id;
            }
        }

        private void btnSil_Click(object sender, RoutedEventArgs e)
        {
            if (_orm.Delete("Numerator", Id, true) > 0)
            {
                Temizle();
            }
        }
    }
}
