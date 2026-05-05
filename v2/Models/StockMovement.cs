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

        public int? CompanyId { get; set; }

        public int? LotId { get; set; }

        public int? ColorId { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        [Display(Name = "Miktar")]
        public decimal Quantity { get; set; }

        [Display(Name = "Hareket Tipi")]
        public int MovementType { get; set; }

        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int ReceiptId { get; set; }

        public int ReceiptItemId { get; set; }

        public int VariantId { get; set; }

        [MaxLength(50)]
        public string BatchNo { get; set; }

        [MaxLength(50)]
        public string OrderNo { get; set; }

        public int UserId { get; set; }

        [MaxLength(50)]
        public string Operation { get; set; }
    }

    public enum MovementType
    {
        Giris = 1,
        Cikis = 2,
        Transfer = 3,
        Duzeltme = 4
    }
}