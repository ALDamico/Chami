namespace Chami.Db.Entities
{
    public class Setting
    {
        public string SettingName { get; set; }
        public string ViewModelName { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string PropertyName { get; set; }
        public string AssemblyName { get; set; }
        public string Converter { get; set; }
    }
}