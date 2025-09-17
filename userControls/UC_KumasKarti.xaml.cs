using MaliyeHesaplama.models;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace MaliyeHesaplama.userControls
{
    // listelemeyi kontrol et. - 16.09.2025
    public partial class UC_KumasKarti : UserControl
    {
        public ObservableCollection<InventoryReceipt> InventoryReceipts { get; set; }
        public UC_KumasKarti()
        {
            InitializeComponent();
            InventoryReceipts = new ObservableCollection<InventoryReceipt>
            {
                new InventoryReceipt { Id = 1, Genus = "Kumaş", Quantity = 25 },
                new InventoryReceipt { Id = 2, Genus = "İplik", Quantity = 25 },
                new InventoryReceipt { Id = 3, Genus = "Aksesuar", Quantity = 25 },
            };
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
