using MaliyeHesaplama.helpers;
using MaliyeHesaplama.Interfaces;

namespace MaliyeHesaplama.userControls
{
    public partial class UC_GTIPTanimlari : System.Windows.Controls.UserControl, IPageCommands
    {
        private MiniOrm _orm; 
        private int Id = 0;
        public UC_GTIPTanimlari()
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
                    {"Explanation", txtAciklama.Text},
                };
                Id = _orm.Save("GTIP", dict);
                Bildirim.Bilgilendirme2("Veri kayıt işlemi başarıyla gerçekleştirildi.");
            }
            else
            {
                Bildirim.Uyari2("Kodu boş bırakılamaz!");
            }
        }

        public void Listele() // buradan devam edilecek 24.03.2026
        {
            //throw new NotImplementedException();
        }

        public void Sil()
        {
            if (_orm.Delete("GTIP", Id, true) > 0)
            {
                Temizle();
            }
        }

        public void Yazdir()
        {
            //throw new NotImplementedException();
        }

        public void Yeni()
        {
            Temizle();
        }
        void Temizle()
        {
            Id = 0;
            txtKodu.Text = string.Empty;
            txtAdi.Text = string.Empty;
            chkAktif.IsChecked = true;
            txtAciklama.Text = string.Empty;
        }
        void KayitlariGetir(string tip)
        {
            dynamic record = null;
            if (tip == "Önceki")
            {
                record = _orm.GetBeforeRecord<dynamic>("GTIP", Id);
            }
            else
            {
                record = _orm.GetNextRecord<dynamic>("GTIP", Id);
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
    }
}
