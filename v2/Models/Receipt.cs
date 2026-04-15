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
    }
}