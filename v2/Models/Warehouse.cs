using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaliyeHesaplama.v2.Models
{
    [Table("Warehouse")]
    public class Warehouse
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(20)]
        [Display(Name = "Kodu")]
        public string Code { get; set; }

        [MaxLength(100)]
        [Display(Name = "Adı")]
        public string Name { get; set; }

        [Display(Name = "Tipi")]
        public int WarehouseType { get; set; }

        [Display(Name = "Aktif")]
        public bool IsActive { get; set; } = true;
    }

    public enum WarehouseType
    {
        AnaDepo = 1,
        BoyamaFason = 2,
        KesimBirim = 3,
        UretimBirim = 4,
        SatisDepo = 5
    }
}