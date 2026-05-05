using System;
using System.ComponentModel.DataAnnotations;

namespace MaliyeHesaplama.v2.Models
{
    public class ReceiptListDto
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Fiş No")]
        public string ReceiptNo { get; set; }

        [Display(Name = "Fiş Tipi")]
        public string ReceiptType { get; set; }

        [Display(Name = "Tarih")]
        public DateTime ReceiptDate { get; set; }

        [Display(Name = "Firma Id")]
        public int CompanyId { get; set; }

        [Display(Name = "Firma Adı")]
        public string CompanyName { get; set; }

        [Display(Name = "Firma Kodu")]
        public string CompanyCode { get; set; }

        [Display(Name = "Yetkili")]
        public string Authorized { get; set; }

        [Display(Name = "Düa Tarihi")]
        public DateTime? DuaDate { get; set; }

        [Display(Name = "Vade")]
        public string Maturity { get; set; }

        [Display(Name = "Müşteri Sipariş No")]
        public string CustomerOrderNo { get; set; }

        [Display(Name = "Açıklama")]
        public string Explanation { get; set; }

        [Display(Name = "Onay")]
        public int? Approved { get; set; }

        [Display(Name = "Kalem Id")]
        public int ReceiptItemId { get; set; }

        [Display(Name = "İşlem Tipi")]
        public string OperationType { get; set; }

        [Display(Name = "Malzeme Id")]
        public int InventoryId { get; set; }

        [Display(Name = "Malzeme Kodu")]
        public string InventoryCode { get; set; }

        [Display(Name = "Malzeme Adı")]
        public string InventoryName { get; set; }

        [Display(Name = "Metre")]
        public decimal NetMeter { get; set; }

        [Display(Name = "Kg")]
        public decimal NetWeight { get; set; }

        [Display(Name = "Adet")]
        public decimal Piece { get; set; }

        [Display(Name = "Nakit Ödeme")]
        public decimal CashPayment { get; set; }

        [Display(Name = "Vadeli Ödeme")]
        public decimal DeferredPayment { get; set; }

        [Display(Name = "Döviz")]
        public string Forex { get; set; }

        [Display(Name = "Varyant Id")]
        public int VariantId { get; set; }

        [Display(Name = "Varyant Kodu")]
        public string VariantCode { get; set; }

        [Display(Name = "Varyant")]
        public string Variant { get; set; }

        [Display(Name = "Satır Açıklama")]
        public string RowExplanation { get; set; }

        [Display(Name = "Depo Id")]
        public int WareHouseId { get; set; }

        [Display(Name = "Depo Kodu")]
        public string WareHouseCode { get; set; }

        [Display(Name = "Depo Adı")]
        public string WareHouseName { get; set; }

        [Display(Name = "Sipariş No")]
        public string OrderNo { get; set; }

        [Display(Name = "İrsaliye No")]
        public string TrackingNumber { get; set; }

        [Display(Name = "Birim Fiyat")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "KDV")]
        public decimal Vat { get; set; }

        [Display(Name = "Satır Tutar")]
        public decimal RowAmount { get; set; }

        [Display(Name = "Teslim Alan")]
        public string Receiver { get; set; }

        [Display(Name = "Belge Adı")]
        public string DocumentName { get; set; }

        [Display(Name = "Belge No")]
        public string InvoiceNo { get; set; }

        [Display(Name = "Belge Tarihi")]
        public DateTime? InvoiceDate { get; set; }

        [Display(Name = "İrsaliye No")]
        public string DispatchNo { get; set; }

        [Display(Name = "İrsaliye Tarihi")]
        public DateTime? DispatchDate { get; set; }
    }
}