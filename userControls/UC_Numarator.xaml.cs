using MaliyeHesaplama.helpers;
using System.Data;
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

        private void btnKayit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var dict = new Dictionary<string, object> {
                {"Id",Id },{"Prefix",txtOnEk.Text },{"Number",txtNumara.Text},{"Name",txtIsim.Text},{"IsActive", chckKullanimda.IsChecked.HasValue},{"InventoryType", cmbTur.SelectedIndex.ToString() }
            };
            if (txtOnEk.Text != string.Empty && txtNumara.Text != string.Empty && txtIsim.Text != string.Empty && cmbTur.SelectedIndex.ToString() != "0")
            {
                if (_orm.Save("Numerator", dict) > 0)
                {
                    Bildirim.Bilgilendirme("Kayıt işlemi başarılı bir şekilde gerçekleştirildi");
                }
            }
            else
            {
                Bildirim.Uyari("Kayıt işleminin yapılabilmesi için tüm (*) ile işaretlemniş alanları doldurunuz!");
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
            wins.winNumaratorListesi win = new wins.winNumaratorListesi();
            win.ShowDialog();
            if (win.SatirSecildi)
            {
                txtOnEk.Text = win.OnEk;
                txtNumara.Text = win.Numara.ToString();
                txtIsim.Text = win.Isim;
                chckKullanimda.IsChecked = win.Kullanimda;
                cmbTur.SelectedIndex = win.Tur;
            }
        }

        private void btnSil_Click(object sender, RoutedEventArgs e)
        {
            //kontrol et -- 15.09.2025
            //if (Bildirim.SilmeOnayi())
            //{
            //    if (_orm.Delete("Numerator", Id) > 0)
            //    {
            //        Bildirim.SilmeBasarili();
            //        Temizle();
            //    }
            //}
        }
    }
}
