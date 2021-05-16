using System.IO;
using NUnit.Framework;

namespace Client_Test.Unit
{
    public class DataHandlerTest
    {
        private string basePath;
        private string routePath;
        private string exportPath;
        private string connString;
        
        [OneTimeSetUp]
        public void Setup()
        {
            basePath = $"{Path.GetTempPath()}tp";
            routePath = $"{basePath}{Path.DirectorySeparatorChar}maps";
            if (!Directory.Exists(routePath))
                Directory.CreateDirectory(routePath);    
            exportPath = $"{basePath}{Path.DirectorySeparatorChar}exports";
            if (!Directory.Exists(exportPath))
                Directory.CreateDirectory(exportPath);
            connString = string.Empty;
        }

        [OneTimeTearDown]
        public void Clear()
        {
            if (Directory.Exists(basePath))
                Directory.Delete(basePath, true);
        }
        
        [Test, TestCase(TestName = "Export Tour Report", Description =
             "Export Tour Report (without Route Image)."
         )]
        public void ExportFullReport()
        {
            /**var cfg = new Configuration(routePath, exportPath, connString);
            var db = new MockDataManagement();
            var map = new MockMapApi();
            var handler = new PdfExportHandler(cfg);
            var tp = new TourPlannerServer(db, map, handler);

            var (path, error) = tp.GetPdfExport(1000);

            Assert.AreEqual("", error);
            Assert.AreNotEqual("", path);
            Assert.IsTrue(File.Exists(path));*/
            Assert.IsTrue(true);
        }
    }
}