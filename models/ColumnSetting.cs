namespace MaliyeHesaplama.models
{
    public class ColumnSetting
    {
        public int Id { get; set; }
        public string ColumnName { get; set; }
        public int? Width { get; set; }
        public bool Hidden { get; set; }
        public int? Location { get; set; }
        public int? UserId { get; set; }
        public string ScreenName { get; set; }
        public string GridName { get; set; }
        public ColumnSetting()
        {
        }
        public ColumnSetting(string columnName, bool hidden)
        {
            ColumnName = columnName;
            Hidden = hidden;
        }
        public bool IsVisible
        {
            get => !Hidden;
            set => Hidden = !value;
        }
    }
}
