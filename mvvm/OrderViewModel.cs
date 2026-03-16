using DocumentFormat.OpenXml.Drawing.Charts;
using MaliyeHesaplama.models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

public class OrderViewModel : INotifyPropertyChanged
{
    public OrderModel Order { get; set; } = new();

    public ICommand VaryantEkleCommand { get; set; }
    public ICommand BedenEkleCommand { get; set; }
    public ICommand VaryantSilCommand { get; set; }
    public ICommand BedenSilCommand { get; set; }
    public ICommand KaydetCommand { get; set; }

    private Varyant seciliVaryant;
    private Beden seciliBeden;
    private string yeniVaryantAdi;
    private string yeniBedenAdi;
    private int varyantSayisi;
    private int bedenSayisi;

    public Varyant SeciliVaryant
    {
        get => seciliVaryant;
        set
        {
            seciliVaryant = value;
            OnPropertyChanged(nameof(SeciliVaryant));
        }
    }

    public Beden SeciliBeden
    {
        get => seciliBeden;
        set
        {
            seciliBeden = value;
            OnPropertyChanged(nameof(SeciliBeden));
        }
    }

    public string YeniVaryantAdi
    {
        get => yeniVaryantAdi;
        set
        {
            yeniVaryantAdi = value;
            OnPropertyChanged(nameof(YeniVaryantAdi));
        }
    }

    public string YeniBedenAdi
    {
        get => yeniBedenAdi;
        set
        {
            yeniBedenAdi = value;
            OnPropertyChanged(nameof(YeniBedenAdi));
        }
    }

    public int VaryantSayisi
    {
        get => varyantSayisi;
        set
        {
            if (varyantSayisi != value)
            {
                varyantSayisi = value;
                OnPropertyChanged(nameof(VaryantSayisi));
            }
        }
    }

    public int BedenSayisi
    {
        get => bedenSayisi;
        set
        {
            if (bedenSayisi != value)
            {
                bedenSayisi = value;
                OnPropertyChanged(nameof(BedenSayisi));
            }
        }
    }

    public OrderViewModel()
    {
        VaryantEkleCommand = new RelayCommand(VaryantEkle);
        BedenEkleCommand = new RelayCommand(BedenEkle);
        VaryantSilCommand = new RelayCommand(VaryantSil);
        BedenSilCommand = new RelayCommand(BedenSil);
        KaydetCommand = new RelayCommand(Kaydet);

        // CollectionChanged event'lerini dinle
        Order.Varyantlar.CollectionChanged += (s, e) => VaryantSayisi = Order.Varyantlar.Count;
        Order.Bedenler.CollectionChanged += (s, e) => BedenSayisi = Order.Bedenler.Count;

        // Test verisi
        YukleTestVeri();
    }

    private void YukleTestVeri()
    {
        // Varyantlar
        Order.Varyantlar.Add(new Varyant { Id = 1, Ad = "Kapson", Renk = "HMW 663" });
        Order.Varyantlar.Add(new Varyant { Id = 2, Ad = "Biye", Renk = "HMW 620" });
        Order.Varyantlar.Add(new Varyant { Id = 3, Ad = "Desen", Renk = "HMW 660" });
        Order.Varyantlar.Add(new Varyant { Id = 4, Ad = "Diğer", Renk = "HMW 646" });

        // Bedenler
        Order.Bedenler.Add(new Beden { Id = 1, Ad = "80x80" });
        Order.Bedenler.Add(new Beden { Id = 2, Ad = "100x..." });
        Order.Bedenler.Add(new Beden { Id = 3, Ad = "140x..." });

        // Sayıları güncelle
        VaryantSayisi = Order.Varyantlar.Count;
        BedenSayisi = Order.Bedenler.Count;

        MaatrisOlustur();
    }

    private void VaryantEkle()
    {
        if (string.IsNullOrEmpty(YeniVaryantAdi))
            return;

        var yeniVaryant = new Varyant
        {
            Id = Order.Varyantlar.Count + 1,
            Ad = YeniVaryantAdi,
            Renk = "Renk Kodu"
        };
        Order.Varyantlar.Add(yeniVaryant);
        YeniVaryantAdi = string.Empty;
        MaatrisOlustur();
    }

    private void BedenEkle()
    {
        if (string.IsNullOrEmpty(YeniBedenAdi))
            return;

        var yeniBeden = new Beden
        {
            Id = Order.Bedenler.Count + 1,
            Ad = YeniBedenAdi
        };
        Order.Bedenler.Add(yeniBeden);
        YeniBedenAdi = string.Empty;
        MaatrisOlustur();
    }

    private void VaryantSil()
    {
        if (SeciliVaryant != null)
        {
            Order.Varyantlar.Remove(SeciliVaryant);
            MaatrisOlustur();
        }
    }

    private void BedenSil()
    {
        if (SeciliBeden != null)
        {
            Order.Bedenler.Remove(SeciliBeden);
            MaatrisOlustur();
        }
    }

    private void MaatrisOlustur()
    {
        Order.MatrisRows.Clear();

        foreach (var varyant in Order.Varyantlar)
        {
            var row = new MatrisRow { Varyant = varyant };

            foreach (var beden in Order.Bedenler)
            {
                row.Huceler.Add(new MatrisHucre { Miktar = 0 });
            }

            Order.MatrisRows.Add(row);
        }
    }

    private void Kaydet()
    {
        MessageBox.Show("Order kaydedildi!");
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class RelayCommand : ICommand
{
    private readonly Action execute;
    private readonly Func<bool> canExecute;

    public RelayCommand(Action execute, Func<bool> canExecute = null)
    {
        this.execute = execute;
        this.canExecute = canExecute;
    }

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public bool CanExecute(object parameter) => canExecute?.Invoke() ?? true;
    public void Execute(object parameter) => execute?.Invoke();
}