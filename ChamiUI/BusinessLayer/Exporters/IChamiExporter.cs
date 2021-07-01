using System.Threading.Tasks;

namespace ChamiUI.BusinessLayer.Exporters
{
    /// <summary>
    /// Generic interfaces for the exporters supported by the Chami application.
    /// </summary>
    public interface IChamiExporter
    {
        /// <summary>
        /// Exports a list of entities synchronously.
        /// </summary>
        /// <param name="filename">The name of the output file.</param>
        void Export(string filename);

        /// <summary>
        /// Exports a list of entities asynchronously.
        /// </summary>
        /// <param name="filename">The name of the output file.</param>
        /// <returns></returns>
        Task ExportAsync(string filename);
    }
}