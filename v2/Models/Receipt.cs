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

        [Display(Name = "Belge Adı")]
        public string DocumentName { get; set; }

        public byte[] Document { get; set; }

        [Display(Name = "Aktif")]
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [NotMapped]
        [Display(Name = "Firma Adı")]
        public string CompanyName { get; set; }

        [NotMapped]
        [Display(Name = "Firma Kodu")]
        public string CompanyCode { get; set; }

        [NotMapped]
        [Display(Name = "Depo Adı")]
        public string WareHouseName { get; set; }

        [NotMapped]
        [Display(Name = "Depo Kodu")]
        public string WareHouseCode { get; set; }

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
        [Display(Name = "Onaylı")]
        public string Approved { get; set; }

        [NotMapped]
        [Display(Name = "Vade")]
        public string Maturity { get; set; }

        [NotMapped]
        [Display(Name = "Düa Tarihi")]
        public string DuaDate { get; set; }

        [NotMapped]
        [Display(Name = "Yetkili")]
        public string Authorized { get; set; }
    }
}