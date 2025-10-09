using MaliyeHesaplama.helpers;
using System.IO;
using System.Windows.Controls;

namespace MaliyeHesaplama.userControls
{
    public partial class UC_RaporOlusturma : UserControl
    {
        public UC_RaporOlusturma()
        {
            InitializeComponent();
            RaporDosyasiOlustur();
        }
        void RaporDosyasiOlustur() // rapor oluşturma işlemlerinden devam edilecek
        {
            string sourcedir = Path.Combine(Directory.GetCurrentDirectory(),"reports");
            string filepath = Path.Combine(sourcedir, "blank1.frx");
            if (File.Exists(filepath))
            {
                Bildirim.Bilgilendirme2("blank frx dosyasına erişildi");
            }
        }

        private void btnYeni_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void btnGeri_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void btnIleri_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void btnSil_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void btnKayit_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void btnListe_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
