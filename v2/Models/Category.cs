using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaliyeHesaplama.v2.Models
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(10)]
        [Display(Name = "Kodu")]
        public string Code { get; set; }

        [MaxLength(100)]
        [Display(Name = "Adı")]
        public string Name { get; set; }

        [Display(Name = "Üst Kategori")]
        public int? ParentId { get; set; }

        [Display(Name = "Aktif")]
        public bool IsActive { get; set; } = true;
    }
}