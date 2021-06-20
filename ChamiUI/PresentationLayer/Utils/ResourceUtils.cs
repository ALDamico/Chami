using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ChamiUI.PresentationLayer.Utils
{
    public static class ResourceUtils
    {
        public static Brush DefaultProgressBarColor => new SolidColorBrush(Color.FromArgb(0, 0X06, 0xb0, 0x25));
        public static bool TryGetResource(this ResourceDictionary resourceDictionary,  string resourceName, out object output)
        {
            output = resourceDictionary[resourceName];
            if (output != null)
            {
                return true;
            }

            return false;
        }

        public static bool TryGetCollectionViewSource(this ResourceDictionary resourceDictionary, string resourceName,
            out CollectionViewSource output)
        {
            var exists = resourceDictionary.TryGetResource(resourceName, out var obj);
            if (obj is CollectionViewSource collectionViewSource)
            {
                output = collectionViewSource;
                return exists;
            }

            output = null;
            return false;
        }
    }
}