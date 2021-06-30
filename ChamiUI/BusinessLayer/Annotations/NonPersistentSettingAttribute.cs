using System;

namespace ChamiUI.BusinessLayer.Annotations
{
    /// <summary>
    /// Marks a property in the SettingsViewModel as "non persistent", meaning the data adapter won't even try to save it.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NonPersistentSettingAttribute: Attribute
    {
        public NonPersistentSettingAttribute()
        {
            IsNonPersistent = true;
        }
        public bool IsNonPersistent { get; set; }
    }
}