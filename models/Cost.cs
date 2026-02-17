using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaliyeHesaplama.models
{
    public class Cost
    {
        public int Id { get; set; }
        [Display(Name = "Firma Id")]
        public int? CompanyId { get; set; }
        [Display(Name = "Tarih")]
        public DateTime? Date { get; set; }
        [Display(Name = "Malzeme Id")]
        public int? InventoryId { get; set; }
        [Display(Name = "Reçete Id")]
        public int? RecipeId { get; set; }
        [Display(Name = "Sipariş No")]
        public string OrderNo { get; set; }
        [Display(Name = "Ürün Görseli")]
        public byte[] ProductImage { get; set; }
        [Display(Name = "Kayıt Eden")]
        public int? InsertedBy { get; set; }
        [Display(Name = "Kayıt Tarihi")]
        public DateTime? InsertedDate { get; set; }
        [Display(Name = "Güncelleyen")]
        public int? UpdatedBy { get; set; }
        [Display(Name = "Güncellenme Tarihi")]
        public DateTime? UpdatedDate { get; set; }
        [Display(Name = "Tip")]
        public int? Type { get; set; }
        /// diğer alanlar
        [NotMapped]
        [Display(Name = "Firma Kodu")]
        public string CompanyCode { get; set; }

        [NotMapped]
        [Display(Name = "Firma Ünvanı")]
        public string CompanyName { get; set; }

        [NotMapped]
        [Display(Name = "Malzeme Kodu")]
        public string InventoryCode { get; set; }

        [NotMapped]
        [Display(Name = "Malzeme Adı")]
        public string InventoryName { get; set; }
        [NotMapped]
        [Display(Name = "Order Id")]
        public int ReceiptId { get; set; }
        [NotMapped]
        [Display(Name = "Sipariş_No")]
        public string ReceiptNo { get; set; }

    }
}
