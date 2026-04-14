using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaliyeHesaplama.v2.Models
{
    [Table("Unit")]
    public class Unit
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(10)]
        [Display(Name = "Kodu")]
        public string Code { get; set; }

        [MaxLength(50)]
        [Display(Name = "Adı")]
        public string Name { get; set; }

        [Display(Name = "Aktif")]
        public bool IsActive { get; set; } = true;
    }
}