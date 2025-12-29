using MaliyeHesaplama.helpers;
using MaliyeHesaplama.Interfaces;
using System.Windows.Controls;

namespace MaliyeHesaplama.userControls
{
    public partial class UC_MalzemeKarti : UserControl, IPageCommands
    {
        private MiniOrm _orm;
        private int Id = 0, _Type = Convert.ToInt32(Enums.Inventory.Malzeme),_InventoryType = Convert.ToInt32(Enums.Inventory.Malzeme);

        public UC_MalzemeKarti()
        {
            InitializeComponent(); 
            ButtonBar.PageCommands = this;
            _orm = new MiniOrm();
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
