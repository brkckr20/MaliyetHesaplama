using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaliyeHesaplama.v2.Models
{
    [Table("AllLogItems")]
    public class AllLogItems
    {
        [Key]
        public int Id { get; set; }

        public int LogId { get; set; }

        public int? DocumentId { get; set; }

        public int? DocumentLineId { get; set; }

        [MaxLength(50)]
        public string OperationType { get; set; }

        public int? InventoryId { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal? GrM2 { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal? GrossWeight { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal? NetWeight { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal? GrossMeter { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal? NetMeter { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal? Piece { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal? Forex { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal? ForexPrice { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal? UnitPrice { get; set; }

        public int? VariantId { get; set; }

        public int? ColorId { get; set; }

        [MaxLength(500)]
        public string RowExplanation { get; set; }

        [MaxLength(100)]
        public string Receiver { get; set; }

        [MaxLength(20)]
        public string MeasurementUnit { get; set; }

        [MaxLength(100)]
        public string Brand { get; set; }

        [MaxLength(50)]
        public string BatchNo { get; set; }

        [MaxLength(50)]
        public string OrderNo { get; set; }

        [MaxLength(50)]
        public string CustomerOrderNo { get; set; }
    }
}