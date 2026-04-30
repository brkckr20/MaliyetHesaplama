using System.Collections.Generic;
using System.Windows;

namespace MaliyeHesaplama.v2.Windows
{
    public partial class winFasonGidenlerListesiV2 : Window
    {
        public List<dynamic> SecilenSatirlar { get; private set; } = new List<dynamic>();
        private int _companyId;
        private string _companyCode;
        private string _companyName;

        public winFasonGidenlerListesiV2()
        {
            InitializeComponent();
        }

        public winFasonGidenlerListesiV2(int companyId, string companyCode, string companyName)
        {
            InitializeComponent();
            _companyId = companyId;
            _companyCode = companyCode;
            _companyName = companyName;
            LoadData();
        }

        private void LoadData()
        {
            var repo = new Data.ReceiptRepository();
            var items = repo.GetFasonGidenler(_companyId);
            grid.ItemsSource = items;
        }

        private void btnAktar_Click(object sender, RoutedEventArgs e)
        {
            var selected = grid.SelectedItems;
            if (selected.Count == 0)
            {
                System.Windows.MessageBox.Show("Lütfen en az bir satır seçin.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            foreach (dynamic item in selected)
            {
                SecilenSatirlar.Add(item);
            }

            this.DialogResult = true;
            this.Close();
        }

        private void btnVazgec_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}