using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaliyeHesaplama.v2.Models
{
    [Table("ReceiptLog")]
    public class ReceiptLog
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Depo")]
        public int WareHouseId { get; set; }

        [Display(Name = "Fiş Tipi")]
        public int ReceiptType { get; set; }

        public int ReceiptId { get; set; }

        public int ReceiptItemId { get; set; }

        [Display(Name = "Malzeme")]
        public int InventoryId { get; set; }

        public int? ColorId { get; set; }

        public int? VariantId { get; set; }

        [MaxLength(50)]
        public string Operation { get; set; }

        [Display(Name = "İşlem Tarihi")]
        public DateTime OperationDate { get; set; } = DateTime.Now;

        public int? UserId { get; set; }

        public int? CompanyId { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal? GrossKg { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal? GrossMeter { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal? NetKg { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal? NetMeter { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal? Piece { get; set; }
    }
}