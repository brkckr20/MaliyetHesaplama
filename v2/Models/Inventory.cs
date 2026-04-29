using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaliyeHesaplama.v2.Models
{
    [Table("Inventory")]
    public class Inventory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Kodu")]
        public string InventoryCode { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Adı")]
        public string InventoryName { get; set; }

        [Required]
        [MaxLength(20)]
        [Display(Name = "Birim")]
        public string Unit { get; set; }

        [Display(Name = "Tipi")]
        public int? Type { get; set; }

        [MaxLength(50)]
        [Display(Name = "Alt Tip")]
        public string SubType { get; set; }

        [Display(Name = "Kullanımda")]
        public bool IsUse { get; set; } = true;

        [Display(Name = "Ön Ek")]
        public bool? IsPrefix { get; set; }

        [Display(Name = "Stok Takibi")]
        public bool? IsStock { get; set; }

        [Display(Name = "Firma")]
        public int? CompanyId { get; set; }

        [Display(Name = "Kategori")]
        public int? CategoryId { get; set; }

        [Display(Name = "Cins")]
        public int? GenusId { get; set; }

        [MaxLength(50)]
        [Display(Name = "Özel Kod")]
        public string SpecialCode { get; set; }

        [MaxLength(50)]
        [Display(Name = "Özel Kod 2")]
        public string SpecialCode2 { get; set; }

        [Display(Name = "Gr/m²")]
        public int? GrM2 { get; set; }

        [Display(Name = "Kullanıcı")]
        public int? UserId { get; set; }

        [Display(Name = "Kumaş Uygun")]
        public bool? FabricOK { get; set; }

        [Display(Name = "Renk Uygun")]
        public bool? ColorOK { get; set; }

        [Display(Name = "Nakış Uygun")]
        public bool? EmbroideryOK { get; set; }

        [Display(Name = "İplik Uygun")]
        public bool? YarnOK { get; set; }

        [Display(Name = "Aksesuar Uygun")]
        public bool? AccessoriesOK { get; set; }

        [MaxLength(50)]
        [Display(Name = "GTIP No")]
        public string GTIPNo { get; set; }

        [Display(Name = "Resim")]
        public string Image { get; set; }

        [Column(TypeName = "decimal(18,0)")]
        [Display(Name = "Ham En")]
        public decimal? RawWidth { get; set; }

        [Column(TypeName = "decimal(18,0)")]
        [Display(Name = "Ham Boy")]
        public decimal? RawHeight { get; set; }

        [Column(TypeName = "decimal(18,0)")]
        [Display(Name = "Üretim En")]
        public decimal? ProdWidth { get; set; }

        [Column(TypeName = "decimal(18,0)")]
        [Display(Name = "Üretim Boy")]
        public decimal? ProdHeight { get; set; }

        [Column(TypeName = "decimal(18,0)")]
        [Display(Name = "Ham Gr/m²")]
        public decimal? RawGrammage { get; set; }

        [Column(TypeName = "decimal(18,0)")]
        [Display(Name = "Üretim Gr/m²")]
        public decimal? ProdGrammage { get; set; }

        [Display(Name = "İplik Boyalı")]
        public bool? YarnDyed { get; set; }

        [Display(Name = "Açıklama")]
        public string Explanation { get; set; }

        [Display(Name = "Kompozisyon")]
        public int? InventoryComposition { get; set; }

        [Display(Name = "Organik")]
        public bool? IsOrganic { get; set; }

        [MaxLength(255)]
        [Display(Name = "Birleşik Kod")]
        public string CombinedCode { get; set; }

        [Display(Name = "Env No")]
        public int? InventoryNo { get; set; }

        [Display(Name = "Cinsi")]
        public int? InventoryCinsi { get; set; }

        [MaxLength(255)]
        [Display(Name = "Orijinal Ad")]
        public string InventoryOriginalName { get; set; }

        [MaxLength(100)]
        [Display(Name = "Barkod")]
        public string Barcode { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        [Display(Name = "KDV (%)")]
        public decimal? VatRate { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        [Display(Name = "Min Stok")]
        public decimal? MinStock { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        [Display(Name = "Max Stok")]
        public decimal? MaxStock { get; set; }

        [Display(Name = "Oluşturulma")]
        public DateTime? CreatedAt { get; set; }

        [Display(Name = "Güncelleme")]
        public DateTime? UpdatedAt { get; set; }

        [NotMapped]
        public string CategoryName { get; set; }

        [NotMapped]
        public string UnitName { get; set; }
    }
}