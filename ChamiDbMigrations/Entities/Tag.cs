using Chami.Db.Annotations;

namespace Chami.Db.Entities
{
    [TableName("Tags")]
    public class Tag : IChamiEntity
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsUserDefined { get; set; }
    }
}