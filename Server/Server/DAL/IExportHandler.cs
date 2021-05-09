using Model;

namespace Server.DAL
{
    public interface IExportHandler
    {
        (string?, string) Export(Tour tour, string? imagePath);
        
        (string?, string) ExportSummary(Tour tour, string? imagePath);
    }
}