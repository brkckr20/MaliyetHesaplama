using CommunityToolkit.Mvvm.ComponentModel;

namespace MaliyeHesaplama.mvvm
{
    public partial class DokumaBilgileriHesaplama : ObservableObject
    {
        [ObservableProperty]
        private double _carpanSayi;
        partial void OnCarpanSayiChanged(double value) => HesaplaVeYansit();
        [ObservableProperty]
        private double _carpim;
        partial void OnCarpimChanged(double value) => HesaplaVeYansit();
        [ObservableProperty]
        private double _sonuc;
        private void HesaplaVeYansit()
        {
            try
            {
                Sonuc = CarpanSayi * Carpim;
            }
            catch
            {
                Sonuc = 0;
            }
        }
    }
}
