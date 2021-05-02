using System.Collections.Generic;
using System.Threading.Tasks;
using Model;

namespace Client.Logic.DAL
{
    public interface IImportExportHandler
    {
        Task<(List<Tour>?, string)> ImportTours(string inputPath);

        Task<(bool, string)> ExportTours(string outputPath, List<Tour> tours);
    }
}