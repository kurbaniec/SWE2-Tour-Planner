using Model;
using WebService_Lib.Attributes;

namespace Server.DAL
{
    [Component]
    public class PdfExportHandler : IExportHandler
    {
        public (string?, string) Export(Tour tour)
        {
            return (null, string.Empty);
        }
    }
    
    
}