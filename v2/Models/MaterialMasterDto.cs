using System.ComponentModel.DataAnnotations;

namespace MaliyeHesaplama.v2.Models
{
    public class MaterialMasterDto
    {
        public int Id { get; set; }

        [Display(Name = "Kodu")]
        public string Code { get; set; }

        [Display(Name = "Adı")]
        public string Name { get; set; }

        public int Type { get; set; }

        [Display(Name = "Tipi")]
        public string TypeName { get; set; }

        public int? CategoryId { get; set; }

        [Display(Name = "Kategorisi")]
        public string CategoryName { get; set; }

        public int UnitId { get; set; }

        [Display(Name = "Birim")]
        public string UnitName { get; set; }

        [Display(Name = "Barkod")]
        public string Barcode { get; set; }

        [Display(Name = "KDV (%)")]
        public decimal VatRate { get; set; }

        [Display(Name = "Min Stok")]
        public decimal MinStock { get; set; }

        [Display(Name = "Max Stok")]
        public decimal MaxStock { get; set; }

        [Display(Name = "Aktif")]
        public bool IsActive { get; set; }
    }
}