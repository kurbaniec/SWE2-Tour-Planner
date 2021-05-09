using Model;

namespace Server.DAL
{
    public interface IExportHandler
    {
        (string?, string) Export(Tour tour, string? imagePath, bool isSummary = false);
    }
}