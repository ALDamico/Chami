using Chami.Db.Annotations;

namespace Chami.Db.Entities
{
    [TableName("ColumnInfos")]
    public class ColumnInfo : IChamiEntity
    {
        public int Id { get; set; }
        public bool IsVisible { get; set; }
        public double ColumnWidth { get; set; }
        public string Binding { get; set; }
        public int? OrdinalPosition { get; set; }
        public string Header { get; set; }
        public string Converter { get; set; }
        public string ConverterParameter { get; set; }
        public string SettingName { get; set; }
    }
}