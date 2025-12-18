using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaliyeHesaplama.models
{
    public class Color
    {
        public int Id { get; set; }
        [Display(Name = "Tip")]
        public int Type { get; set; }
        [Display(Name = "Kodu")]
        public string Code { get; set; }
        [Display(Name = "Tip Adı")]
        public string TypeName { get; set; }
        [Display(Name = "Adı")]
        public string Name { get; set; }
        [Display(Name = "Firma Id")]
        public int CompanyId { get; set; }
        [Display(Name = "Ana Renk Id")]
        public int ParentId { get; set; }
        [Display(Name = "Tarih")]
        public DateTime Date { get; set; }
        [Display(Name = "Talep Tarihi")]
        public DateTime RequestDate { get; set; }
        [Display(Name = "Onay Tarihi")]
        public DateTime ConfirmDate { get; set; }
        [Display(Name = "Pantone No")]
        public string PantoneNo { get; set; }
        [Display(Name = "Fiyat")]
        public decimal Price { get; set; }
        [Display(Name = "Döviz")]
        public string Forex { get; set; }
        [Display(Name = "Ana Renk Mi?")]
        public bool IsParent { get; set; }
        [Display(Name = "Kullanımda Mı?")]
        public bool IsUse { get; set; }
        [Display(Name = "Açıklama")]
        public string Explanation { get; set; }
        /*****************************/
        [NotMapped]
        [Display(Name = "Firma Adı")]
        public string CompanyName { get; set; }
        [NotMapped]
        [Display(Name = "Firma Kodu")]
        public string CompanyCode { get; set; }
    }
}
