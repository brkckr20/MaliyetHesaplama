using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MaliyeHesaplama.userControls
{
    /// <summary>
    /// Interaction logic for UC_UretimYonetimiParametreleri.xaml
    /// </summary>
    public partial class UC_UretimYonetimiParametreleri : UserControl
    {
        MiniOrm _orm = new MiniOrm();
        public UC_UretimYonetimiParametreleri()
        {
            InitializeComponent();
            GetDatas();
        }

        void GetDatas()
        {
            txtOperasyonTipi.Text = _orm.GetById<dynamic>("ProductionManagementParams", 1).ReceteOperasyonTipleri;
        }
        private void btnYeni_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnKayit_Click(object sender, RoutedEventArgs e)
        {
            var dict = new Dictionary<string, object>()
            {
                { "Id", 1},
                { "ReceteOperasyonTipleri", txtOperasyonTipi.Text },
            };
            _orm.Save("ProductionManagementParams", dict);
        }

        private void btnListe_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnGeri_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnIleri_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSil_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
