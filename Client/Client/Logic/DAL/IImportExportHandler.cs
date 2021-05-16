using System.Collections.Generic;
using System.Threading.Tasks;
using Model;

namespace Client.Logic.DAL
{
    /// <summary>
    /// Interfaces that defines serialization & deserialization of Tours with files.
    /// </summary>
    public interface IImportExportHandler
    {
        /// <summary>
        /// Deserializes Tours from earlier serialized, exported Tour data.
        /// </summary>
        /// <param name="inputPath">
        /// Path to the file with the exported Tour data.
        /// </param>
        /// <returns>
        /// Returns a tuple.
        /// On success a tuple with the deserialized Tours as item1 and a empty string as item2 is returned.
        /// On failure item1 is null and item2 will contain the error message.
        /// </returns>
        Task<(List<Tour>?, string)> ImportTours(string inputPath);

        /// <summary>
        /// Serializes and exports given Tours.
        /// </summary>
        /// <param name="outputPath">
        /// Path to the file where the Tour data should be exported.
        /// </param>
        /// <param name="tours">
        /// Tours that should be exported.
        /// </param>
        /// <returns>
        /// Returns a tuple.
        /// On success a tuple with true as item1 and a empty string as item2 is returned.
        /// On failure item1 is false and item2 will contain the error message.
        /// </returns>
        Task<(bool, string)> ExportTours(string outputPath, List<Tour> tours);
    }
}