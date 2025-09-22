using MaliyeHesaplama.models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace MaliyeHesaplama.userControls
{
    // malzeme kodu alanına button edit mantığını ekle. (Redux Toolkit ile Veri Güncelleme) burdan devam - 18.09.2025
    public partial class UC_KumasKarti : UserControl
    {
        MiniOrm _orm = new MiniOrm();
        public ObservableCollection<string> OperasyonTipleri { get; set; }
        public ObservableCollection<InventoryReceipt> Recete { get; set; }
        public UC_KumasKarti()
        {
            InitializeComponent();
            BaslangicVerileri();

            this.DataContext = this;
        }
        void BaslangicVerileri()
        {
            string raw = Convert.ToString(_orm.GetById<dynamic>("ProductionManagementParams", 1).ReceteOperasyonTipleri);

            OperasyonTipleri = new ObservableCollection<string>(
                raw.Split(',').Select(x => x.Trim()).ToList()
            );

            var data = new ObservableCollection<Kalem>
            {
                new Kalem()
            };

            myDataGrid.ItemsSource = data;
        }
        public class Kalem
        {
            public string Operasyon { get; set; }
        }

        private void btnYeni_Click(object sender, RoutedEventArgs e)
        {

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

        private void btnKayit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnListe_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
