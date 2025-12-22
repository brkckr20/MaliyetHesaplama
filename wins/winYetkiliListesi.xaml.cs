using MaliyeHesaplama.helpers;
using MaliyeHesaplama.models;
using Stimulsoft.Report.Helpers;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace MaliyeHesaplama.wins
{
    public partial class winYetkiliListesi : Window
    {
        MiniOrm _orm = new MiniOrm();
        private ICollectionView _collectionView;
        public string Yetkili;
        public bool SecimYapildi = false;
        int _companyId;
        FilterGridHelpers fgh;
        public winYetkiliListesi(int companyId)
        {
            InitializeComponent();
            _companyId = companyId;
            fgh = new FilterGridHelpers(mygrid, "Yetkili Listesi", "grid");
        }
        private void mygrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (mygrid.SelectedItem != null)
            {
                this.SecimYapildi = true;
                dynamic record = mygrid.SelectedItem;
                Yetkili = record.Yetkili;
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var data = _orm.GetAll<Receipt>("Receipt")
                .Where(x => x.CompanyId == _companyId && x.Authorized != null)
                .Select(x => new
                {
                    Yetkili = x.Authorized
                })
                .Distinct()
            .ToList();
            _collectionView = CollectionViewSource.GetDefaultView(data);
            mygrid.ItemsSource = _collectionView;
            Dispatcher.BeginInvoke(new Action(() =>
            {
                fgh.InitializeColumnSettings();
                fgh.LoadColumnSettingsFromDatabase();
            }), System.Windows.Threading.DispatcherPriority.Loaded);
        }

        private void mygrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var hiddenColumns = new[] { "" };
            fgh.GridGeneratingColumn(e, mygrid, hiddenColumns);
        }
    }
}
