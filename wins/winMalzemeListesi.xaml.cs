using MaliyeHesaplama.helpers;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using MaliyeHesaplama.models;

namespace MaliyeHesaplama.wins
{
    public partial class winMalzemeListesi : Window
    {
        public int _inventoryType, Id;
        public string Code, Name, RawWidth, RawHeight, ProdWidth, ProdHeight, RawGrammage, ProdGrammage,  Explanation;
        string ScreenName;

        private void mColChooser_Click(object sender, RoutedEventArgs e)
        {
            winKolonSecici win = new winKolonSecici(ScreenName, Properties.Settings.Default.RememberUserId);
            win.Show();
        }

        public bool YarnDyed;

        private void FilterDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            e.Column.Header = e.PropertyName;
        }

        private void grid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (grid.SelectedItem != null)
            {
                dynamic record = grid.SelectedItem;
                Id = record.Id;
                Code = record.InventoryCode;
                Name = record.InventoryName;
                RawWidth = record.RawWidth == null ? "" : record.RawWidth.ToString();
                RawHeight = record.RawHeight == null ? "" : record.RawHeight.ToString();
                ProdWidth = record.ProdWidth == null ? "" : record.ProdWidth.ToString();
                ProdHeight = record.ProdHeight == null ? "" : record.ProdHeight.ToString();
                RawGrammage = record.RawGrammage == null ? "" : record.RawGrammage.ToString();
                ProdGrammage = record.ProdGrammage == null ? "" : record.ProdGrammage.ToString();
                YarnDyed = Convert.ToBoolean(record.YarnDyed);
                Explanation = record.Explanation == null ? "" : record.Explanation.ToString();
                this.DialogResult = true;
                this.Close();
            }
        }

        private ICollectionView collectionView;
        MiniOrm _orm = new MiniOrm();
        public winMalzemeListesi(int InventoryType)
        {
            InitializeComponent();
            _inventoryType = InventoryType;
            var data = _orm.GetAll<Inventory>("Inventory").Where(x => x.Type == InventoryType && x.IsPrefix == false).ToList();
            collectionView = CollectionViewSource.GetDefaultView(data);
            grid.ItemsSource = collectionView;
            switch (_inventoryType)
            {
                case 1:
                    Title = "Kumaş Kartı Listesi";
                    this.ScreenName = "Kumaş Kartı Listesi";
                    break;
                case 2:
                    Title = "İplik Kartı Listesi";
                    this.ScreenName = "İplik Kartı Listesi";
                    break;
                default:
                    break;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HiddenFields();
        }

        private void HiddenFields()
        {
            //if (_inventoryType == 2)
            //{
            //    colHamBoy.Visibility = Visibility.Hidden;
            //    colMamulBoy.Visibility = Visibility.Hidden;
            //    colMamulEn.Visibility = Visibility.Hidden;
            //    colHamEn.Visibility = Visibility.Hidden;
            //    spHamBoy.Visibility = Visibility.Hidden;
            //    spMamulBoy.Visibility = Visibility.Hidden;
            //    spMamulEn.Visibility = Visibility.Hidden;
            //    spHamEn.Visibility = Visibility.Hidden;
            //}
        }
    }
}
