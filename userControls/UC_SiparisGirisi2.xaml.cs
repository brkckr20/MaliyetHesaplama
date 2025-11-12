using MaliyeHesaplama.Interfaces;
using MaliyeHesaplama.mvvm;
using System.Windows.Controls;

namespace MaliyeHesaplama.userControls
{
    public partial class UC_SiparisGirisi2 : UserControl, IPageCommands
    {
        public UC_SiparisGirisi2()
        {
            InitializeComponent();
            var vm = new MVM();
            this.DataContext = vm;
            ButtonBar.CommandTarget = vm;
        }

        public void Geri()
        {
            throw new NotImplementedException();
        }

        public void Ileri()
        {
            throw new NotImplementedException();
        }

        public void Kaydet()
        {
            throw new NotImplementedException();
        }

        public void Listele()
        {
            throw new NotImplementedException();
        }

        public void Sil()
        {
            throw new NotImplementedException();
        }

        public void Yazdir()
        {
            throw new NotImplementedException();
        }

        public void Yeni()
        {
            throw new NotImplementedException();
        }
    }
}
