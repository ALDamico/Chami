using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using WPFLocalizeExtension.Providers;

namespace ChamiUI.Localization.LocalizationProviders;

public class XliffLocalizationProvider : ILocalizationProvider
{
    public XliffLocalizationProvider()
    {
        AvailableCultures = new ObservableCollection<CultureInfo>();
    }
    public FullyQualifiedResourceKeyBase GetFullyQualifiedResourceKey(string key, DependencyObject target)
    {
        throw new System.NotImplementedException();
    }

    public object GetLocalizedObject(string key, DependencyObject target, CultureInfo culture)
    {
        throw new System.NotImplementedException();
    }

    public ObservableCollection<CultureInfo> AvailableCultures { get; }
    public event ProviderChangedEventHandler ProviderChanged;
    public event ProviderErrorEventHandler ProviderError;
    public event ValueChangedEventHandler ValueChanged;
    
    
}