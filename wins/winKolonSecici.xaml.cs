using MaliyeHesaplama.models;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Controls;

namespace MaliyeHesaplama.wins
{

    public partial class winKolonSecici : Window
    {
        int _userId;
        string _screenName, _gridName;
        MiniOrm _orm = new MiniOrm();
        Dictionary<string, string> displayNames = new Dictionary<string, string>();
        public winKolonSecici(string screenName, int userId, string gridName)
        {
            InitializeComponent();
            _userId = userId;
            _screenName = screenName;
            _gridName = gridName;

            var properties = typeof(Inventory).GetProperties();
            foreach (var prop in properties)
            {
                var displayAttr = prop.GetCustomAttributes(typeof(DisplayAttribute), false)
                                     .FirstOrDefault() as DisplayAttribute;

                displayNames[prop.Name] = displayAttr != null ? displayAttr.Name : prop.Name;
            }
            var columns = _orm.GetAll<ColumnSelector>("ColumnSelector")
                              .Where(c => c.UserId == _userId &&
                                          c.ScreenName == _screenName &&
                                          c.GridName == _gridName)
                              .ToList();

            int currentCount = columns.Count;
            foreach (var prop in properties)
            {
                if (!columns.Any(c => c.ColumnName == prop.Name))
                {
                    currentCount++;
                    var newCol = new ColumnSelector
                    {
                        Id = 0,
                        ColumnName = prop.Name,
                        UserId = _userId,
                        ScreenName = _screenName,
                        GridName = _gridName,
                        Hidden = prop.Name == "InternalRef" ? true : false,
                        Width = 100,
                        Location = currentCount
                    };

                    var data = new Dictionary<string, object>
                    {
                        { "Id", 0 }, { "ColumnName", newCol.ColumnName }, { "UserId", newCol.UserId },
                        { "ScreenName", newCol.ScreenName }, { "GridName", newCol.GridName },
                        { "Hidden", newCol.Hidden }, { "Width", newCol.Width }, { "Location", newCol.Location }
                    };

                    int newId = _orm.Save("ColumnSelector", data);
                    newCol.Id = newId;
                    columns.Add(newCol);
                }
            }
            foreach (var col in columns.OrderBy(c => c.Location))
            {
                var displayName = displayNames.ContainsKey(col.ColumnName) ? displayNames[col.ColumnName] : col.ColumnName;

                var chk = new System.Windows.Controls.CheckBox
                {
                    Content = displayName,
                    IsChecked = !col.Hidden,
                    Tag = col
                };
                lstColumns.Items.Add(chk);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int locationIndex = 0;
            foreach (var item in lstColumns.Items)
            {
                if (item is System.Windows.Controls.CheckBox chk)
                {
                    if (chk.Tag is ColumnSelector col)
                    {
                        col.Hidden = chk.IsChecked != true;
                        col.Location = locationIndex++;

                        var data = new Dictionary<string, object>
                {
                    { "Id", col.Id },
                    { "ColumnName", col.ColumnName },
                    { "UserId", col.UserId },
                    { "ScreenName", col.ScreenName },
                    { "GridName", col.GridName },
                    { "Hidden", col.Hidden },
                    { "Location", col.Location }
                };
                        _orm.Save("ColumnSelector", data);
                    }
                }
            }
            //this.DialogResult = true;
            this.Close();
        }
    }
}
