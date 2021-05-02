using System.Collections.Generic;
using System.Threading.Tasks;
using Model;

namespace Client.Logic.DAL
{
    public class DataHandler : IImportExportHandler
    {
        public async Task<(List<Tour>?, string)> ImportTours(string inputPath)
        {
            return (null, "");
        }

        public async Task<(bool, string)> ExportTours(string outputPath, List<Tour> tours)
        {
            return (false, "");
        }
    }
}