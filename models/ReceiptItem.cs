using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaliyeHesaplama.models
{
    public class ReceiptItem
    {
        public int Id { get; set; }
        public int ReceiptId { get; set; }
        public string OperationType { get; set; }
        public int InventoryId { get; set; }
        public int GrM2 { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal NetWieght { get; set; }
    }
}
