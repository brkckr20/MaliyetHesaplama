using MaliyeHesaplama.helpers;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;

namespace MaliyeHesaplama.wins
{
    public partial class winFisHareketleriListesi : Window
    {
        MiniOrm _orm = new MiniOrm();
        private int _receiptType;
        private ICollectionView _collectionView;
        public bool secimYapildi = false;
        public int Id, CompanyId, _depoId;
        public string ReceiptNo, CompanyName, Authorized, Maturity, CustomerOrderNo, Explanation;
        private void sFisNo_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainHelper.SearchWithCW(sender, "ReceiptNo", _collectionView, lblRecordCount);
        }

        private void sFirmaAdi_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainHelper.SearchWithCW(sender, "CompanyName", _collectionView, lblRecordCount);
        }

        private void sYetkili_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainHelper.SearchWithCW(sender, "Authorized", _collectionView, lblRecordCount);
        }

        private void sTermin_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainHelper.SearchWithCW(sender, "DuaDate", _collectionView, lblRecordCount);
        }

        private void sVade_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainHelper.SearchWithCW(sender, "Maturity", _collectionView, lblRecordCount);
        }

        private void sKalemIslem_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainHelper.SearchWithCW(sender, "OperationType", _collectionView, lblRecordCount);
        }

        private void sMalzemeKodu_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainHelper.SearchWithCW(sender, "InventoryCode", _collectionView, lblRecordCount);
        }

        private void sMalzemeAdi_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainHelper.SearchWithCW(sender, "InventoryName", _collectionView, lblRecordCount);
        }

        private void sVaryantKodu_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainHelper.SearchWithCW(sender, "VariantCode", _collectionView, lblRecordCount);
        }

        private void sVaryant_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainHelper.SearchWithCW(sender, "Variant", _collectionView, lblRecordCount);
        }

        private void sMetre_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainHelper.SearchWithCW(sender, "NetMeter", _collectionView, lblRecordCount);
        }

        private void sForex_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainHelper.SearchWithCW(sender, "Forex", _collectionView, lblRecordCount);
        }

        private void sPesin_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainHelper.SearchWithCW(sender, "CashPayment", _collectionView, lblRecordCount);
        }

        private void sVadeli_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainHelper.SearchWithCW(sender, "DeferredPayment", _collectionView, lblRecordCount);
        }

        private void sTarih_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainHelper.SearchWithCW(sender,"ReceiptDate", _collectionView, lblRecordCount);
        }

        public DateTime _Date, DuaDate;
        public List<dynamic> HareketlerListesi { get; set; } = new List<dynamic>();
        public winFisHareketleriListesi(int depoId, Enums.Receipt receipt)
        {
            InitializeComponent();
            this._receiptType = Convert.ToInt32(receipt);
            this._depoId = depoId;
        }
        private IEnumerable<dynamic> _tumHareketler;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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
                Explanation = record.Explanation.ToString();
                HareketlerListesi = _tumHareketler.Where(x => x.Id == Id).ToList();
                Close();
            }
        }
    }
}
