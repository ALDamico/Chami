using System;

namespace ChamiUI.BusinessLayer.Annotations
{
    /// <summary>
    /// Marks a specific <see cref="SettingViewModel"/> as a setting that can only be updated explicitly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ExplicitSaveOnlyAttribute:Attribute
    {
        public ExplicitSaveOnlyAttribute(bool isExplicitSaveOnly = true)
        {
            IsExplicitSaveOnly = isExplicitSaveOnly;
        }
        /// <summary>
        /// The properties in this class cannot be saved to the database automatically, but need to be saved explicitly.
        /// </summary>
        public bool IsExplicitSaveOnly { get; }
    }
}