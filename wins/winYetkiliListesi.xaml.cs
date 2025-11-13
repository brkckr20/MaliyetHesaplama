using Syncfusion.UI.Xaml.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace MaliyeHesaplama.wins
{
    /// <summary>
    /// Interaction logic for winYetkiliListesi.xaml
    /// </summary>
    public partial class winYetkiliListesi : Window
    {
        MiniOrm _orm = new MiniOrm();
        private ICollectionView _collectionView;
        public string Yetkili;
        public bool SecimYapildi = false;
        int _companyId;
        public winYetkiliListesi(int companyId)
        {
            InitializeComponent();
            _companyId = companyId;
        }

        private void txtYetkili_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void mygrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (mygrid.SelectedItem != null)
            {
                this.SecimYapildi = true;
                dynamic record = mygrid.SelectedItem;
                Yetkili = record.Authorized;
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var data = _orm.GetAll<dynamic>("Receipt")
            .Where(x => x.CompanyId == _companyId)
            .Select(x => new
            {
                Authorized = (x.Authorized is DBNull || x.Authorized == null)
                    ? ""
                    : x.Authorized.ToString()
            })
            .Where(x => !string.IsNullOrWhiteSpace(x.Authorized))
            .GroupBy(x => x.Authorized)
            .Select(g => g.First())
            .ToList();
            _collectionView = CollectionViewSource.GetDefaultView(data);
            mygrid.ItemsSource = _collectionView;
        }
    }
}
