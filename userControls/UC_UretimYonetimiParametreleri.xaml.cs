using MaliyeHesaplama.helpers;
using MaliyeHesaplama.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace MaliyeHesaplama.userControls
{
    public partial class UC_UretimYonetimiParametreleri : UserControl, IPageCommands
    {
        MiniOrm _orm = new MiniOrm();
        public UC_UretimYonetimiParametreleri()
        {
            InitializeComponent();
            ButtonBar.PageCommands = this;
            GetDatas();
        }

        void GetDatas()
        {
            var data = _orm.GetById<dynamic>("ProductionManagementParams", 1);
            txtOperasyonTipi.Text = data.ReceteOperasyonTipleri;
            chckReceteOlacak.IsChecked = Convert.ToBoolean(data.KumasRecetesiOlacak);
            foreach (ComboBoxItem item in cmbKurSecimi.Items)
            {
                if (item.Tag?.ToString() == data.BazAlinacakKur)
                {
                    cmbKurSecimi.SelectedItem = item;
                    break;
                }
            }
            txtDovizKurlari.Text = data.DovizKurlari;
        }

        public void Yeni()
        {

        }

        public void Kaydet()
        {
            var dict = new Dictionary<string, object>()
            {
                { "Id", 1},
                { "ReceteOperasyonTipleri", txtOperasyonTipi.Text },
                { "KumasRecetesiOlacak", chckReceteOlacak.IsChecked },
                { "BazAlinacakKur", (cmbKurSecimi.SelectedItem as ComboBoxItem)?.Tag?.ToString() },
                { "DovizKurlari", txtDovizKurlari.Text },
            };
            _orm.Save("ProductionManagementParams", dict);
            Bildirim.Bilgilendirme2("Kayıt işlemi başarıyla gerçekleştirildi.");
        }

        public void Sil()
        {
            
        }

        public void Yazdir()
        {
            
        }

        public void Ileri()
        {
            
        }

        public void Geri()
        {
            
        }

        public void Listele()
        {
            
        }
    }
}
