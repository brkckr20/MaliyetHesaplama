using System.Windows;

namespace MaliyeHesaplama.wins
{
    public partial class winNumaratorListesi : Window
    {
        MiniOrm _orm = new MiniOrm();
        public int Id { get; set; } = 0;
        public bool SatirSecildi { get; set; } = false;
        public string OnEk { get; set; } = "";
        public int Numara { get; set; } = 0;
        public string Isim { get; set; } = "";
        public bool Kullanimda { get; set; } = false;
        public int Tur { get; set; }
        public winNumaratorListesi()
        {
            InitializeComponent();
            sfDataGrid.ItemsSource = _orm.GetAll<dynamic>("Numerator");
            KolonlaraTurkceIsimVer();
        }

        void KolonlaraTurkceIsimVer()
        {
            sfDataGrid.Loaded += (s, e) =>
            {
                var idColumn = sfDataGrid.Columns.FirstOrDefault(c => c.MappingName == "Id");
                if (idColumn != null)
                    idColumn.IsHidden = true;
            };
            sfDataGrid.AutoGeneratingColumn += (s, e) =>
            {
                switch (e.Column.MappingName)
                {
                    case "Prefix":
                        e.Column.HeaderText = "Ön Ek";
                        break;
                    case "Number":
                        e.Column.HeaderText = "Numara";
                        break;
                    case "Name":
                        e.Column.HeaderText = "İsim";
                        break;
                    case "IsActive":
                        e.Column.HeaderText = "Kullanımda?";
                        break;
                    case "InventoryType":
                        e.Column.HeaderText = "Tip";
                        break;
                }
            };
        }

        private void sfDataGrid_CellDoubleTapped(object sender, Syncfusion.UI.Xaml.Grid.GridCellDoubleTappedEventArgs e)
        {
            var record = e.Record as dynamic;
            if (record != null)
            {
                Id = record.Id;
                OnEk = record.Prefix;
                Numara = record.Number;
                Isim = record.Name;
                Kullanimda = Convert.ToBoolean(record.IsActive);
                SatirSecildi = true;
                Tur = Convert.ToInt32(record.Inventorytype);
                this.Close();
            }
        }
    }
}
