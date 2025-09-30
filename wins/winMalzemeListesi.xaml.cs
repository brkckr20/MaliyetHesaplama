﻿using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace MaliyeHesaplama.wins
{
    public partial class winMalzemeListesi : Window
    {
        public int _inventoryType, Id;
        public string Code, Name, RawWidth, RawHeight, ProdWidth, ProdHeight, RawGrammage, ProdGrammage,  Explanation;
        public bool YarnDyed;
        private ICollectionView collectionView;
        MiniOrm _orm = new MiniOrm();
        public winMalzemeListesi(int InventoryType)
        {
            InitializeComponent();
            _inventoryType = InventoryType;
            var data = _orm.GetAll<dynamic>("Inventory");
            txtAdi.Focus();
            collectionView = CollectionViewSource.GetDefaultView(data);
            collectionView.Filter = FilterItems;
            sfDataGrid.ItemsSource = collectionView;
            switch (_inventoryType)
            {
                case 1:
                    Title = "Kumaş Kartı Listesi";
                    break;
                case 2:
                    Title = "İplik Kartı Listesi";
                    break;
                default:
                    break;
            }
        }
        private bool FilterItems(object obj)
        {
            dynamic item = obj;

            if (item.Type != _inventoryType || item.IsPrefix)
                return false;
            bool matchesCode = string.IsNullOrEmpty(txtKodu.Text) || (item.InventoryCode?.ToLower().Contains(txtKodu.Text.ToLower()) ?? false);
            bool matchesName = string.IsNullOrEmpty(txtAdi.Text) || (item.InventoryName?.ToLower().Contains(txtAdi.Text.ToLower()) ?? false);
            bool matchesRawWidth = string.IsNullOrEmpty(txtHamEn.Text) || (item.RawWidth != null && item.RawWidth.ToString().Contains(txtHamEn.Text));
            bool matchesRawHeight = string.IsNullOrEmpty(txtHamBoy.Text) || (item.RawHeight != null && item.RawHeight.ToString().Contains(txtHamBoy.Text));
            bool matchesProdWith = string.IsNullOrEmpty(txtMamulEn.Text) || (item.ProdWidth != null && item.ProdWidth.ToString().Contains(txtMamulEn.Text));
            bool matchesProdHeight = string.IsNullOrEmpty(txtMamulBoy.Text) || (item.ProdHeight != null && item.ProdHeight.ToString().Contains(txtMamulBoy.Text));

            return matchesCode && matchesName && matchesRawWidth && matchesRawHeight && matchesProdWith && matchesProdHeight;
        }

        private void txtKodu_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            collectionView.Refresh();
        }

        private void txtAdi_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            collectionView.Refresh();
        }

        private void sfDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sfDataGrid.SelectedItem != null)
            {
                dynamic record = sfDataGrid.SelectedItem;
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
                this.Close();
            }
        }
    }
}
