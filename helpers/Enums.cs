using System.ComponentModel.DataAnnotations;
using System.Reflection;

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
            Model = 3,
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

        public enum Messages
        {
            [Display(Name ="Rapor alabilmek için lütfen bir kayıt seçiniz!")]
            RaporSeciniz,
            [Display(Name = "Görüntülenecek başka bir kayıt bulunamadı!")]
            KayitBulunamadi,
            [Display(Name = "Onaylanmış sipariş üzerinde değişiklik yapamazsınız.\nDeğişiklik yapabilmek için lütfen yetkili ile iletişime geçiniz!")]
            DegisiklikOnayi,
            [Display(Name = "Kayıt işlemi başarılı bir şekilde gerçekleştirildi")]
            KayitBasarili
        }

        public static string GetDisplayName(this Enum enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            if (fieldInfo == null)
                return enumValue.ToString();

            var displayAttribute = fieldInfo.GetCustomAttribute<DisplayAttribute>();

            return displayAttribute?.Name ?? enumValue.ToString();
        }
    }
}
