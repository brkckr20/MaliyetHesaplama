using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaliyeHesaplama.v2.Models
{
    [Table("ReceiptItem")]
    public class ReceiptItem
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Fiş Id")]
        public int ReceiptId { get; set; }

        [Display(Name = "İşlem Tipi")]
        public string OperationType { get; set; }

        [Display(Name = "Malzeme")]
        public int MaterialId { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        [Display(Name = "Adet")]
        public decimal Piece { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        [Display(Name = "Metre")]
        public decimal NetMeter { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        [Display(Name = "Kg")]
        public decimal NetWeight { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Birim Fiyat")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "Fiyat Birimi")]
        public string MeasurementUnit { get; set; }

        public string MaterialCode { get; set; }

        public string MaterialName { get; set; }

        [Display(Name = "Açıklama")]
        public string RowExplanation { get; set; }

        [Display(Name = "İrsaliye No")]
        public string TrackingNumber { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        [Display(Name = "KDV (%)")]
        public decimal Vat { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Satır Tutar")]
        public decimal RowAmount { get; set; }

        [Display(Name = "Teslim Alan")]
        public string Receiver { get; set; }
    }
}