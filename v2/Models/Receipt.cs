using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaliyeHesaplama.v2.Models
{
    [Table("Receipt")]
    public class Receipt
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Fiş No")]
        public string ReceiptNo { get; set; }

        [Display(Name = "Fiş Tipi")]
        public int ReceiptType { get; set; }

        [Display(Name = "Fiş Tarihi")]
        public DateTime ReceiptDate { get; set; }

        [Display(Name = "Firma")]
        public int CompanyId { get; set; }

        [Display(Name = "Depo")]
        public int WareHouseId { get; set; }

        [Display(Name = "Açıklama")]
        public string Explanation { get; set; }

        [Display(Name = "Belge No")]
        public string InvoiceNo { get; set; }

        [Display(Name = "Belge Tarihi")]
        public DateTime? InvoiceDate { get; set; }

        [Display(Name = "İrsaliye No")]
        public string DispatchNo { get; set; }

        [Display(Name = "İrsaliye Tarihi")]
        public DateTime? DispatchDate { get; set; }

        [Display(Name = "Düa Tarihi")]
        public DateTime? DuaDate { get; set; }

        [Display(Name = "Yetkili")]
        public string Authorized { get; set; }

        [Display(Name = "Onaylı")]
        public bool? Approved { get; set; }

        [Display(Name = "Tamamlandı")]
        public bool? IsFinished { get; set; }

        [Display(Name = "Ödeme Tipi")]
        public string PaymentType { get; set; }

        [Display(Name = "Kaydeden")]
        public int? SavedUser { get; set; }

        [Display(Name = "Kaydetme Tarihi")]
        public DateTime? SavedDate { get; set; }

        [Display(Name = "Güncelleyen")]
        public int? UpdatedUser { get; set; }

        [Display(Name = "Güncelleme Tarihi")]
        public DateTime? UpdatedDate { get; set; }

        [Display(Name = "Taşıyıcı")]
        public int? CarrierId { get; set; }

        [Display(Name = "Nakliyeci")]
        public int? TransporterId { get; set; }

        [Display(Name = "Belge Adı")]
        public string DocumentName { get; set; }

        public byte[] Document { get; set; }

        [NotMapped]
        [Display(Name = "Kalem Id")]
        public string ReceiptItemId { get; set; }

        [NotMapped]
        [Display(Name = "İşlem Tipi")]
        public string OperationType { get; set; }

        [NotMapped]
        [Display(Name = "Malzeme Id")]
        public string InventoryId { get; set; }

        [NotMapped]
        [Display(Name = "Malzeme Kodu")]
        public string InventoryCode { get; set; }

        [NotMapped]
        [Display(Name = "Malzeme Adı")]
        public string InventoryName { get; set; }

        [NotMapped]
        [Display(Name = "Metre")]
        public string NetMeter { get; set; }

        [NotMapped]
        [Display(Name = "Kg")]
        public string NetWeight { get; set; }

        [NotMapped]
        [Display(Name = "Adet")]
        public string Piece { get; set; }

        [NotMapped]
        [Display(Name = "Birim Fiyat")]
        public string UnitPrice { get; set; }

        [NotMapped]
        [Display(Name = "KDV")]
        public string Vat { get; set; }

        [NotMapped]
        [Display(Name = "Satır Tutar")]
        public string RowAmount { get; set; }

        [NotMapped]
        [Display(Name = "Satır Açıklama")]
        public string RowExplanation { get; set; }

        [NotMapped]
        [Display(Name = "İrsaliye No")]
        public string TrackingNumber { get; set; }

        [NotMapped]
        [Display(Name = "Sipariş No")]
        public string OrderNo { get; set; }

        [NotMapped]
        [Display(Name = "Müşteri Sipariş No")]
        public string CustomerOrderNo { get; set; }

        [NotMapped]
        [Display(Name = "Teslim Alan")]
        public string Receiver { get; set; }

        [NotMapped]
        [Display(Name = "Vade")]
        public string Maturity { get; set; }
    }
}