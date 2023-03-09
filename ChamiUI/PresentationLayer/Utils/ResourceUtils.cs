using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ChamiUI.PresentationLayer.Utils
{
    /// <summary>
    /// Helper functions and shared objects that have to do with resources in the application windows.
    /// </summary>
    public static class ResourceUtils
    {
        /// <summary>
        /// The default color for the progress bar in main window.
        /// </summary>
        public static Brush DefaultProgressBarColor => new SolidColorBrush(Color.FromArgb(0xFF, 0X06, 0xb0, 0x25));

        public static Brush ErrorProgressBarColor => Brushes.Red;
        
        /// <summary>
        /// Helper function that tries to get the specified resource and, if successful, sets the output parameter.
        /// </summary>
        /// <param name="resourceDictionary">The <see cref="ResourceDictionary"/> to extract from.</param>
        /// <param name="resourceName">The name of the resource we want to retrieve.</param>
        /// <param name="output">The requested resource, if found.</param>
        /// <returns>True if the requested resource has been found, otherwise false.</returns>
        /// <seealso cref="ResourceDictionary"/>
        public static bool TryGetResource(this ResourceDictionary resourceDictionary,  string resourceName, out object output)
        {
            output = resourceDictionary[resourceName];
            if (output != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Helper function that tries to get a CollectionViewSource from a resource dictionary.
        /// </summary>
        /// <param name="resourceDictionary"></param>
        /// <param name="resourceName"></param>
        /// <param name="output"></param>
        /// <returns></returns>
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