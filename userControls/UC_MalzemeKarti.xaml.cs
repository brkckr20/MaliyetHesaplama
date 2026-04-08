using MaliyeHesaplama.helpers;
using MaliyeHesaplama.Interfaces;
using System.Collections.ObjectModel;
using System.Linq;

namespace MaliyeHesaplama.userControls
{
    public partial class UC_MalzemeKarti : System.Windows.Controls.UserControl, IPageCommands
    {
        private MiniOrm _orm;
        private int Id = 0, _Type, _InventoryType;
        int brandId = 0, seasonId = 0, genderId = 0, categoryId = 0, companyId = 0, gtipId = 0;
        private ObservableCollection<dynamic> _renkListesi = new ObservableCollection<dynamic>();
        private ObservableCollection<dynamic> _bedenListesi = new ObservableCollection<dynamic>();
        private ObservableCollection<MatrisRow> _matrisData = new ObservableCollection<MatrisRow>();

        public UC_MalzemeKarti(Enums.Inventory _inventory = Enums.Inventory.Malzeme)
        {
            InitializeComponent();
            ButtonBar.PageCommands = this;
            _orm = new MiniOrm();
            _Type = Convert.ToInt32(_inventory);
            _InventoryType = Convert.ToInt32(_inventory);
            if (_Type != 3)
            {
                tabControl1.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                _orm.CreateTablesIfNotExist();
                RenkBedenYukle();
            }
        }

        private void RenkBedenYukle()
        {
            var renkler = _orm.QueryRaw<dynamic>("SELECT Id, Kodu, Adi FROM Renk WHERE Aktif = 1 ORDER BY Adi");
            cmbRenk.ItemsSource = renkler;

            var bedenler = _orm.QueryRaw<dynamic>("SELECT Id, Kodu, Adi FROM Beden ORDER BY Siralama, Adi");
            cmbBeden.ItemsSource = bedenler;
        }

        private void btnRenkEkle_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (cmbRenk.SelectedItem == null) return;
            dynamic renk = cmbRenk.SelectedItem;
            var renkDict = (IDictionary<string, object>)renk;
            var renkId = Convert.ToInt32(renkDict["Id"]);
            var renkAdi = renkDict["Adi"].ToString();

            foreach (var row in _matrisData)
            {
                if (row.RenkId == renkId)
                {
                    Bildirim.Uyari2("Bu renk zaten matriste var!");
                    return;
                }
            }

            var yeniSatir = new MatrisRow { RenkId = renkId, RenkAdi = renkAdi };
            if (_bedenListesi.Count > 0)
            {
                foreach (var beden in _bedenListesi)
                {
                    dynamic b = beden;
                    var bDict = (IDictionary<string, object>)b;
                    yeniSatir.Variantlar.Add(new VariantCell { BedenId = Convert.ToInt32(bDict["Id"]), BedenAdi = bDict["Adi"].ToString() });
                }
            }
            _matrisData.Add(yeniSatir);
            dgMatris.ItemsSource = _matrisData;
            MatrisKolonlariniGuncelle();
        }

        private void btnRenkYeni_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string yeniRenk = Microsoft.VisualBasic.Interaction.InputBox("Yeni Renk Adı:", "Renk Ekle", "");
            if (!string.IsNullOrWhiteSpace(yeniRenk))
            {
                var dict = new Dictionary<string, object> { { "Id", 0 }, { "Kodu", yeniRenk.ToUpper() }, { "Adi", yeniRenk }, { "Aktif", true } };
                _orm.Save("Renk", dict);
                RenkBedenYukle();
            }
        }

        private void btnBedenEkle_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (cmbBeden.SelectedItem == null) return;
            dynamic beden = cmbBeden.SelectedItem;
            var bedenDict = (IDictionary<string, object>)beden;
            var bedenId = Convert.ToInt32(bedenDict["Id"]);
            var bedenAdi = bedenDict["Adi"].ToString();

            if (_bedenListesi.Any(b => Convert.ToInt32(((IDictionary<string, object>)b)["Id"]) == bedenId))
            {
                Bildirim.Uyari2("Bu beden zaten eklenmiş!");
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

        private void btnBedenYeni_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string yeniBeden = Microsoft.VisualBasic.Interaction.InputBox("Yeni Beden Adı:", "Beden Ekle", "");
            if (!string.IsNullOrWhiteSpace(yeniBeden))
            {
                var dict = new Dictionary<string, object> { { "Id", 0 }, { "Kodu", yeniBeden.ToUpper() }, { "Adi", yeniBeden }, { "Siralama", _bedenListesi.Count + 1 } };
                _orm.Save("Beden", dict);
                RenkBedenYukle();
            }
        }

        private void MatrisKolonlariniGuncelle()
        {
            dgMatris.Columns.Clear();
            dgMatris.Columns.Add(new System.Windows.Controls.DataGridTextColumn { Header = "Renk", Binding = new System.Windows.Data.Binding("RenkAdi"), Width = 120, IsReadOnly = true });

            if (_matrisData.Count > 0 && _matrisData[0].Variantlar.Count > 0)
            {
                for (int i = 0; i < _matrisData[0].Variantlar.Count; i++)
                {
                    int colIndex = i;
                    var col = new System.Windows.Controls.DataGridTemplateColumn { Header = _matrisData[0].Variantlar[i].BedenAdi, Width = 100 };
                    var template = new System.Windows.DataTemplate();

                    var stackPanelFactory = new System.Windows.FrameworkElementFactory(typeof(System.Windows.Controls.StackPanel));
                    stackPanelFactory.SetValue(System.Windows.Controls.StackPanel.OrientationProperty, System.Windows.Controls.Orientation.Horizontal);

                    var textBoxFactory = new System.Windows.FrameworkElementFactory(typeof(System.Windows.Controls.TextBox));
                    textBoxFactory.SetValue(System.Windows.Controls.TextBox.WidthProperty, 50.0);
                    textBoxFactory.SetBinding(System.Windows.Controls.TextBox.TextProperty, new System.Windows.Data.Binding($"Variantlar[{colIndex}].Barkod"));

                    var textBoxFiyatFactory = new System.Windows.FrameworkElementFactory(typeof(System.Windows.Controls.TextBox));
                    textBoxFiyatFactory.SetValue(System.Windows.Controls.TextBox.WidthProperty, 50.0);
                    textBoxFiyatFactory.SetBinding(System.Windows.Controls.TextBox.TextProperty, new System.Windows.Data.Binding($"Variantlar[{colIndex}].Fiyat"));

                    stackPanelFactory.AppendChild(textBoxFactory);
                    stackPanelFactory.AppendChild(textBoxFiyatFactory);

                    template.VisualTree = stackPanelFactory;
                    col.CellTemplate = template;
                    dgMatris.Columns.Add(col);
                }
            }
        }

        private void btnMatrisKaydet_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Id == 0)
            {
                Bildirim.Uyari2("Önce malzemeyi kaydedin!");
                return;
            }

            foreach (var row in _matrisData)
            {
                foreach (var variant in row.Variantlar)
                {
                    if (!string.IsNullOrWhiteSpace(variant.Barkod) || variant.Fiyat > 0)
                    {
                        var dict = new Dictionary<string, object>
                        {
                            { "InventoryId", Id },
                            { "RenkId", row.RenkId },
                            { "BedenId", variant.BedenId },
                            { "Barkod", variant.Barkod ?? string.Empty },
                            { "Fiyat", variant.Fiyat },
                            { "Aktif", true }
                        };
                        _orm.Save("Variant", dict);
                    }
                }
            }
            Bildirim.Bilgilendirme2("Renk/Beden matrisi kaydedildi.");
        }

        private void btnMatrisSil_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (dgMatris.SelectedItem is MatrisRow secili)
            {
                _matrisData.Remove(secili);
                dgMatris.ItemsSource = _matrisData;
                MatrisKolonlariniGuncelle();
            }
        }

        public void Geri()
        {
            KayitlariGetir("Önceki");
        }

        public void Ileri()
        {
            KayitlariGetir("Sonraki");
        }

        void KayitlariGetir(string tip)
        {
            dynamic record = null;
            if (tip == "Önceki")
            {
                record = _orm.GetBeforeRecord<dynamic>("Inventory", Id, $"Type = {_InventoryType}");
            }
            else
            {
                record = _orm.GetNextRecord<dynamic>("Inventory", Id, $"Type = {_InventoryType}");
            }

            if (record != null)
            {
                Id = record.Id;
                txtKodu.Text = record.InventoryCode;
                txtAdi.Text = record.InventoryName;
                chkKullanimda.IsChecked = record.IsUse;
                VaryantlariYukle();
            }
            else
            {
                Bildirim.Uyari2("Gösterilecek başka bir kayıt bulunamadı!");
            }
        }

        private void VaryantlariYukle()
        {
            _matrisData.Clear();
            var varyantlar = _orm.QueryRaw<dynamic>($"SELECT V.Id, V.RenkId, R.Adi AS RenkAdi, V.BedenId, B.Adi AS BedenAdi, V.Barkod, V.Fiyat FROM Variant V LEFT JOIN Renk R ON V.RenkId = R.Id LEFT JOIN Beden B ON V.BedenId = B.Id WHERE V.InventoryId = {Id} AND V.Aktif = 1");

            var gruplu = varyantlar.GroupBy(v => new { v.RenkId, v.RenkAdi });
            foreach (var grup in gruplu)
            {
                var satir = new MatrisRow { RenkId = (int)grup.Key.RenkId, RenkAdi = grup.Key.RenkAdi };
                foreach (var v in grup)
                {
                    satir.Variantlar.Add(new VariantCell { BedenId = (int)v.BedenId, BedenAdi = v.BedenAdi?.ToString(), Barkod = v.Barkod?.ToString(), Fiyat = v.Fiyat != null ? (decimal)v.Fiyat : 0 });
                }
                _matrisData.Add(satir);
            }

            var bedenler = _matrisData.FirstOrDefault()?.Variantlar.Select(v => new { v.BedenId, v.BedenAdi });
            if (bedenler != null)
            {
                foreach (var b in bedenler)
                {
                    _bedenListesi.Add(new { Id = b.BedenId, Adi = b.BedenAdi });
                }
            }

            dgMatris.ItemsSource = _matrisData;
            MatrisKolonlariniGuncelle();
        }

        public void Kaydet()
        {
            if (txtKodu.Text != string.Empty)
            {
                var dict = new Dictionary<string, object>
                {
                    { "Id",Id },
                    { "InventoryCode", txtKodu.Text },
                    { "InventoryName", txtAdi.Text },
                    { "IsPrefix",false},
                    { "IsUse",chkKullanimda.IsChecked},
                    { "Unit",string.Empty},
                    { "IsStock",true},
                    { "Type",_Type},
                };
                Id = _orm.Save("Inventory", dict);
                Bildirim.Bilgilendirme2("Veri kayıt işlemi başarıyla gerçekleştirildi.");
            }
            else
            {
                Bildirim.Uyari2("Malzeme kodu boş bırakılamaz!");
            }
        }

        public void Listele()
        {
            wins.winMalzemeListesi win = new wins.winMalzemeListesi(_Type);
            win.ShowDialog();
            if (win.Code != null)
            {
                this.Id = win.Id;
                txtKodu.Text = win.Code;
                txtAdi.Text = win.Name;
                chkKullanimda.IsChecked = win.IsUse;
                if (_Type == 3) VaryantlariYukle();
            }
        }

        public void Sil()
        {
            if (_orm.Delete("Inventory", Id, true) > 0)
            {
                Temizle();
            }
        }

        void Temizle()
        {
            this.Id = 0;
            txtKodu.Text = string.Empty;
            txtAdi.Text = string.Empty;
            chkKullanimda.IsChecked = false;
            txtModelSezon.Text = string.Empty;
            txtModelMarka.Text = string.Empty;
            txtModelCinsiyet.Text = string.Empty;
            txtModelKategori.Text = string.Empty;
            companyId = 0;
            txtFirma.Text = string.Empty;
            tbFirma.Text = string.Empty;
            _matrisData.Clear();
            _bedenListesi.Clear();
            dgMatris.ItemsSource = null;
            MatrisKolonlariniGuncelle();
        }

        void OzellikGetir(string ozellik, ref int id, System.Windows.Controls.TextBox tbox)
        {
            wins.winOzellikSecimi win = new wins.winOzellikSecimi(ozellik, _InventoryType);
            win.ShowDialog();
            if (win.SecimYapildi)
            {
                id = win.Id;
                tbox.Text = win.Explanation;
            }
        }

        private void modelMarka_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OzellikGetir("Marka", ref brandId, txtModelMarka);
        }

        private void btnGTIP_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MainHelper.GetGTIP(ref this.gtipId, txtGTIP, tbGTIP);
        }

        private void btnFirmaSecimi_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MainHelper.SetCompanyInformation(ref companyId, txtFirma, tbFirma);
        }

        private void btnModelKategori_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OzellikGetir("Kategori", ref categoryId, txtModelKategori);
        }

        private void btnModelCinsiyet_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OzellikGetir("Cinsiyet", ref genderId, txtModelCinsiyet);
        }

        private void btnModelSezon_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OzellikGetir("Sezon", ref seasonId, txtModelSezon);
        }

        public void Yazdir()
        {
            if (this.Id == 0)
            {
                Bildirim.Uyari2("Form görüntüleyebilmek için bir kayıt seçiniz!");
            }
            else
            {
                wins.winRaporSecimi win = new wins.winRaporSecimi("Malzeme Kartı", Id);
                win.ShowDialog();
            }
        }

        public void Yeni()
        {
            Temizle();
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