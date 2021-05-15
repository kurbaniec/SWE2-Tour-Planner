using Model;

namespace Server.DAL
{
    /// <summary>
    /// Interface that describes a way to create a printable document.
    /// </summary>
    public interface IExportHandler
    {
        /// <summary>
        /// Exports a printable document from a given Tour.
        /// </summary>
        /// <param name="tour">
        /// Tour that should be printed.
        /// </param>
        /// <param name="imagePath">
        /// Path to the image that should be used for the Route Image.
        /// </param>
        /// <param name="isSummary">
        /// Determines if a summary or full report is generated.
        /// </param>
        /// <returns>
        /// Returns a tuple.
        /// On success a tuple with the path to the document as item1 and a empty string as item2 is returned.
        /// On failure item1 is null and item2 will contain the error message.
        /// </returns>
        (string?, string) Export(Tour tour, string? imagePath, bool isSummary = false);
    }
}