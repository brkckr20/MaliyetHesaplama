using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaliyeHesaplama.models
{
    public class Receipt
    {
        public int Id { get; set; }
        public string ReceiptNo { get; set; }
        public int ReceiptType { get; set; }
        public DateTime ReceiptDate { get; set; }
        public int CompanyId { get; set; }
        public string Explanation { get; set; }
        public int WareHouseId { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        //public ObservableCollection<ReceiptItem> Items = new ObservableCollection<ReceiptItem>();
    }
}
