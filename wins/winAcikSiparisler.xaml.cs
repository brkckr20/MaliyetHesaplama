using MaliyeHesaplama.helpers;
using MaliyeHesaplama.models;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;

namespace MaliyeHesaplama.wins
{
    public partial class winAcikSiparisler : Window
    {
        string _condition;
        private IEnumerable<Receipt> _tumHareketler;
        MiniOrm _orm = new MiniOrm();
        private ICollectionView _collectionView;
        FilterGridHelpers fgh;
        public List<Receipt> SelectedReceipts { get; private set; } = new();
        public winAcikSiparisler(string condition)
        {
            InitializeComponent();
            _condition= condition;
            fgh = new FilterGridHelpers(grid, "Açık Siparişler", "gridAcikSiparisler");
        }
        private void grid_AutoGeneratingColumn(object sender, System.Windows.Controls.DataGridAutoGeneratingColumnEventArgs e)
        {
            var hiddenColumns = new[] { "ReceiptType", "CompanyId", "WareHouseId" };
            fgh.GridGeneratingColumn(e, grid, hiddenColumns);
        }

        private void grid_ColumnReordered(object sender, System.Windows.Controls.DataGridColumnEventArgs e)
        {
            fgh.GridReOrdered(sender, e);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            fgh.OpenColumnsForm(this);
        }

        private void ExportToExcel(object sender, RoutedEventArgs e)
        {
            fgh.ExportToExcel();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _tumHareketler = _orm.GetMovementListWithQuantities<Receipt>(_condition);
            _collectionView = CollectionViewSource.GetDefaultView(_tumHareketler);
            grid.ItemsSource = _collectionView;

            Dispatcher.BeginInvoke(new Action(() =>
            {
                fgh.InitializeColumnSettings();
                fgh.LoadColumnSettingsFromDatabase();
            }), System.Windows.Threading.DispatcherPriority.Loaded);
        }

        private void btnAktar_Click(object sender, RoutedEventArgs e)
        {
            SelectedReceipts = grid?.SelectedItems?.OfType<Receipt>().ToList() ?? new List<Receipt>();

            if (SelectedReceipts.Count == 0)
            {
                Bildirim.Uyari2("Lütfen en az bir satır seçiniz.");
                return;
            }

            // Dialog'u başarılı olarak kapat
            DialogResult = true;
        }
    }
}
