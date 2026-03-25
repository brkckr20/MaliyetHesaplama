using System.ComponentModel.DataAnnotations;

namespace MaliyeHesaplama.models
{
    public class GTIP
    {
        public int Id { get; set; }
        [Display(Name = "Firma Kodu")]
        public string Code { get; set; }
        [Display(Name = "Malzeme Adı")]
        public string Name { get; set; }
        [Display(Name = "Açıklama")]
        public string Explanation { get; set; }
        [Display(Name = "Kullanımda ?")]
        public bool IsUse { get; set; }
    }
}
