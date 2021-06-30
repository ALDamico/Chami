using Chami.Db.Annotations;

namespace Chami.Db.Entities
{
    /// <summary>
    /// Represents a configurable aspect of the Chami application.
    /// </summary>
    [TableName("Settings")]
    public class Setting : IChamiEntity
    {
        /// <summary>
        /// The property in the SettingsViewModel to insert this <see cref="Setting"/> in.
        /// </summary>
        public string SettingName { get; set; }
        /// <summary>
        /// The fully-qualified class name of the ViewModel inside the <seealso cref="SettingsViewModel"/>
        /// </summary>
        public string ViewModelName { get; set; }
        /// <summary>
        /// The type to convert the setting to.
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// The value in string format of the setting.
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// The name of the property in the <see cref="ViewModelName">ViewModel</see> to set the value of.
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// The name of the assembly containing the <see cref="Value"/> class definition.
        /// It can be NULL if the class exists in the ChamiUI assembly.
        /// </summary>
        public string AssemblyName { get; set; }
        /// <summary>
        /// The class to use to convert the object to and from a format (i.e., string) that can be saved in the database.
        /// It can be NULL if the <seealso cref="Type"/> property is a CLR type (e.g., bool, string, or a numeric type.
        /// </summary>
        public string Converter { get; set; }
    }
}