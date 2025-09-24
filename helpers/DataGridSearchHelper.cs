using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace MaliyeHesaplama.helpers
{
    public static class DataGridSearchHelper
    {
        public static void SearchWithTextboxValue(TextBox aranacakTextbox, string fieldAdi, ICollectionView collectionView)
        {
            //string filterText = aranacakTextbox.Text.ToLower();

            //if (collectionView != null)
            //{
            //    collectionView.Filter = item =>
            //    {
            //        var dict = (IDictionary<string, object>)item;

            //        if (dict.ContainsKey(fieldAdi) && dict[fieldAdi] != null)
            //        {
            //            string value = dict[fieldAdi].ToString().ToLower();
            //            return value.Contains(filterText);
            //        }
            //        return false;
            //    };

            //    // Filter’i her CollectionChanged sonrası tekrar uygulat
            //    collectionView.Refresh();
            //}
        }
    }
}
