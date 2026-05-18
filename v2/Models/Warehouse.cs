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
        public bool IsUse { get; set; } = true;

        [MaxLength(200)]
        public string Address1 { get; set; }

        [MaxLength(200)]
        public string Address2 { get; set; }

        [MaxLength(50)]
        public string District { get; set; }

        [MaxLength(50)]
        public string City { get; set; }

        [MaxLength(10)]
        public string PostalCode { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }
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