using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaliyeHesaplama.Interfaces
{
    public interface IPageCommands
    {
        void Yeni();
        void Kaydet();
        void Sil();
        void Yazdir();
        void Ileri();
        void Geri();
        void Listele();
    }
}
