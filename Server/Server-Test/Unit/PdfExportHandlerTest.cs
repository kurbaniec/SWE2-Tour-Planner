using System.Collections.Generic;
using System.IO;
using System.Linq;
using Moq;
using NUnit.Framework;
using Server.BL;
using Server.DAL;
using Server.Setup;
using Server_Test.Unit.Mocks;

namespace Server_Test.Unit
{
    public class PdfExportHandlerTest
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
             "Export Tour Report (without Route Image)"
         )]
        public void ExportFullReport()
        {
            var cfg = new Configuration(routePath, exportPath, connString);
            var db = new MockDataManagement();
            var map = new MockMapApi();
            var handler = new PdfExportHandler(cfg);
            var tp = new TourPlannerServer(db, map, handler);

            var (path, error) = tp.GetPdfExport(1000);

            Assert.AreEqual("", error);
            Assert.AreNotEqual("", path);
            Assert.IsTrue(File.Exists(path));
        }
        
        [Test, TestCase(TestName = "Export Tour Summary Report", Description =
             "Export Tour Summary Report (without Route Image)"
         )]
        public void ExportSummaryReport()
        {
            var cfg = new Configuration(routePath, exportPath, connString);
            var db = new MockDataManagement();
            var map = new MockMapApi();
            var handler = new PdfExportHandler(cfg);
            var tp = new TourPlannerServer(db, map, handler);

            var (path, error) = tp.GetPdfExport(1000, true);

            Assert.AreEqual("", error);
            Assert.AreNotEqual("", path);
            Assert.IsTrue(File.Exists(path));
        }
        
        [Test, TestCase(TestName = "Export Report for nonexistent Tour", Description =
             "Export Report for nonexistent Tour (MockDataManagement only has Tours 1000 & 1001)"
         )]
        public void ExportReportForNonexistentTour()
        {
            var cfg = new Configuration(routePath, exportPath, connString);
            var db = new MockDataManagement();
            var map = new MockMapApi();
            var handler = new PdfExportHandler(cfg);
            var tp = new TourPlannerServer(db, map, handler);

            var (path, error) = tp.GetPdfExport(-2, true);

            Assert.AreNotEqual("", error);
            Assert.IsNull(path);
        }
    }
}