using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaliyeHesaplama.models
{
    public class Inventory
    {
        public int Id { get; set; }
        public string InventoryCode { get; set; }
        public string InventoryName { get; set; }
        public string Unit { get; set; }
        public int Type { get; set; }
        public string SubType { get; set; }
    }
}
