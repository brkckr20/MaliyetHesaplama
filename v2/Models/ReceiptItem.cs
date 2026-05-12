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
        public int InventoryId { get; set; }

        [Display(Name = "Gr/m²")]
        public int? GrM2 { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Brüt Kg")]
        public decimal? GrossWeight { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Net Kg")]
        public decimal? NetWeight { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Brüt Metre")]
        public decimal? GrossMeter { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        [Display(Name = "Net Metre")]
        public decimal NetMeter { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "Adet")]
        public decimal Piece { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Döviz Fiyat")]
        public decimal? ForexPrice { get; set; }

        [Display(Name = "Döviz")]
        public string Forex { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "Birim Fiyat")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "Varyant")]
        public int? VariantId { get; set; }

        [Display(Name = "Renk")]
        public int? ColorId { get; set; }

        [Display(Name = "Açıklama")]
        public string Explanation { get; set; }

        [Display(Name = "UUID")]
        public string UUID { get; set; }

        [Display(Name = "İrsaliye No")]
        public string TrackingNumber { get; set; }

        [Display(Name = "Desen")]
        public int? PatternId { get; set; }

        [Display(Name = "Proses")]
        public int? ProcessId { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "Satır Tutar")]
        public decimal RowAmount { get; set; }

        [Display(Name = "KDV (%)")]
        public decimal Vat { get; set; }

        [Display(Name = "Teslim Alan")]
        public string Receiver { get; set; }

        [Display(Name = "Fiyat Birimi")]
        public string MeasurementUnit { get; set; }

        [Display(Name = "Fiş No")]
        public string ReceiptNo { get; set; }

        [Display(Name = "Marka")]
        public string Brand { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "Fire")]
        public decimal? Wastage { get; set; }

        [Display(Name = "Miktar")]
        public int? Quantity { get; set; }

        [Display(Name = "Varyant")]
        public string Variant { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Nakit Ödeme")]
        public decimal? CashPayment { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Vadeli Ödeme")]
        public decimal? DeferredPayment { get; set; }

        [Display(Name = "Satır Açıklama")]
        public string RowExplanation { get; set; }

        [Display(Name = "Parti No")]
        public string BatchNo { get; set; }

        [Display(Name = "Sipariş No")]
        public string OrderNo { get; set; }

        [Display(Name = "Müşteri Sipariş No")]
        public string CustomerOrderNo { get; set; }

        [Display(Name = "Maliyet Hesaplandı")]
        public bool? IsCostCalculated { get; set; }

        [NotMapped]
        public string InventoryCode { get; set; }

        [NotMapped]
        public string InventoryName { get; set; }

        [NotMapped]
        public string CompanyCode { get; set; }

        [NotMapped]
        public string CompanyName { get; set; }

        [NotMapped]
        public decimal KalanAdet { get; set; }

        public bool IsWithChip { get; set; }
    }
}
