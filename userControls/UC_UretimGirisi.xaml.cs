using MaliyeHesaplama.Interfaces;
using System.Windows.Controls;
using MaliyeHesaplama.helpers;

namespace MaliyeHesaplama.userControls
{
    public partial class UC_UretimGirisi : UserControl, IPageCommands
    {
        public UC_UretimGirisi()
        {
            InitializeComponent();
            ButtonBar.PageCommands = this;
        }

        public void Geri()
        {
            //
        }

        public void Ileri()
        {
            //
        }

        public void Kaydet()
        {
            //
        }

        public void Listele()
        {
            //
        }

        public void Sil()
        {
            //
        }

        public void Yazdir()
        {
            //
        }

        public void Yeni()
        {
            
        }

        private void btnFirmaListesi_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void btnYetkiliListesi_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void btnDepoListesi_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

        }

        private void btnVariantListe_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void btnKumasListe_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
