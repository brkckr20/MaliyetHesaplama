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
        public string ColumnName { get; set; }
        public int Width { get; set; }
        public bool Hidden { get; set; }   // DB’de Hidden = 1 ise gizli, 0 ise görünür
        public int Location { get; set; }
        public int UserId { get; set; }
        public string ScreenName { get; set; }

        // CheckBox ile bağlanacak
        public bool IsSelected
        {
            get => !Hidden;   // Hidden = false ise göster, Hidden = true ise seçili değil
            set => Hidden = !value;
        }
    }
}
