using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaliyeHesaplama.models
{
    public class ColumnSelector
    {
        public int Id { get; set; }
        public string ColumnName { get; set; } // Modeldeki Property Adı
        public int Width { get; set; }
        public bool Hidden { get; set; }
        public int Location { get; set; } // DisplayIndex
        public int UserId { get; set; }
        public string ScreenName { get; set; } // Örneğin: "StokGirisEkrani"
        public string GridName { get; set; } // Örneğin: "AnaStokGridi"

        // CheckBox ile bağlanacak
        public bool IsSelected
        {
            get => !Hidden;   // Hidden = false ise göster, Hidden = true ise seçili değil
            set => Hidden = !value;
        }
    }
}
