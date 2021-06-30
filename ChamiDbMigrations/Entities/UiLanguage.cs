using Chami.Db.Annotations;

namespace Chami.Db.Entities
{
    [TableName("UiLanguages")]
    public class UiLanguage : IChamiEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string FlagPath { get; set; }
    }
}