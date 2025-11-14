using MaliyeHesaplama.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MaliyeHesaplama.userControls
{
    /// <summary>
    /// Interaction logic for UC_RenkKarti.xaml
    /// </summary>
    public partial class UC_RenkKarti : UserControl,IPageCommands
    {
        public UC_RenkKarti()
        {
            InitializeComponent();
            ButtonBar.PageCommands = this;
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
            MessageBox.Show("renk");
        }
    }
}
