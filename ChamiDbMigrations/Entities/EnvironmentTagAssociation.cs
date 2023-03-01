using Chami.Db.Annotations;

namespace Chami.Db.Entities
{
    [TableName("EnvironmentTagAssociations")]
    public class EnvironmentTagAssociation : IChamiEntity
    {
        public int Id { get; set; }
        public int EnvironmentId { get; set; }
        public int TagId { get; set; }
        public bool IsUserDefined { get; set; }
    }
}