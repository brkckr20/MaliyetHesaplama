using System.ComponentModel.DataAnnotations;

namespace MaliyeHesaplama.v2.Models
{
    public class MaterialMasterDto
    {
        public int Id { get; set; }

        [Display(Name = "Kodu")]
        public string Code { get; set; }

        [Display(Name = "Adı")]
        public string Name { get; set; }

        public int Type { get; set; }

        [Display(Name = "Tipi")]
        public string TypeName { get; set; }

        public int? CategoryId { get; set; }

        [Display(Name = "Kategorisi")]
        public string CategoryName { get; set; }

        public int UnitId { get; set; }

        [Display(Name = "Birim")]
        public string UnitName { get; set; }

        [Display(Name = "Barkod")]
        public string Barcode { get; set; }

        [Display(Name = "KDV (%)")]
        public decimal VatRate { get; set; }

        [Display(Name = "Min Stok")]
        public decimal MinStock { get; set; }

        [Display(Name = "Max Stok")]
        public decimal MaxStock { get; set; }

        [Display(Name = "Aktif")]
        public bool IsActive { get; set; }

        [Display(Name = "Fiş No")]
        public string ReceiptNo { get; set; }

        [Display(Name = "Tarih")]
        public string ReceiptDate { get; set; }

        [Display(Name = "Firma Id")]
        public string CompanyId { get; set; }

        [Display(Name = "Firma Adı")]
        public string CompanyName { get; set; }

        [Display(Name = "Firma Kodu")]
        public string CompanyCode { get; set; }

        [Display(Name = "Açıklama")]
        public string Explanation { get; set; }

        [Display(Name = "Onaylı")]
        public string Approved { get; set; }

        [Display(Name = "Kalem Id")]
        public string ReceiptItemId { get; set; }

        [Display(Name = "İşlem Tipi")]
        public string OperationType { get; set; }

        [Display(Name = "Malzeme Id")]
        public string InventoryId { get; set; }

        [Display(Name = "Malzeme Kodu")]
        public string InventoryCode { get; set; }

        [Display(Name = "Malzeme Adı")]
        public string InventoryName { get; set; }

        [Display(Name = "Metre")]
        public string NetMeter { get; set; }

        [Display(Name = "Kg")]
        public string NetWeight { get; set; }

        [Display(Name = "Adet")]
        public string Piece { get; set; }

        [Display(Name = "Birim Fiyat")]
        public string UnitPrice { get; set; }

        [Display(Name = "KDV")]
        public string Vat { get; set; }

        [Display(Name = "Satır Tutar")]
        public string RowAmount { get; set; }

        [Display(Name = "Satır Açıklama")]
        public string RowExplanation { get; set; }

        [Display(Name = "Depo Id")]
        public string WareHouseId { get; set; }

        [Display(Name = "Depo Kodu")]
        public string WareHouseCode { get; set; }

        [Display(Name = "Depo Adı")]
        public string WareHouseName { get; set; }

        [Display(Name = "İrsaliye No")]
        public string TrackingNumber { get; set; }

        [Display(Name = "Sipariş No")]
        public string OrderNo { get; set; }

        [Display(Name = "Müşteri Sipariş No")]
        public string CustomerOrderNo { get; set; }

        [Display(Name = "Teslim Alan")]
        public string Receiver { get; set; }
    }
}