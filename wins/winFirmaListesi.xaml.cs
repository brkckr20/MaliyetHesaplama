using System.Windows;
using System.Windows.Media;

namespace MaliyeHesaplama.wins
{
    public partial class winFirmaListesi : Window
    {
        MiniOrm _orm = new MiniOrm();
        public int Id;
        public string FirmaKodu, FirmaUnvan, Adres1, Adres2, Adres3;

        private void sfDataGrid_CellDoubleTapped(object sender, Syncfusion.UI.Xaml.Grid.GridCellDoubleTappedEventArgs e)
        {
            var record = e.Record as dynamic;
            if (record != null)
            {
                Id = record.Id;
                FirmaKodu = record.CompanyCode;
                FirmaUnvan = record.CompanyName;
                Adres1 = record.AddressLine1;
                Adres2 = record.AddressLine2;
                Adres3 = record.AddressLine3;
                this.Close();
            }
        }

        public winFirmaListesi()
        {
            InitializeComponent();
            sfDataGrid.ItemsSource = _orm.GetAll<dynamic>("Company");            
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
                    case "CompanyCode":
                        e.Column.HeaderText = "Firma Kodu";
                        break;
                    case "CompanyName":
                        e.Column.HeaderText = "Firma Adı";
                        break;
                    case "AdressLine1":
                        e.Column.HeaderText = "Adres 1";
                        break;
                    case "AdressLine2":
                        e.Column.HeaderText = "Adres 2";
                        break;
                    case "AdressLine3":
                        e.Column.HeaderText = "Adres 3";
                        break;
                }
            };
        }
        
    }
}
