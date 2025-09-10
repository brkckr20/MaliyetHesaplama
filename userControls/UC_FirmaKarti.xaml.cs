using Syncfusion.UI.Xaml.Grid;
using System.Windows.Controls;

namespace MaliyeHesaplama.userControls
{
    public partial class UC_FirmaKarti : UserControl
    {   //gride tıklayınca verileri ilgili text alanlarına yansıt ve diğer crud işlemlerini kontrol et. 10.09.2025
        private MiniOrm _orm;
        private int Id = 0;
        public UC_FirmaKarti()
        {
            InitializeComponent();
            _orm = new MiniOrm();
            sfDataGrid.ItemsSource = _orm.GetAll<dynamic>("Company");
            KolonlaraTurkceIsımVer();
        }
        private void btnKayit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var dict = new Dictionary<string, object>
            {
                { "Id",Id },
                {"CompanyCode", txtFirmaKodu.Text },
                {"CompanyName", txtFirmaUnvan.Text },
                {"AdressLine1", txtAdres1.Text },
                {"AdressLine2", txtAdres2.Text },
                {"AdressLine3", txtAdres3.Text },
            };
            Id = _orm.Save("Company", dict);
        }

        void KolonlaraTurkceIsımVer()
        {
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
