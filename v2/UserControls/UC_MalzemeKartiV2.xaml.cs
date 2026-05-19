using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
        private readonly MiniOrm _orm;
        private int _currentId = 0;
        private ObservableCollection<dynamic> _bedenListesi = new ObservableCollection<dynamic>();
        private ObservableCollection<MatrisRow> _matrisData = new ObservableCollection<MatrisRow>();

        public UC_MalzemeKartiV2()
        {
            InitializeComponent();
            _orm = new MiniOrm();
            _repo = new InventoryRepository();
            _unitRepo = new UnitRepository();
            _categoryRepo = new CategoryRepository();
            LoadBirimler();
            LoadKategoriler();
            LoadRenkler();
            LoadBedenler();
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

        private void LoadRenkler()
        {
            var renkler = _orm.QueryRaw<dynamic>("SELECT Id, Kodu, Adi FROM Renk WHERE Aktif = 1 ORDER BY Adi");
            cmbRenk.ItemsSource = renkler;
        }

        private void LoadBedenler()
        {
            var bedenler = _orm.QueryRaw<dynamic>("SELECT Id, Kodu, Adi FROM Beden ORDER BY Siralama, Adi");
            cmbBeden.ItemsSource = bedenler;
        }

        private void btnRenkEkle_Click(object sender, RoutedEventArgs e)
        {
            if (cmbRenk.SelectedItem == null) return;
            dynamic renk = cmbRenk.SelectedItem;
            var renkDict = (IDictionary<string, object>)renk;
            var renkId = Convert.ToInt32(renkDict["Id"]);
            var renkAdi = renkDict["Adi"].ToString();

            if (_matrisData.Any(r => r.RenkId == renkId))
            {
                System.Windows.MessageBox.Show("Bu renk zaten matriste var!", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var yeniSatir = new MatrisRow { RenkId = renkId, RenkAdi = renkAdi };
            foreach (var beden in _bedenListesi)
            {
                dynamic b = beden;
                var bDict = (IDictionary<string, object>)b;
                yeniSatir.Variantlar.Add(new VariantCell
                {
                    BedenId = Convert.ToInt32(bDict["Id"]),
                    BedenAdi = bDict["Adi"].ToString()
                });
            }
            _matrisData.Add(yeniSatir);
            dgMatris.ItemsSource = null;
            dgMatris.ItemsSource = _matrisData;
            MatrisKolonlariniGuncelle();
        }

        private void btnRenkYeni_Click(object sender, RoutedEventArgs e)
        {
            string yeniRenk = Microsoft.VisualBasic.Interaction.InputBox("Yeni Renk Adı:", "Renk Ekle", "");
            if (!string.IsNullOrWhiteSpace(yeniRenk))
            {
                var dict = new Dictionary<string, object> { { "Id", 0 }, { "Kodu", yeniRenk.ToUpper() }, { "Adi", yeniRenk }, { "Aktif", true } };
                _orm.Save("Renk", dict);
                LoadRenkler();
            }
        }

        private void btnBedenEkle_Click(object sender, RoutedEventArgs e)
        {
            if (cmbBeden.SelectedItem == null) return;
            dynamic beden = cmbBeden.SelectedItem;
            var bedenDict = (IDictionary<string, object>)beden;
            var bedenId = Convert.ToInt32(bedenDict["Id"]);
            var bedenAdi = bedenDict["Adi"].ToString();

            if (_bedenListesi.Any(b =>
            {
                dynamic x = b;
                return Convert.ToInt32(((IDictionary<string, object>)x)["Id"]) == bedenId;
            }))
            {
                System.Windows.MessageBox.Show("Bu beden zaten eklenmiş!", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _bedenListesi.Add(beden);

            foreach (var row in _matrisData)
            {
                row.Variantlar.Add(new VariantCell { BedenId = bedenId, BedenAdi = bedenAdi });
            }

            dgMatris.ItemsSource = null;
            dgMatris.ItemsSource = _matrisData;
            MatrisKolonlariniGuncelle();
        }

        private void btnBedenYeni_Click(object sender, RoutedEventArgs e)
        {
            string yeniBeden = Microsoft.VisualBasic.Interaction.InputBox("Yeni Beden Adı:", "Beden Ekle", "");
            if (!string.IsNullOrWhiteSpace(yeniBeden))
            {
                var dict = new Dictionary<string, object> { { "Id", 0 }, { "Kodu", yeniBeden.ToUpper() }, { "Adi", yeniBeden }, { "Siralama", _bedenListesi.Count + 1 } };
                _orm.Save("Beden", dict);
                LoadBedenler();
            }
        }

        private void btnMatrisSil_Click(object sender, RoutedEventArgs e)
        {
            if (dgMatris.SelectedItem is MatrisRow secili)
            {
                _matrisData.Remove(secili);
                dgMatris.ItemsSource = null;
                dgMatris.ItemsSource = _matrisData;
                MatrisKolonlariniGuncelle();
            }
        }

        private void MatrisKolonlariniGuncelle()
        {
            dgMatris.Columns.Clear();
            dgMatris.Columns.Add(new DataGridTextColumn
            {
                Header = "Renk",
                Binding = new System.Windows.Data.Binding("RenkAdi"),
                Width = 120,
                IsReadOnly = true
            });

            if (_matrisData.Count > 0 && _matrisData[0].Variantlar.Count > 0)
            {
                for (int i = 0; i < _matrisData[0].Variantlar.Count; i++)
                {
                    int colIndex = i;
                    var col = new DataGridTemplateColumn
                    {
                        Header = _matrisData[0].Variantlar[i].BedenAdi,
                        Width = 160
                    };

                    var factory = new FrameworkElementFactory(typeof(StackPanel));
                    factory.SetValue(StackPanel.OrientationProperty, System.Windows.Controls.Orientation.Horizontal);

                    var txtBarkod = new FrameworkElementFactory(typeof(System.Windows.Controls.TextBox));
                    txtBarkod.SetValue(System.Windows.Controls.TextBox.WidthProperty, 70.0);
                    txtBarkod.SetBinding(System.Windows.Controls.TextBox.TextProperty, new System.Windows.Data.Binding($"Variantlar[{colIndex}].Barkod"));

                    var txtFiyat = new FrameworkElementFactory(typeof(System.Windows.Controls.TextBox));
                    txtFiyat.SetValue(System.Windows.Controls.TextBox.WidthProperty, 70.0);
                    txtFiyat.SetBinding(System.Windows.Controls.TextBox.TextProperty, new System.Windows.Data.Binding($"Variantlar[{colIndex}].Fiyat"));

                    factory.AppendChild(txtBarkod);
                    factory.AppendChild(txtFiyat);

                    var template = new DataTemplate { VisualTree = factory };
                    col.CellTemplate = template;
                    dgMatris.Columns.Add(col);
                }
            }
        }

        private void VaryantlariYukle()
        {
            _matrisData.Clear();
            _bedenListesi.Clear();

            var varyantlar = _orm.QueryRaw<dynamic>(
                $"SELECT V.Id, V.RenkId, R.Adi AS RenkAdi, V.BedenId, B.Adi AS BedenAdi, V.Barkod, V.Fiyat " +
                $"FROM Variant V " +
                $"LEFT JOIN Renk R ON V.RenkId = R.Id " +
                $"LEFT JOIN Beden B ON V.BedenId = B.Id " +
                $"WHERE V.InventoryId = {_currentId} AND V.Aktif = 1");

            var gruplu = varyantlar.GroupBy(v =>
            {
                dynamic x = v;
                var dict = (IDictionary<string, object>)x;
                return new { RenkId = Convert.ToInt32(dict["RenkId"]), RenkAdi = dict["RenkAdi"]?.ToString() ?? "" };
            });

            foreach (var grup in gruplu)
            {
                var satir = new MatrisRow { RenkId = grup.Key.RenkId, RenkAdi = grup.Key.RenkAdi };
                foreach (var v in grup)
                {
                    dynamic dv = v;
                    var vDict = (IDictionary<string, object>)dv;
                    satir.Variantlar.Add(new VariantCell
                    {
                        BedenId = Convert.ToInt32(vDict["BedenId"]),
                        BedenAdi = vDict["BedenAdi"]?.ToString() ?? "",
                        Barkod = vDict["Barkod"]?.ToString() ?? "",
                        Fiyat = vDict["Fiyat"] != null ? Convert.ToDecimal(vDict["Fiyat"]) : 0
                    });

                    var bedenId = Convert.ToInt32(vDict["BedenId"]);
                    if (!_bedenListesi.Any(b =>
                    {
                        dynamic x = b;
                        return Convert.ToInt32(((IDictionary<string, object>)x)["Id"]) == bedenId;
                    }))
                    {
                        _bedenListesi.Add(new Dictionary<string, object>
                        {
                            { "Id", bedenId },
                            { "Adi", vDict["BedenAdi"]?.ToString() ?? "" }
                        });
                    }
                }
                _matrisData.Add(satir);
            }

            dgMatris.ItemsSource = _matrisData;
            MatrisKolonlariniGuncelle();
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

            VaryantlariYukle();
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
            _matrisData.Clear();
            _bedenListesi.Clear();
            dgMatris.ItemsSource = null;
            MatrisKolonlariniGuncelle();
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
            if (tipItem?.Tag != null && int.TryParse(tipItem.Tag.ToString(), out int tempTip))
                tip = tempTip;

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

            var data = new Dictionary<string, object>
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

            _orm.ExecuteRaw($"DELETE FROM Variant WHERE InventoryId = {_currentId}");
            foreach (var row in _matrisData)
            {
                foreach (var variant in row.Variantlar)
                {
                    if (!string.IsNullOrWhiteSpace(variant.Barkod) || variant.Fiyat > 0)
                    {
                        var vData = new Dictionary<string, object>
                        {
                            { "Id", 0 },
                            { "InventoryId", _currentId },
                            { "RenkId", row.RenkId },
                            { "BedenId", variant.BedenId },
                            { "Barkod", variant.Barkod ?? "" },
                            { "Fiyat", variant.Fiyat },
                            { "Aktif", true }
                        };
                        _orm.Save("Variant", vData);
                    }
                }
            }

            System.Windows.MessageBox.Show("Kaydedildi!", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void Sil()
        {
            if (_currentId == 0) return;

            var result = System.Windows.MessageBox.Show("Kaydı silmek istediğinize emin misiniz?", "Silme Onayı", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _orm.ExecuteRaw($"DELETE FROM Variant WHERE InventoryId = {_currentId}");
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

    public class MatrisRow
    {
        public int RenkId { get; set; }
        public string RenkAdi { get; set; }
        public ObservableCollection<VariantCell> Variantlar { get; set; } = new ObservableCollection<VariantCell>();
    }

    public class VariantCell
    {
        public int BedenId { get; set; }
        public string BedenAdi { get; set; }
        public string Barkod { get; set; }
        public decimal Fiyat { get; set; }
    }
}
