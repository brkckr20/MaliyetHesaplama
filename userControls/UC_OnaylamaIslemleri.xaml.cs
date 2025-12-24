using MaliyeHesaplama.helpers;
using MaliyeHesaplama.models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Windows.Controls;
using System.Windows.Data;

namespace MaliyeHesaplama.userControls
{
    class ReceiptVM // sadece bu alanları göstermek için ViewModel - başka bir yerde kullanılmıyor
    {
        [Display(Name = "Kayıt No")]
        public int Id { get; set; }
        [Display(Name = "Fiş No")]
        public string ReceiptNo { get; set; }
        [Display(Name = "Firma Adı")]
        public string CompanyName { get; set; }
        [Display(Name = "Onaylı mı?")]
        public string Approved { get; set; }
        [Display(Name = "Fiş Tarihi")]
        public DateTime ReceiptDate { get; set; }
        [Display(Name = "Depo")]
        public string WarehouseName { get; set; }
        [Display(Name = "Metre")]
        public decimal NetMeter { get; set; }
        [Display(Name = "Fiş Tipi Adı")]
        public string ReceiptTypeName { get; set; }
    }
    public partial class UC_OnaylamaIslemleri : UserControl
    {
        FilterGridHelpers fgh1;
        MiniOrm _orm = new MiniOrm();
        private ICollectionView _collectionView;
        bool approvedFilter = false;
        public UC_OnaylamaIslemleri()
        {
            InitializeComponent();
            fgh1 = new FilterGridHelpers(gridSip, "Sipariş Onaylama", "gridSip");
            SetFilteredDataGridProperties(fgh1);
        }

        private void onayli_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            approvedFilter = true;
            LoadGridData(gridSip);
        }

        private void bekleyen_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            approvedFilter = false;
            LoadGridData(gridSip);
        }
        void OnayDurumunuDegistir(bool _approve, System.Windows.RoutedEventArgs e, FilterDataGrid.FilterDataGrid grid)
        {
            if (grid.SelectedItem == null)
            {
                e.Handled = true;
                Bildirim.Uyari2("Kayıt güncellemek için lütfen bir satır seçiniz!");
            }
            if (grid.SelectedItem is ReceiptVM drv)
            {
                int id = Convert.ToInt32(drv.Id);
                var dict = new Dictionary<string, object>
                            {{"Id",id },{ "Approved", _approve }};
                if (_orm.Save("Receipt", dict) > 0)
                {
                    Bildirim.Bilgilendirme2("Güncelleme işlemi tamamlandı");
                }
                ;
            }
        }
        private void sip_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            LoadGridData(gridSip);
        }

        private void gridSip_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var hiddenColumns = new[] { "Id" };
            fgh1.GridGeneratingColumn(e, gridSip, hiddenColumns);
        }

        private void onayla_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OnayDurumunuDegistir(true, e, gridSip);
            LoadGridData(gridSip);
        }

        private void kaldir_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OnayDurumunuDegistir(false, e, gridSip);
            LoadGridData(gridSip);
        }
        void LoadGridData(FilterDataGrid.FilterDataGrid grid)
        {
            var companies = _orm.GetAll<Company>("Company")
                    .ToDictionary(c => c.Id, c => c.CompanyName);
            var wh = _orm.GetAll<WareHouse>("WareHouse")
                    .ToDictionary(c => c.Id, c => c.Name);
            var items = _orm.GetAll<ReceiptItem>("ReceiptItem");
            var totals = items
                        .GroupBy(i => i.ReceiptId)
                        .ToDictionary(g => g.Key, g => g.Sum(i => i.NetMeter));

            var data = (from r in _orm.GetAll<Receipt>("Receipt")
                        where r.ReceiptType == (int)Enums.Receipt.Siparis && r.Approved == approvedFilter
                        join ri in _orm.GetAll<ReceiptItem>("ReceiptItem")
                            on r.Id equals ri.ReceiptId into receiptItems
                        select new ReceiptVM
                        {
                            Id = r.Id,
                            ReceiptNo = r.ReceiptNo,
                            ReceiptDate = r.ReceiptDate,
                            CompanyName = companies.ContainsKey(r.CompanyId) ? companies[r.CompanyId] : "",
                            WarehouseName = wh.ContainsKey(r.WareHouseId) ? wh[r.WareHouseId] : "",
                            Approved = r.Approved ? "Evet" : "Hayır",
                            NetMeter = receiptItems.Sum(x => x.NetMeter),
                            ReceiptTypeName = MainHelper.GetEnumDisplayName((Enums.Receipt)r.ReceiptType)
                        }).ToList();

            _collectionView = CollectionViewSource.GetDefaultView(data);
            grid.ItemsSource = _collectionView;
        }
        void SetFilteredDataGridProperties(FilterGridHelpers grid)
        {
            Dispatcher.BeginInvoke(new System.Action(() =>
            {
                grid.InitializeColumnSettings();
                grid.LoadColumnSettingsFromDatabase();
            }), System.Windows.Threading.DispatcherPriority.Loaded);
        }
    }
}
