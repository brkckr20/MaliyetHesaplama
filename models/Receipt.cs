using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaliyeHesaplama.models
{
    public class Receipt
    {
        public int Id { get; set; }
        [Display(Name = "Sipariş No")]
        public string ReceiptNo { get; set; }
        [Display(Name = "Sipariş Tipi")]
        public int ReceiptType { get; set; }

        [Display(Name = "Sipariş Tarihi")]
        public DateTime ReceiptDate { get; set; }

        [Display(Name = "Firma Id")]
        public int CompanyId { get; set; }

        [Display(Name = "Açıklama")]
        public string Explanation { get; set; }

        [Display(Name = "Depo Id")]
        public int WareHouseId { get; set; }

        [Display(Name = "İrsaliye No")]
        public string InvoiceNo { get; set; }

        [Display(Name = "İrsaliye Tarihi")]
        public DateTime InvoiceDate { get; set; }

        [Display(Name = "Yetkili")]
        public string Authorized { get; set; }

        [Display(Name = "Termin Tarihi")]
        public DateTime DuaDate { get; set; }

        [Display(Name = "Vade")]
        public int Maturity { get; set; }
        [Display(Name = "Müşteri Sipariş No")]
        public string CustomerOrderNo { get; set; }
        [Display(Name = "Onaylı Mı?")]
        public bool Approved { get; set; }

        /*****************************/
        [NotMapped]
        [Display(Name = "Firma Adı")]
        public string CompanyName { get; set; }
        [NotMapped]
        [Display(Name = "Firma Kodu")]
        public string CompanyCode { get; set; }
        [NotMapped]
        [Display(Name = "Kalem Kayıt No")]
        public int ReceiptItemId { get; set; }
        [NotMapped]
        [Display(Name = "Malzeme Id")]
        public int InventoryId { get; set; }
        [NotMapped]
        [Display(Name = "Malzeme Kodu")]
        public string InventoryCode { get; set; }
        [NotMapped]
        [Display(Name = "Malzeme Adı")]
        public string InventoryName { get; set; }
        [NotMapped]
        [Display(Name = "Kalem İşlem")]
        public string OperationType { get; set; }
        [NotMapped]
        [Display(Name = "Varyant")]
        public string Variant { get; set; }
        [NotMapped]
        [Display(Name = "Net Metre")]
        public decimal NetMeter { get; set; }
        [NotMapped]
        [Display(Name = "Peşin Ödeme")]
        public decimal CashPayment { get; set; }
        [NotMapped]
        [Display(Name = "Vadeli Ödeme")]
        public decimal DeferredPayment { get; set; }
        [NotMapped]
        [Display(Name = "Döviz")]
        public string Forex { get; set; }
        [NotMapped]
        [Display(Name = "Satır Açıklama")]
        public string RowExplanation { get; set; }
        [NotMapped]
        [Display(Name = "Varyant Id")]
        public int VariantId { get; set; }
        [NotMapped]
        [Display(Name = "Varyant Kodu")]
        public string VariantCode { get; set; }
        [NotMapped]
        [Display(Name = "KG")]
        public decimal NetWeight { get; set; }
        [NotMapped]
        [Display(Name = "Adet")]
        public decimal Piece { get; set; }
        [NotMapped]
        [Display(Name = "Takip No")]
        public string TrackingNumber { get; set; }
        [NotMapped]
        [Display(Name = "Depo Kodu")]
        public string WareHouseCode { get; set; }
        [NotMapped]
        [Display(Name = "Depo Adı")]
        public string WareHouseName { get; set; }
        [NotMapped]
        [Display(Name = "Sipariş No Satır")]
        public string OrderNo { get; set; }
        [NotMapped]
        [Display(Name = "Birim Fiyat")]
        public decimal UnitPrice { get; set; }
        [NotMapped]
        [Display(Name = "KDV %")]
        public decimal Vat { get; set; }
        [NotMapped]
        [Display(Name = "Satır Tutarı")]
        public decimal RowAmount { get; set; }
        [NotMapped]
        [Display(Name = "Teslim Alan")]
        public string Receiver { get; set; }

    }
}
