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
        public string Code { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        public int Type { get; set; }

        public int? CategoryId { get; set; }

        public int UnitId { get; set; }

        [MaxLength(100)]
        public string Barcode { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal VatRate { get; set; } = 18;

        [Column(TypeName = "decimal(18,3)")]
        public decimal MinStock { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal MaxStock { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

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