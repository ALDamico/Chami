using ChamiUI.BusinessLayer.Annotations;

namespace ChamiUI.PresentationLayer.ViewModels
{
    /// <summary>
    /// Marker class that gives the default <see cref="ExplicitSaveOnlyAttribute"/> value (false)
    /// </summary>
    [ExplicitSaveOnly(false)]
    public abstract class SettingCategoryViewModelBase : ViewModelBase
    {
        
    }
}