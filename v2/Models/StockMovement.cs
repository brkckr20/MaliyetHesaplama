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
        public int InventoryId { get; set; }

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

        [Column(TypeName = "decimal(18,4)")]
        public decimal DeltaKg { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal DeltaMeter { get; set; }

        public int DeltaPiece { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal BeforeKg { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal AfterKg { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal BeforeMeter { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal AfterMeter { get; set; }

        public int BeforePiece { get; set; }

        public int AfterPiece { get; set; }

        public int ReceiptId { get; set; }

        public int ReceiptItemId { get; set; }

        public int VariantId { get; set; }

        [MaxLength(50)]
        public string BatchNo { get; set; }

        [MaxLength(50)]
        public string OrderNo { get; set; }

        public int UserId { get; set; }

        public int StockId { get; set; }
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