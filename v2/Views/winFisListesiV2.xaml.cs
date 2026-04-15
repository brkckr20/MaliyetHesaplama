using System.Windows;
using System.Windows.Input;
using MaliyeHesaplama.v2.Data;
using MaliyeHesaplama.v2.Models;

namespace MaliyeHesaplama.v2.Views
{
    public partial class winFisListesiV2 : Window
    {
        private readonly ReceiptRepository _repo;
        public int SecilenId { get; private set; }
        private readonly int _receiptType;

        public winFisListesiV2(int receiptType)
        {
            InitializeComponent();
            _repo = new ReceiptRepository();
            _receiptType = receiptType;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var data = _repo.GetByTypeList(_receiptType, 100);
            grid.ItemsSource = data;
        }

        private void grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grid.SelectedItem != null)
            {
                dynamic secilen = grid.SelectedItem;
                SecilenId = secilen.Id;
                this.DialogResult = true;
                this.Close();
            }
        }

        private void btnSec_Click(object sender, RoutedEventArgs e)
        {
            if (grid.SelectedItem != null)
            {
                dynamic secilen = grid.SelectedItem;
                SecilenId = secilen.Id;
                this.DialogResult = true;
                this.Close();
            }
        }

        private void btnIptal_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}