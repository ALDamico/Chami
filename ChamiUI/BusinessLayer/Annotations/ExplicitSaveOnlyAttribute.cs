using System;

namespace ChamiUI.BusinessLayer.Annotations
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ExplicitSaveOnlyAttribute:Attribute
    {
        public ExplicitSaveOnlyAttribute(bool isExplicitSaveOnly = true)
        {
            IsExplicitSaveOnly = isExplicitSaveOnly;
        }
        public bool IsExplicitSaveOnly { get; }
    }
}