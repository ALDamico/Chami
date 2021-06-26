namespace Chami.Db.Entities
{
    public class UiLanguage : IChamiEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string FlagPath { get; set; }
    }
}