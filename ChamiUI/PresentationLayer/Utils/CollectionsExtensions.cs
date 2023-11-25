using System.Collections.ObjectModel;

namespace ChamiUI.PresentationLayer.Utils;

public static class CollectionsExtensions
{
    public static void ReplaceInCollection<T>(this Collection<T> collection, T objectToRemove, T replacementObject)
    {
        var indexInCollection = collection.IndexOf(objectToRemove);
        if (indexInCollection == -1) return;
        collection.RemoveAt(indexInCollection);
        collection.Insert(indexInCollection, replacementObject);
    }
}