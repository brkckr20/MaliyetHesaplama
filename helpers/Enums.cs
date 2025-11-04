using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaliyeHesaplama.helpers
{
    public static class Enums
    {
        public enum Inventory
        {
            Tumu = -1,
            Malzeme = 0,
            Kumas = 1,
            Iplik = 2,
        }

        public enum Receipt
        {
            Siparis = 10
        }

        public enum Depo
        {
            HamKumasDepo = 0,
        }
    }
}
