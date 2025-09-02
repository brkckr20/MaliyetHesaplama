using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


namespace MaliyeHesaplama.mvvm
{
    public partial class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
            Cozgu1 = new IplikBilgisiHesaplama { BolunenSayi = 1, BolenSayi = 1, Sonuc = 1 };
            Cozgu2 = new IplikBilgisiHesaplama { BolunenSayi = 1, BolenSayi = 1, Sonuc = 1 };
            Atki1 = new IplikBilgisiHesaplama { BolunenSayi = 1, BolenSayi = 1, Sonuc = 1 };
            Atki2 = new IplikBilgisiHesaplama { BolunenSayi = 1, BolenSayi = 1, Sonuc = 1 };
            Atki3 = new IplikBilgisiHesaplama { BolunenSayi = 1, BolenSayi = 1, Sonuc = 1 };
            Atki4 = new IplikBilgisiHesaplama { BolunenSayi = 1, BolenSayi = 1, Sonuc = 1 };
        }

        /* ÜRETİM BİLGİLERİ - İplik Bilgileri Hesaplamaları */
        [ObservableProperty]
        private IplikBilgisiHesaplama _cozgu1;
        [ObservableProperty]
        private IplikBilgisiHesaplama _cozgu2;
        [ObservableProperty]
        private IplikBilgisiHesaplama _atki1;
        [ObservableProperty]
        private IplikBilgisiHesaplama _atki2;
        [ObservableProperty]
        private IplikBilgisiHesaplama _atki3;
        [ObservableProperty]
        private IplikBilgisiHesaplama _atki4;
    }
}
