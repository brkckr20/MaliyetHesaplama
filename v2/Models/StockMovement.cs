using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaliyeHesaplama.v2.Models
{
    [Table("StockMovement")]
    public class StockMovement
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Malzeme")]
        public int MaterialId { get; set; }

        [Display(Name = "Depo")]
        public int WarehouseId { get; set; }

        public int? LotId { get; set; }

        public int? ColorId { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        [Display(Name = "Miktar")]
        public decimal Quantity { get; set; }

        [Display(Name = "Hareket Tipi")]
        public int MovementType { get; set; }

        [Display(Name = "Belge Tipi")]
        public int DocumentType { get; set; }

        [Display(Name = "Belge Id")]
        public int DocumentId { get; set; }

        [Display(Name = "Belge Kalem Id")]
        public int? DocumentLineId { get; set; }

        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public enum MovementType
    {
        Giris = 1,
        Cikis = 2,
        Transfer = 3,
        Duzeltme = 4
    }

    public enum StockDocumentType
    {
        Receipt = 1,
        Transfer = 2,
        Sayim = 3,
        FasonGonderi = 4,
        FasonDonusu = 5
    }
}