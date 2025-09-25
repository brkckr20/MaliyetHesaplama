using MaliyeHesaplama.helpers;
using System.Windows;

namespace MaliyeHesaplama.wins
{
    public partial class winNumaratorListesi : Window
    {
        MiniOrm _orm = new MiniOrm();
        public int Id, Number;
        public string NameX, Prefix;
        public winNumaratorListesi(Enums.Inventory type)
        {
            InitializeComponent(); // listeden kontrol ederken ki kısım için düzeltilecek - şartlar kalkacak
            sfDataGrid.ItemsSource = _orm.GetAll<dynamic>("Numerator").Where(x => x.InventoryType == Convert.ToInt32(type) && x.IsActive == true).ToList();
        }

        private void txtKodu_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void txtAdi_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void sfDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sfDataGrid.SelectedItem != null)
            {
                dynamic record = sfDataGrid.SelectedItem;
                Id = record.Id;
                Prefix = record.Prefix;
                NameX = record.Name;
                Number = record.Number;
                this.Close();
            }
        }
    }
}
