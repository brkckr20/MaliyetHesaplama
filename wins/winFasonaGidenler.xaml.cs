using MaliyeHesaplama.helpers;
using MaliyeHesaplama.models;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace MaliyeHesaplama.wins
{
    public partial class winFasonaGidenler : Window
    {
        string _condition, _calculateField, _windowName;
        private IEnumerable<Receipt> _tumHareketler;
        MiniOrm _orm = new MiniOrm();
        private ICollectionView _collectionView;
        FilterGridHelpers fgh;
        public List<Receipt> SelectedReceipts { get; private set; } = new();
        public winFasonaGidenler(string condition, string depoId, string calculateField, string windowName)
        {
            InitializeComponent();
            _condition = condition;
            _calculateField = calculateField;
            _windowName = windowName;
            Title = _windowName;
            fgh = new FilterGridHelpers(gridFG, "Fasona Gidenler " + depoId, "gridFasonaGidenler_" + depoId);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _tumHareketler = _orm.GetMovementListWithQuantities<Receipt>(_condition, _calculateField);
            _collectionView = CollectionViewSource.GetDefaultView(_tumHareketler);
            gridFG.ItemsSource = _collectionView;

            Dispatcher.BeginInvoke(new Action(() =>
            {
                fgh.InitializeColumnSettings();
                fgh.LoadColumnSettingsFromDatabase();
            }), System.Windows.Threading.DispatcherPriority.Loaded);
        }

        private void gridFG_AutoGeneratingColumn(object sender, System.Windows.Controls.DataGridAutoGeneratingColumnEventArgs e)
        {
            var hiddenColumns = new[] { "ReceiptType", "CompanyId", "WareHouseId" };
            fgh.GridGeneratingColumn(e, gridFG, hiddenColumns);
        }

        private void gridFG_ColumnReordered(object sender, System.Windows.Controls.DataGridColumnEventArgs e)
        {
            fgh.GridReOrdered(sender, e);
        }

        private void kolSec_Click(object sender, RoutedEventArgs e)
        {
            fgh.OpenColumnsForm(this);
        }

        private void expExcel_Click(object sender, RoutedEventArgs e)
        {
            fgh.ExportToExcel();
        }

        private void btnAktar_Click(object sender, RoutedEventArgs e)
        {
            SelectedReceipts = gridFG?.SelectedItems?.OfType<Receipt>().ToList() ?? new List<Receipt>();

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
