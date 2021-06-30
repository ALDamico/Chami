using Chami.Db.Annotations;

namespace Chami.Db.Entities
{
    /// <summary>
    /// Represents a localization language for the Chami user interface.
    /// </summary>
    [TableName("UiLanguages")]
    public class UiLanguage : IChamiEntity
    {
        /// <summary>
        /// The four-letter ISO 639 code of the language.
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// The displayed name of the language.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The path to the language icon.
        /// Generally under the folder /ChamiUI;component/Assets/Flags
        /// </summary>
        public string FlagPath { get; set; }
    }
}