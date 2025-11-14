using MaliyeHesaplama.helpers;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using static MaliyeHesaplama.helpers.Enums;

namespace MaliyeHesaplama.wins
{
    public partial class winFisHareketleriListesi : Window
    {
        MiniOrm _orm = new MiniOrm();
        private int _receiptType;
        private ICollectionView _collectionView;
        public bool secimYapildi = false;
        public int Id, CompanyId, _depoId;
        public string ReceiptNo, CompanyName, Authorized, Maturity, CustomerOrderNo;
        public DateTime _Date, DuaDate;
        public List<dynamic> HareketlerListesi { get; set; } = new List<dynamic>();
        public winFisHareketleriListesi(int depoId, Enums.Receipt receipt)
        {
            InitializeComponent();
            this._receiptType = Convert.ToInt32(receipt);
            this._depoId = depoId;
            //dgListe.Columns[8].Visibility = Visibility.Collapsed; // diğer alanlarında kontrolü yapılacak
        }
        private IEnumerable<dynamic> _tumHareketler;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //var data = _orm.GetMovementList<dynamic>(_depoId, _receiptType);
            //_collectionView = CollectionViewSource.GetDefaultView(data);
            //dgListe.ItemsSource = _collectionView;
            _tumHareketler = _orm.GetMovementList<dynamic>(_depoId, _receiptType);
            _collectionView = CollectionViewSource.GetDefaultView(_tumHareketler);
            dgListe.ItemsSource = _collectionView;
        }

        private void dgListe_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (dgListe.SelectedItem != null)
            {
                this.secimYapildi = true;
                dynamic record = dgListe.SelectedItem;
                Id = record.Id;
                ReceiptNo = record.ReceiptNo;
                _Date = record.ReceiptDate;
                CompanyId = record.CompanyId;
                CompanyName = record.CompanyName;
                Authorized = record.Authorized;
                DuaDate = record.DuaDate;
                Maturity = record.Maturity.ToString();
                CustomerOrderNo = record.CustomerOrderNo.ToString();
                HareketlerListesi = _tumHareketler.Where(x => x.Id == Id).ToList();
                Close();
            }
        }
    }
}
