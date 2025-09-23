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
            string filterText = aranacakTextbox.Text.ToLower();

            if (collectionView != null)
            {
                collectionView.Filter = item =>
                {
                    var dict = (IDictionary<string, object>)item;

                    // Eğer "CompanyName" property’si varsa
                    if (dict.ContainsKey(fieldAdi) && dict[fieldAdi] != null)
                    {
                        string companyName = dict[fieldAdi].ToString().ToLower();
                        return companyName.Contains(filterText);
                    }
                    return false;
                };
                collectionView.Refresh();
            }
        }
    }
}
