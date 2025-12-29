using System.ComponentModel.DataAnnotations;

namespace MaliyeHesaplama.models
{
    public class Inventory
    {
        public int Id { get; set; }

        [Display(Name ="Malzeme Kodu")]
        public string InventoryCode { get; set; }
        [Display(Name = "Malzeme Adı")]
        public string InventoryName { get; set; }
        [Display(Name = "Birim")] 
        public string Unit { get; set; }
        [Display(Name = "Tipi")]
        public int Type { get; set; }
        [Display(Name = "Alt Tipi")]
        public string SubType { get; set; }
        [Display(Name = "Ön Ek?")]
        public bool IsPrefix{ get; set; }
        [Display(Name = "Kullanımda")]
        public bool IsUse { get; set; }
    }
}
