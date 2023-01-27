using Chami.Db.Annotations;

namespace Chami.Db.Entities
{
    [TableName("Categories")]
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string BackgroundColor { get; set; }
        public bool Visibility { get; set; }
    }
}