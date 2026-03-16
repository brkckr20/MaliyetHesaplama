using MaliyeHesaplama.models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaliyeHesaplama.mvvm
{
    public class MatrixViewModel : BaseViewModel
    {
        public ObservableCollection<string> VariantColumns { get; set; }
        public ObservableCollection<string> SizeColumns { get; set; }
        public ObservableCollection<MatrixRow> Rows { get; set; }
        public ObservableCollection<string> Colors { get; set; }
        public MatrixViewModel()
        {
            VariantColumns = new ObservableCollection<string>();
            SizeColumns = new ObservableCollection<string>();
            Rows = new ObservableCollection<MatrixRow>();
            Colors = new ObservableCollection<string>
            {
                "Kırmızı",
                "Beyaz",
                "Siyah",
                "Mavi",
                "Yeşil"
            };
            LoadSample();
        }
        private void LoadSample()
        {
            VariantColumns.Add("Ana Kumaş");
            VariantColumns.Add("Biye");

            SizeColumns.Add("50x50");
            SizeColumns.Add("100x100");

            var row = new MatrixRow();

            row.Cells["Ana Kumaş"] = "Kırmızı";
            row.Cells["Biye"] = "Beyaz";
            row.Cells["50x50"] = 120;
            row.Cells["100x100"] = 80;

            Rows.Add(row);
        }
    }
}
