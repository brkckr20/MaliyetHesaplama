using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MaliyeHesaplama.Interfaces;
using MaliyeHesaplama.v2.Data;
using MaliyeHesaplama.v2.Models;
using MaliyeHesaplama.v2.Windows;

namespace MaliyeHesaplama.v2.UserControls
{
    public partial class UC_MalzemeKartiV2 : System.Windows.Controls.UserControl, IPageCommands
    {
        private readonly InventoryRepository _repo;
        private readonly UnitRepository _unitRepo;
        private readonly CategoryRepository _categoryRepo;
        private int _currentId = 0;

        public UC_MalzemeKartiV2()
        {
            InitializeComponent();
            _repo = new InventoryRepository();
            _unitRepo = new UnitRepository();
            _categoryRepo = new CategoryRepository();
            LoadBirimler();
            LoadKategoriler();
            ButtonBar.PageCommands = this;
            Yeni();
        }

        private void LoadKategoriler()
        {
            cmbKategori.Items.Clear();
            cmbKategori.Items.Add(new ComboBoxItem { Content = "Seçiniz", Tag = null });
            var kategoriler = _categoryRepo.GetActive().ToList();
            foreach (var kat in kategoriler)
            {
                cmbKategori.Items.Add(new ComboBoxItem
                {
                    Content = kat.Name,
                    Tag = kat.Id
                });
            }
            cmbKategori.SelectedIndex = 0;
        }

        private void LoadBirimler()
        {
            var birimler = _unitRepo.GetActive().ToList();
            foreach (var birim in birimler)
            {
                cmbBirim.Items.Add(new ComboBoxItem
                {
                    Content = birim.Name,
                    Tag = birim.Id
                });
            }
        }

        private void LoadRecord(Inventory record)
        {
            _currentId = record.Id;
            txtKodu.Text = record.InventoryCode ?? "";
            txtAdi.Text = record.InventoryName ?? "";

            if (record.Type.HasValue)
            {
                foreach (ComboBoxItem item in cmbTip.Items)
                {
                    if (item.Tag != null && int.TryParse(item.Tag.ToString(), out int tagVal) && tagVal == record.Type.Value)
                    {
                        cmbTip.SelectedItem = item;
                        break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(record.Unit))
            {
                foreach (ComboBoxItem item in cmbBirim.Items)
                {
                    if (item.Content?.ToString() == record.Unit)
                    {
                        cmbBirim.SelectedItem = item;
                        break;
                    }
                }
            }

            if (record.CategoryId.HasValue)
            {
                foreach (ComboBoxItem item in cmbKategori.Items)
                {
                    if (item.Tag != null && int.TryParse(item.Tag.ToString(), out int tagVal) && tagVal == record.CategoryId.Value)
                    {
                        cmbKategori.SelectedItem = item;
                        break;
                    }
                }
            }

            txtBarkod.Text = record.Barcode ?? "";

            if (record.VatRate.HasValue)
            {
                foreach (ComboBoxItem item in cmbKDV.Items)
                {
                    if (item.Tag != null && decimal.TryParse(item.Tag.ToString(), out decimal tagVal) && tagVal == record.VatRate.Value)
                    {
                        cmbKDV.SelectedItem = item;
                        break;
                    }
                }
            }

            txtMinStok.Text = record.MinStock?.ToString() ?? "";
            txtMaxStok.Text = record.MaxStock?.ToString() ?? "";
            chkKullanimda.IsChecked = record.IsUse;
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
            cmbBirim.SelectedIndex = 0;
            cmbKategori.SelectedIndex = 0;
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

            var tipItem = cmbTip.SelectedItem as ComboBoxItem;
            var birimItem = cmbBirim.SelectedItem as ComboBoxItem;
            var kategoriItem = cmbKategori.SelectedItem as ComboBoxItem;
            var kdvItem = cmbKDV.SelectedItem as ComboBoxItem;

            int? tip = null;
            if (tipItem?.Tag != null)
                int.TryParse(tipItem.Tag.ToString(), out int tempTip);

            string birim = "";
            if (birimItem?.Content != null)
                birim = birimItem.Content.ToString();

            int? kategoriId = null;
            if (kategoriItem?.Tag != null && int.TryParse(kategoriItem.Tag.ToString(), out int tempKat))
                kategoriId = tempKat;

            decimal? kdv = null;
            if (kdvItem?.Tag != null && decimal.TryParse(kdvItem.Tag.ToString(), out decimal kdvDeger))
                kdv = kdvDeger;

            decimal? minStok = null;
            if (!string.IsNullOrWhiteSpace(txtMinStok.Text) && decimal.TryParse(txtMinStok.Text, out decimal minStokDeger))
                minStok = minStokDeger;

            decimal? maxStok = null;
            if (!string.IsNullOrWhiteSpace(txtMaxStok.Text) && decimal.TryParse(txtMaxStok.Text, out decimal maxStokDeger))
                maxStok = maxStokDeger;

            var data = new System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", _currentId },
                { "InventoryCode", txtKodu.Text.Trim() },
                { "InventoryName", txtAdi.Text?.Trim() ?? "" },
                { "Type", tip },
                { "CategoryId", kategoriId },
                { "Unit", birim },
                { "Barcode", txtBarkod.Text?.Trim() ?? "" },
                { "VatRate", kdv },
                { "MinStock", minStok },
                { "MaxStock", maxStok },
                { "IsUse", chkKullanimda.IsChecked == true },
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
            wins.winRaporSecimi win = new wins.winRaporSecimi("Malzeme Kartı", _currentId);
            if (_currentId == 0)
            {
                System.Windows.MessageBox.Show("Rapor alabilmek için lütfen kayıt seçiniz", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            win.ShowDialog();
        }

        public void Ileri()
        {
            var list = _repo.GetAll().ToList();
            if (list.Count == 0)
            {
                System.Windows.MessageBox.Show("Kayıt bulunamadı!", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var currentIndex = list.FindIndex(x => x.Id == _currentId);
            if (currentIndex < 0)
            {
                System.Windows.MessageBox.Show("Kayıt bulunamadı!", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (currentIndex < list.Count - 1)
            {
                LoadRecord(list[currentIndex + 1]);
            }
        }

        public void Geri()
        {
            var list = _repo.GetAll().ToList();
            if (list.Count == 0)
            {
                System.Windows.MessageBox.Show("Kayıt bulunamadı!", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var currentIndex = list.FindIndex(x => x.Id == _currentId);
            if (currentIndex < 0)
            {
                System.Windows.MessageBox.Show("Kayıt bulunamadı!", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
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