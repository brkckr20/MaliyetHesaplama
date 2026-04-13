using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MaliyeHesaplama.Interfaces;
using MaliyeHesaplama.v2.Data;

namespace MaliyeHesaplama.v2.Views
{
    public partial class UC_MalzemeKartiV2 : System.Windows.Controls.UserControl, IPageCommands
    {
        private readonly MaterialRepository _repo;
        private int _currentId = 0;

        public UC_MalzemeKartiV2()
        {
            InitializeComponent();
            _repo = new MaterialRepository();
            ButtonBar.PageCommands = this;
            Yeni();
        }

        private void LoadRecord(dynamic record)
        {
            _currentId = record.Id;
            txtKodu.Text = record.Code ?? "";
            txtAdi.Text = record.Name ?? "";
            cmbTip.SelectedIndex = (record.Type ?? 1) - 1;
            txtBirim.Text = record.UnitId?.ToString() ?? "";
            txtBarkod.Text = record.Barcode ?? "";
            
            var kdv = record.VatRate ?? 18;
            foreach (ComboBoxItem item in cmbKDV.Items)
            {
                if (item.Tag?.ToString() == kdv.ToString())
                {
                    cmbKDV.SelectedItem = item;
                    break;
                }
            }

            txtMinStok.Text = record.MinStock?.ToString() ?? "";
            txtMaxStok.Text = record.MaxStock?.ToString() ?? "";
            chkKullanimda.IsChecked = record.IsActive;
        }

        private void btnKoduListele_Click(object sender, RoutedEventArgs e)
        {
            var pencere = new winMalzemeListesiV2();
            pencere.Owner = Window.GetWindow(this);
            if (pencere.ShowDialog() == true)
            {
                var record = _repo.GetById(pencere.SecilenId);
                if (record != null)
                    LoadRecord(record);
            }
        }

        public void Yeni()
        {
            _currentId = 0;
            txtKodu.Text = "";
            txtAdi.Text = "";
            cmbTip.SelectedIndex = 0;
            txtBirim.Text = "";
            txtBarkod.Text = "";
            cmbKDV.SelectedIndex = 3;
            txtMinStok.Text = "";
            txtMaxStok.Text = "";
            chkKullanimda.IsChecked = true;
            txtKodu.Focus();
        }

        public void Kaydet()
        {
            if (string.IsNullOrWhiteSpace(txtKodu.Text))
            {
                System.Windows.MessageBox.Show("Malzeme kodu girilmelidir!", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var kdvItem = cmbKDV.SelectedItem as ComboBoxItem;
            var tipItem = cmbTip.SelectedItem as ComboBoxItem;

            decimal kdv = 18;
            if (kdvItem?.Tag != null)
                decimal.TryParse(kdvItem.Tag.ToString(), out kdv);

            int tip = 1;
            if (tipItem?.Tag != null)
                int.TryParse(tipItem.Tag.ToString(), out tip);

            int birim = 0;
            int.TryParse(txtBirim.Text, out birim);

            decimal minStok = 0;
            decimal.TryParse(txtMinStok.Text, out minStok);

            decimal maxStok = 0;
            decimal.TryParse(txtMaxStok.Text, out maxStok);

            var data = new System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", _currentId },
                { "Code", txtKodu.Text },
                { "Name", txtAdi.Text },
                { "Type", tip },
                { "UnitId", birim },
                { "Barcode", txtBarkod.Text ?? "" },
                { "VatRate", kdv },
                { "MinStock", minStok },
                { "MaxStock", maxStok },
                { "IsActive", chkKullanimda.IsChecked == true },
                { "CreatedAt", DateTime.Now },
                { "UpdatedAt", DateTime.Now }
            };

            _currentId = _repo.Save(data);
            System.Windows.MessageBox.Show("Kaydedildi!", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void Sil()
        {
            if (_currentId == 0) return;

            var result = System.Windows.MessageBox.Show("Kaydı silmek istediğinize emin misiniz?", "Silme Onayı", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _repo.Delete(_currentId);
                Yeni();
            }
        }

        public void Yazdir()
        {
            System.Windows.MessageBox.Show("Yazdırma özelliği henüz eklenmedi.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void Ileri()
        {
            var list = _repo.GetAll().ToList();
            var currentIndex = list.FindIndex(x => x.Id == _currentId);
            if (currentIndex >= 0 && currentIndex < list.Count - 1)
            {
                LoadRecord(list[currentIndex + 1]);
            }
        }

        public void Geri()
        {
            var list = _repo.GetAll().ToList();
            var currentIndex = list.FindIndex(x => x.Id == _currentId);
            if (currentIndex > 0)
            {
                LoadRecord(list[currentIndex - 1]);
            }
        }

        public void Listele()
        {
            var pencere = new winMalzemeListesiV2();
            pencere.Owner = Window.GetWindow(this);
            if (pencere.ShowDialog() == true)
            {
                var record = _repo.GetById(pencere.SecilenId);
                if (record != null)
                    LoadRecord(record);
            }
        }
    }
}