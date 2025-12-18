using MaliyeHesaplama.helpers;
using MaliyeHesaplama.models;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace MaliyeHesaplama.wins
{
    public partial class winRenkListesi : Window
    {
        MiniOrm _orm = new MiniOrm();
        private ICollectionView _collectionView;
        public int Id, CompanyId, ParentId, EmployeeId, Type;
        public string Kodu, Adi, Pantone, Doviz, Explanation;
        public bool SecimYapildi = false, IsParent, IsUse;
        public DateTime TalepTarihi, OkeyTarihi;
        public decimal Fiyat;
        FilterGridHelpers fgh;
        public winRenkListesi(bool IsVariant)
        {
            InitializeComponent();
            IsParent = IsVariant;
            fgh = new FilterGridHelpers(grid, "Renk Kartları Listesi", "gridRenkKartlari");
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var data = _orm.GetColorList<Color>(IsParent);
            _collectionView = CollectionViewSource.GetDefaultView(data);
            grid.ItemsSource = _collectionView;
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
        private void ExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            fgh.ExportToExcel();
        }
        private void grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var hiddenColumns = new[] { "CompanyId", "IsUse", "IsParent","EmployeeId" };
            fgh.GridGeneratingColumn(e, grid, hiddenColumns);
        }
        private void grid_ColumnReordered(object sender, DataGridColumnEventArgs e)
        {
            fgh.GridReOrdered(sender, e);
        }

        private void dgListe_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grid.SelectedItem != null)
            {
                this.SecimYapildi = true;
                dynamic record = grid.SelectedItem;
                Id = record.Id;
                CompanyId = record.CompanyId;
                ParentId = record.ParentId;
                TalepTarihi = record.RequestDate;
                OkeyTarihi = record.ConfirmDate;
                Adi = record.Name;
                Kodu = record.Code;
                Pantone = record.PantoneNo;
                Fiyat = record.Price;
                Doviz = record.Forex;
                Type = record.Type;
                IsUse = record.IsUse;
                Explanation = record.Explanation;
                this.DialogResult = true;
                this.Close();
            }
        }
    }
}
