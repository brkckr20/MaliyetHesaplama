using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MaliyeHesaplama.helpers;

namespace MaliyeHesaplama.v2.Windows
{
    public partial class winFasonGidenlerListesiV2 : Window
    {
        public List<dynamic> SecilenSatirlar { get; private set; } = new List<dynamic>();
        private int _companyId;
        private string _companyCode;
        private string _companyName;
        private FilterGridHelpers fgh;

        public winFasonGidenlerListesiV2()
        {
            InitializeComponent();
            fgh = new FilterGridHelpers(grid, "Fason Gidenler", "gridFasonGidenler");
        }

        public winFasonGidenlerListesiV2(int companyId, string companyCode, string companyName)
        {
            InitializeComponent();
            _companyId = companyId;
            _companyCode = companyCode;
            _companyName = companyName;
            fgh = new FilterGridHelpers(grid, "Fason Gidenler", "gridFasonGidenler");
        }

        private void grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var hiddenColumns = new[] { "Id", "InventoryId", "CompanyId" };
            fgh.GridGeneratingColumn(e, grid, hiddenColumns);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_companyId > 0)
            {
                LoadData();
            }
            Dispatcher.BeginInvoke(new Action(() =>
            {
                fgh.InitializeColumnSettings();
                fgh.LoadColumnSettingsFromDatabase();
            }), System.Windows.Threading.DispatcherPriority.Loaded);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            fgh.OpenColumnsForm(this);
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