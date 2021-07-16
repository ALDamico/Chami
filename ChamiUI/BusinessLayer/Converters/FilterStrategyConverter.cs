using Chami.Db.Entities;
using ChamiUI.PresentationLayer.Filtering;

namespace ChamiUI.BusinessLayer.Converters
{
    /// <summary>
    /// Converts an <see cref="IFilterStrategy"/> object to a string that can be saved as a <see cref="Setting"/> entity.
    /// </summary>
    public class FilterStrategyConverter : UnwrappingConverter<IFilterStrategy>
    {
        /// <summary>
        /// Get the Value representation of an <see cref="IFilterStrategy"/> for saving it to the datastore.
        /// </summary>
        /// <param name="filterStrategy"></param>
        /// <returns></returns>
        public string GetSettingValue(IFilterStrategy filterStrategy)
        {
            if (filterStrategy == null)
            {
                return null;
            }

            return filterStrategy.GetType().FullName;
        }
    }
}