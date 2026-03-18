using MaliyeHesaplama.helpers;
using MaliyeHesaplama.Interfaces;
using System.Windows.Controls;

namespace MaliyeHesaplama.userControls
{
    public partial class UC_MalzemeKarti : System.Windows.Controls.UserControl, IPageCommands
    {
        private MiniOrm _orm;
        private int Id = 0, _Type, _InventoryType;
        int brandId = 0, seasonId = 0, genderId = 0, categoryId = 0;

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
            }
            else
            {
                Bildirim.Uyari2("Gösterilecek başka bir kayıt bulunamadı!");
            }
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
            txtKodu.Text = string.Empty;
            chkKullanimda.IsChecked = false;
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
}
