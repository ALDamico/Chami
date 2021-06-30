using System;

namespace ChamiUI.BusinessLayer.Annotations
{
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