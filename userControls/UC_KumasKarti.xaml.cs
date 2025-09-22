using MaliyeHesaplama.models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace MaliyeHesaplama.userControls
{
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
        public class TransactionRow
        {
            public int Id { get; set; }
            public string SelectedTransaction { get; set; }
        }
        void BaslangicVerileri() // grid controlü kontrol et. verileri alt alta yazıyor olarak geldi
        {
            var data = new List<TransactionRow>();
            data.Add(new TransactionRow { Id = 1, SelectedTransaction = "Alım" });
            data.Add(new TransactionRow { Id = 2, SelectedTransaction = "Satım" });
            data.Add(new TransactionRow { Id = 3, SelectedTransaction = "Transfer" });

            dataGrid.ItemsSource = data;
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
