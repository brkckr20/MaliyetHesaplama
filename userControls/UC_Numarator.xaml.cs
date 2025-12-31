using MaliyeHesaplama.helpers;
using MaliyeHesaplama.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace MaliyeHesaplama.userControls
{
    public partial class UC_Numarator : System.Windows.Controls.UserControl,IPageCommands
    {
        MiniOrm _orm = new MiniOrm();
        private int Id = 0;
        public UC_Numarator()
        {
            InitializeComponent();
            ButtonBar.PageCommands = this;
        }

        void Temizle()
        {
            txtOnEk.Text = string.Empty;
            txtNumara.Text = string.Empty;
            txtIsim.Text = string.Empty;
            cmbTur.SelectedIndex = -1;
            chckKullanimda.IsChecked = true;
        }

        public void Yeni()
        {
            Temizle();
        }

        public void Kaydet()
        {
            var dict = new Dictionary<string, object> {
                {"Id",Id },{"Prefix",txtOnEk.Text },{"Number",txtNumara.Text},{"Name",txtIsim.Text},{"IsActive", chckKullanimda.IsChecked.HasValue},{"InventoryType", cmbTur.SelectedIndex.ToString() }
            };
            if (txtOnEk.Text != string.Empty && txtNumara.Text != string.Empty && txtIsim.Text != string.Empty && cmbTur.SelectedIndex.ToString() != "0")
            {
                if (_orm.Save("Numerator", dict) > 0)
                {
                    Bildirim.Bilgilendirme2("Kayıt işlemi başarılı bir şekilde gerçekleştirildi");
                }
            }
            else
            {
                Bildirim.Uyari2("Kayıt işleminin yapılabilmesi için tüm (*) ile işaretlemniş alanları doldurunuz!");
            }
        }

        public void Sil()
        {
            if (_orm.Delete("Numerator", Id, true) > 0)
            {
                Temizle();
            }
        }

        public void Yazdir()
        {
            //throw new NotImplementedException();
        }

        public void Ileri()
        {
            //throw new NotImplementedException();
        }

        public void Geri()
        {
            //throw new NotImplementedException();
        }

        public void Listele()
        {
            wins.winNumaratorListesi win = new wins.winNumaratorListesi(Enums.Inventory.Tumu);
            win.ShowDialog();
            if (win.SatirSecildi)
            {
                txtOnEk.Text = win.Prefix;
                txtNumara.Text = win.Number.ToString();
                txtIsim.Text = win.NameX;
                chckKullanimda.IsChecked = win.IsActive;
                cmbTur.SelectedIndex = win.InventoryType;
                this.Id = win.Id;
            }
        }
    }
}
