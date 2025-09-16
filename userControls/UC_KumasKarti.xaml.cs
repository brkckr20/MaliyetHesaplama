using MaliyeHesaplama.models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace MaliyeHesaplama.userControls
{
    // gridcontrol üzerinde combobox olayını araştır. - 16.09.2025
    public partial class UC_KumasKarti : UserControl
    {
        public ObservableCollection<InventoryReceipt> InventoryReceipts { get; set; }
        public UC_KumasKarti()
        {
            InitializeComponent();
            InventoryReceipts = new ObservableCollection<InventoryReceipt>();
            this.DataContext = this;
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
