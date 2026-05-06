using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaliyeHesaplama.v2.Models
{
    [Table("AllLog")]
    public class AllLog
    {
        [Key]
        public int Id { get; set; }

        public int ReceiptId { get; set; }

        public int ReceiptType { get; set; }

        [MaxLength(50)]
        public string Operation { get; set; }

        public DateTime OperationDate { get; set; } = DateTime.Now;

        public int? UserId { get; set; }

        public int? CompanyId { get; set; }

        [MaxLength(100)]
        public string ComputerName { get; set; }

        [MaxLength(50)]
        public string ComputerIP { get; set; }

        public int? WareHouseId { get; set; }

        [MaxLength(50)]
        public string ReceiptNo { get; set; }

        [MaxLength(50)]
        public string InvoiceNo { get; set; }
    }
}