using System.ComponentModel.DataAnnotations;

namespace MaliyeHesaplama.helpers
{
    public static class Enums
    {
        public enum Inventory
        {
            Tumu = -1,
            Malzeme = 0,
            Kumas = 1,
            Iplik = 2,
        }
        public enum Receipt
        {
            [Display(Name = "Sipariş")]
            Siparis = 10,
            [Display(Name = "Kumaş Reçetesi")]
            KumasRecetesi = 11,
            [Display(Name = "Üretim Girişi")]
            UretimGirisi = 12,
            [Display(Name = "Malzeme Girişi")]
            MalzemeGiris = 13,
            [Display(Name = "Malzeme Çıkışı")]
            MalzemeCikis = 14,
        }
        public enum Depo
        {
            HamKumasDepo = 4,
        }
        public enum ColorAndPatternType
        {
            Kumas = 1,
            Iplik = 2,
            Desen = 3,
            Musteri = 4,
        }
    }
}
