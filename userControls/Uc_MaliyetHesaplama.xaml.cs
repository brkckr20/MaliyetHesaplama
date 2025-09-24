using System.Windows.Controls;

namespace MaliyeHesaplama.userControls
{
    public partial class Uc_MaliyetHesaplama : UserControl
    {
        
        public Uc_MaliyetHesaplama()
        {
            InitializeComponent();
            dpTarih.SelectedDate = DateTime.Now;
        }

        private void btnFirmaListesi_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            wins.winFirmaListesi win = new wins.winFirmaListesi();
            win.ShowDialog();
            if (win.FirmaKodu != null)
            {
                txtFirmaKodu.Text = win.FirmaKodu;
                txtFirmaUnvan.Content = win.FirmaUnvan;
            }
        }
    }
}
