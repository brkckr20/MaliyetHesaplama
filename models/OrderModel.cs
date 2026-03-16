using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MaliyeHesaplama.models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public string UrunAdi { get; set; }
        public ObservableCollection<Varyant> Varyantlar { get; set; } = new();
        public ObservableCollection<Beden> Bedenler { get; set; } = new();
        public ObservableCollection<MatrisRow> MatrisRows { get; set; } = new();
    }

    // BaseViewModel - PropertyChanged için
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Her satırı temsil eder
    public class MatrisRow
    {
        public Varyant Varyant { get; set; }
        public ObservableCollection<MatrisHucre> Huceler { get; set; } = new();
    }

    public class Varyant
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Kod { get; set; }
        public string Renk { get; set; } // HMW 663, HMW 620 gibi
    }
    public class MatrisHucre : BaseViewModel
    {
        private int miktar;

        public int Miktar
        {
            get => miktar;
            set
            {
                if (miktar != value)
                {
                    miktar = value;
                    OnPropertyChanged(nameof(Miktar));
                }
            }
        }
    }

    public class Beden
    {
        public int Id { get; set; }
        public string Ad { get; set; } // "80x80", "100x...", "140x..." gibi
    }
    public class RenkBedenHucre
    {
        public int RenkId { get; set; }
        public int BedenId { get; set; }
        public string RenkAdi { get; set; }
        public string BedenAdi { get; set; }
        public int Miktar { get; set; }
    }
}
