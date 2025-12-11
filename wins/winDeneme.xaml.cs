using Stimulsoft.Report.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
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
    /// Interaction logic for winDeneme.xaml
    /// </summary>
    public partial class winDeneme : Window
    {
        public winDeneme()
        {
            InitializeComponent();
            var liste = new List<Hashtable>();

            var row1 = new Hashtable();
            row1["Ad"] = "Ahmet";
            row1["Yas"] = 25;

            var row2 = new Hashtable();
            row2["Ad"] = "Mehmet";
            row2["Yas"] = 30;

            var row3 = new Hashtable();
            row3["Ad"] = "Ayşe";
            row3["Yas"] = 22;

            liste.Add(row1);
            liste.Add(row2);
            liste.Add(row3);

            //grid.ItemsSource = liste;
        }
    }
}
