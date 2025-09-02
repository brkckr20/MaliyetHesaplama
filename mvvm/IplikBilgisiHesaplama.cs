using CommunityToolkit.Mvvm.ComponentModel;

namespace MaliyeHesaplama.mvvm
{
    public partial class IplikBilgisiHesaplama : ObservableObject
    {
        [ObservableProperty]
        private double _bolunenSayi;
        partial void OnBolunenSayiChanged(double value) => HesaplaVeYansit();
        [ObservableProperty]
        private double _bolenSayi;
        partial void OnBolenSayiChanged(double value) => HesaplaVeYansit();
        [ObservableProperty]
        private double _sonuc;
        private void HesaplaVeYansit()
        {
            try
            {
                if (BolenSayi != 0)
                {
                    Sonuc = BolunenSayi / BolenSayi;
                }
                else
                {
                    Sonuc = 0;
                }
            }
            catch
            {
                Sonuc = 0;
            }
        }
    }
}
