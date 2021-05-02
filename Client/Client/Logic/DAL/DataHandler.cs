using System.Collections.Generic;
using System.Threading.Tasks;
using Model;

namespace Client.Logic.DAL
{
    public class DataHandler : IImportExportHandler
    {
        public Task<(List<Tour>?, string)> ImportTours(string inputPath)
        {
            throw new System.NotImplementedException();
        }

        public Task<(bool, string)> ExportTours(string outputPath, List<Tour> tours)
        {
            throw new System.NotImplementedException();
        }
    }
}