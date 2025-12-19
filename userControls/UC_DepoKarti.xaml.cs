using MaliyeHesaplama.helpers;
using MaliyeHesaplama.Interfaces;
using System.Windows.Controls;

namespace MaliyeHesaplama.userControls
{
    public partial class UC_DepoKarti : UserControl, IPageCommands
    {
        private MiniOrm _orm;
        private int Id = 0;
        public UC_DepoKarti()
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

        public void Kaydet()
        {
            if (txtKodu.Text != string.Empty)
            {
                var dict = new Dictionary<string, object>
                {
                    { "Id",Id },
                    {"Code", txtKodu.Text },
                    {"Name", txtAdi.Text },
                    {"IsUse", chkAktif.IsChecked},
                };
                Id = _orm.Save("WareHouse", dict);
                Bildirim.Bilgilendirme2("Veri kayıt işlemi başarıyla gerçekleştirildi.");
            }
            else
            {
                Bildirim.Uyari2("Depo kodu boş bırakılamaz!");
            }
        }

        public void Listele()
        {
            wins.winDepoListesi win = new wins.winDepoListesi();
            win.ShowDialog();
            if (win.SecimYapildi)
            {
                Id = win.Id;
                txtKodu.Text = win.Kodu;
                txtAdi.Text = win.Adi;
                chkAktif.IsChecked = win.Kullanimda;
            }
        }

        public void Sil()
        {
            if (_orm.Delete("WareHouse", Id, true) > 0)
            {
                Temizle();
            }
        }

        public void Yazdir()
        {
            if (this.Id == 0)
            {
                Bildirim.Uyari2("Rapor alabilmek için lütfen bir kayıt seçiniz!");
            }
            else
            {
                wins.winRaporSecimi win = new wins.winRaporSecimi("Depo Kartı", Id);
                win.ShowDialog();
            }
        }

        public void Yeni()
        {
            Temizle();
        }
        void KayitlariGetir(string tip)
        {
            dynamic record = null;
            if (tip == "Önceki")
            {
                record = _orm.GetBeforeRecord<dynamic>("WareHouse", Id);
            }
            else
            {
                record = _orm.GetNextRecord<dynamic>("WareHouse", Id);
            }

            if (record != null)
            {
                Id = record.Id;
                txtKodu.Text = record.Code;
                txtAdi.Text = record.Name;
                chkAktif.IsChecked = record.IsUse;
            }
            else
            {
                Bildirim.Uyari2("Gösterilecek başka bir kayıt bulunamadı!");
            }
        }
        void Temizle()
        {
            Id = 0;
            txtKodu.Text = string.Empty;
            txtAdi.Text = string.Empty;
            chkAktif.IsChecked = true;
        }
    }
}
