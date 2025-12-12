using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.ComponentModel;

namespace MaliyeHesaplama.wins
{

    public partial class winDemo : Window
    {
        public class Company
        {
            public int Id { get; set; }
            public string CompanyCode { get; set; }
            public string CompanyName { get; set; }
            public string AddressLine1 { get; set; }
            public string AddressLine2 { get; set; }
            public string AddressLine3 { get; set; }
        }
        MiniOrm _orm = new MiniOrm();
        public winDemo()
        {
            InitializeComponent();
            var source = _orm.GetAll<Company>("Company");
            gridim.ItemsSource = source;
        }

        private void gridim_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "CompanyCode":
                    e.Column.Header = "Firma Kodu";
                    break;
                case "CompanyName":
                    e.Column.Header = "Firma Ünvan";
                    break;
                case "AddressLine1":
                    e.Column.Header = "Adres 1";
                    break;
                case "AddressLine2":
                    e.Column.Header = "Adres 2";
                    break;
                case "AddressLine3":
                    e.Column.Header = "Adres 3";
                    break;
            }
        }
    }
}
