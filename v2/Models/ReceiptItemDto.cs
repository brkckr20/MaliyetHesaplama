using System.ComponentModel.DataAnnotations;

namespace MaliyeHesaplama.v2.Models
{
    public class ReceiptItemDto
    {
        public int Id { get; set; }
        public int ReceiptId { get; set; }
        public int MaterialId { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public string OperationType { get; set; }
        public decimal Piece { get; set; }
        public decimal NetMeter { get; set; }
        public decimal NetWeight { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Vat { get; set; }
        public decimal RowAmount { get; set; }
        public string RowExplanation { get; set; }
    }
}