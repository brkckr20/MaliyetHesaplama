using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaliyeHesaplama.v2.Models
{
    [Table("MaterialMaster")]
    public class MaterialMaster
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Kodu")]
        public string Code { get; set; }

        [Required]
        [MaxLength(200)]
        [Display(Name = "Adı")]
        public string Name { get; set; }

        [Display(Name = "Tipi")]
        public int Type { get; set; }

        [Display(Name = "Kategorisi")]
        public int? CategoryId { get; set; }

        [Display(Name = "Birim")]
        public int UnitId { get; set; }

        [MaxLength(100)]
        [Display(Name = "Barkod")]
        public string Barcode { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        [Display(Name = "KDV (%)")]
        public decimal VatRate { get; set; } = 18;

        [Column(TypeName = "decimal(18,3)")]
        [Display(Name = "Min Stok")]
        public decimal MinStock { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        [Display(Name = "Max Stok")]
        public decimal MaxStock { get; set; }

        [Display(Name = "Aktif")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Güncelleme Tarihi")]
        public DateTime? UpdatedAt { get; set; }
    }

    public enum MaterialType
    {
        HamMadde = 1,
        YarıMamul = 2,
        Mamul = 3,
        SarfMalzeme = 4
    }
}