using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Client.Logic.BL;
using Client.Logic.DAL;
using Model;
using Moq;
using NUnit.Framework;

namespace Client_Test.Unit
{
    public class DataHandlerTest
    {
        private string basePath;

        [OneTimeSetUp]
        public void Setup()
        {
            basePath = $"{Path.GetTempPath()}tp";
            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath); 
        }

        [OneTimeTearDown]
        public void Clear()
        {
            if (Directory.Exists(basePath))
                Directory.Delete(basePath, true);
        }
        
        [Test, TestCase(TestName = "Export Tour Data", Description =
             "Serialize & Export Tour Data to filesystem."
         )]
        public async Task ExportTours()
        {
            var api = new Mock<ITourApi>().Object;
            var filter = new Mock<IFilter>().Object;
            var handler = new DataHandler();
            var tp = new TourPlannerClient(api, handler, filter);
            var tours = new List<Tour>()
            {
                new(1000, "From", "To", "Name", 10.0, "",
                    new List<TourLog>())
            };
            var path = $"{basePath}{Path.DirectorySeparatorChar}export.td";

            var (ok, error) = await tp.ExportTours(path, tours);
            
            Assert.AreEqual("", error);
            Assert.IsTrue(ok);
        }
        
        [Test, TestCase(TestName = "Export & Import Tour Data", Description =
             "Serialize, Export Tour Data to filesystem and Deserialize, Import it later on."
         )]
        public async Task ExportAndImportTours()
        {
            var api = new Mock<ITourApi>().Object;
            var filter = new Mock<IFilter>().Object;
            var handler = new DataHandler();
            var tp = new TourPlannerClient(api, handler, filter);
            var tours = new List<Tour>()
            {
                new(1000, "From", "To", "Name", 10.0, "",
                    new List<TourLog>())
            };
            var path = $"{basePath}{Path.DirectorySeparatorChar}export.td";

            var (ok, exportError) = await tp.ExportTours(path, tours);
            var (importedTours, importError) = await tp.ImportTours(path);
            
            Assert.AreEqual("", exportError);
            Assert.IsTrue(ok);
            Assert.AreEqual("", importError);
            Assert.IsNotNull(importedTours);
            // Do not check for equality because the id of imported Tours is always 0
            Assert.AreEqual(tours.First().From, importedTours!.First().From);
            Assert.AreEqual(tours.First().To, importedTours!.First().To);
            Assert.AreEqual(tours.First().Name, importedTours!.First().Name);
            Assert.AreEqual(tours.First().Distance, importedTours!.First().Distance);
            Assert.AreEqual(tours.First().Description, importedTours!.First().Description);
            Assert.AreEqual(tours.First().Logs, importedTours!.First().Logs);
        }
    }
}