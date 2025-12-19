using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaliyeHesaplama.models
{
    internal class WareHouse
    {
        public int Id { get; set; }
        [Display(Name = "Depo Kodu")]
        public string Code { get; set; }
        [Display(Name = "Depo Adı")]
        public string Name { get; set; }
        [Display(Name = "Kullanımda")]
        public bool IsUse { get; set; }
    }
}
