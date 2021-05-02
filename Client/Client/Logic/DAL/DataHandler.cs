using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Client.Utils.Logging;
using Microsoft.Extensions.Logging;
using Model;
using Newtonsoft.Json;

namespace Client.Logic.DAL
{
    public class DataHandler : IImportExportHandler
    {
        private readonly ILogger logger = ApplicationLogging.CreateLogger<IImportExportHandler>();
        
        public async Task<(List<Tour>?, string)> ImportTours(string inputPath)
        {
            try
            {
                var tourData = await File.ReadAllTextAsync(inputPath);
                var tours = JsonConvert.DeserializeObject<List<Tour>>(tourData);
                return (tours, string.Empty);
            }
            catch (JsonException jsonEx)
            {
                logger.Log(LogLevel.Error, jsonEx.StackTrace);
                return (null, "Invalid Tour data given.");
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.StackTrace);
                return (null, "Could not read Tour data file.");
            }
        }

        public async Task<(bool, string)> ExportTours(string outputPath, List<Tour> tours)
        {
            try
            {
                // Export Tour data without Id property
                // See: https://stackoverflow.com/a/59227350/12347616
                var tourData = JsonConvert.SerializeObject(
                    tours, Formatting.Indented, 
                    new JsonSerializerSettings()
                    {ContractResolver = new IgnorePropertiesResolver(new[] {"Id"})});
                await File.WriteAllTextAsync(outputPath, tourData);
                return (true, string.Empty);
            }
            catch (JsonException jsonEx)
            {
                logger.Log(LogLevel.Warning, jsonEx.StackTrace);
                return (false, "Could not export Tour data\nPlease try again later.");
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Warning, ex.StackTrace);
                return (false, "Could not write Tour data file.");
            }
        }
    }
}