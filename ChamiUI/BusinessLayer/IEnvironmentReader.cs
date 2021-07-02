using System.Collections;
using System.Collections.Generic;

namespace ChamiUI.BusinessLayer
{
    /// <summary>
    /// Interface for classes to read environments from files.
    /// </summary>
    /// <typeparam name="TOut"></typeparam>
    public interface IEnvironmentReader<TOut>
    {
        /// <summary>
        /// Process a single environment.
        /// </summary>
        /// <returns>A single converted object.</returns>
        TOut Process();
        
        /// <summary>
        /// Process multiple environments
        /// </summary>
        /// <returns>An <see cref="ICollection{TOut}"/> of objects.</returns>
        ICollection<TOut> ProcessMultiple();
    }
}