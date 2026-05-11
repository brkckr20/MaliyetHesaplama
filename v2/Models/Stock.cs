using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaliyeHesaplama.v2.Models
{
    [Table("Stock")]
    public class Stock
    {
        [Key]
        public int Id { get; set; }

        public int InventoryId { get; set; }

        public int WareHouseId { get; set; }

        public int? VariantId { get; set; }

        [MaxLength(50)]
        public string BatchNo { get; set; }

        [MaxLength(50)]
        public string DyeColorNo { get; set; }

        public int? Variant2 { get; set; }

        [MaxLength(50)]
        public string OrderNo { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal QuantityKg { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal QuantityMeter { get; set; }

        public int QuantityPiece { get; set; }

        public int RollCount { get; set; }

        public DateTime LastUpdatedAt { get; set; }

        public int? StockId { get; set; }

        public int? ColorId { get; set; }

        [NotMapped]
        public string InventoryCode { get; set; }

        [NotMapped]
        public string InventoryName { get; set; }

        [NotMapped]
        public string Unit { get; set; }

        [NotMapped]
        public decimal UnitPrice { get; set; }

        [NotMapped]
        public decimal Vat { get; set; }

        [NotMapped]
        public decimal Quantity => QuantityKg > 0 ? QuantityKg : (QuantityMeter > 0 ? QuantityMeter : QuantityPiece);

        [NotMapped]
        public string WarehouseCode { get; set; }

        [NotMapped]
        public string WarehouseName { get; set; }

        [NotMapped]
        public DateTime? ReceiptDate { get; set; }

        [NotMapped]
        public int GirisAdet { get; set; }

        [NotMapped]
        public string Ids { get; set; }

        [NotMapped]
        public string Tarihler { get; set; }

        [NotMapped]
        public string FirstId { get; set; }

        //[NotMapped]
        //public decimal UnitPrice { get; set; }

        //[NotMapped]
        //public decimal Vat { get; set; }
    }
}