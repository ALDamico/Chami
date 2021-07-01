namespace ChamiUI.BusinessLayer.Converters
{
    /// <summary>
    /// Interface for the converters used by the Chami application to convert between entities and viewmodels.
    /// </summary>
    /// <typeparam name="TFrom">The entity type.</typeparam>
    /// <typeparam name="TTo">The viewmodel type.</typeparam>
    public interface IConverter<TFrom, TTo>
    {
        /// <summary>
        /// Converts from a viewmodel to an entity.
        /// </summary>
        /// <param name="model">The viewmodel to convert from.</param>
        /// <returns>A converted entity.</returns>
        TFrom From(TTo model);
        /// <summary>
        /// Converts from an entity to a viewmodel.
        /// </summary>
        /// <param name="entity">The entity to convert from.</param>
        /// <returns>A converted viewmodel.</returns>
        TTo To(TFrom entity);
    }
}