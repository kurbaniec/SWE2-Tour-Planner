using System.Collections.Generic;
using System.IO;
using Model;
using NUnit.Framework;
using Server.BL;
using Server.DAL;
using Server.Setup;
using Server_Test.Unit.Mocks;

namespace Server_Test.Unit
{
    public class TourPlannerServerTest
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

        [Test, TestCase(TestName = "Invalid Tour Addition", Description =
             "Invalid Tour Addition because 'From' parameter is missing."
         )]
        public void InvalidTourAdditionEmptyFrom()
        {
            var cfg = new Configuration(routePath, exportPath, connString);
            var db = new MockDataManagement();
            var map = new MockMapApi();
            var handler = new PdfExportHandler(cfg);
            var tp = new TourPlannerServer(db, map, handler);
            var tour = new Tour(1000, string.Empty, "To", "Name", 10.0, "", 
                new List<TourLog>());

            var (_, error) = tp.AddTour(tour);

            Assert.AreNotEqual("", error);
        }
        
        [Test, TestCase(TestName = "Invalid Tour Addition", Description =
             "Invalid Tour Addition because 'To' parameter is missing."
         )]
        public void InvalidTourAdditionEmptyTo()
        {
            var cfg = new Configuration(routePath, exportPath, connString);
            var db = new MockDataManagement();
            var map = new MockMapApi();
            var handler = new PdfExportHandler(cfg);
            var tp = new TourPlannerServer(db, map, handler);
            var tour = new Tour(1000, "From", string.Empty, "Name", 10.0, "", 
                new List<TourLog>());

            var (_, error) = tp.AddTour(tour);

            Assert.AreNotEqual("", error);
        }
        
        [Test, TestCase(TestName = "Invalid Tour Addition", Description =
             "Invalid Tour Addition because 'From' and 'To' parameter are missing."
         )]
        public void InvalidTourAdditionEmptyFromAndTo()
        {
            var cfg = new Configuration(routePath, exportPath, connString);
            var db = new MockDataManagement();
            var map = new MockMapApi();
            var handler = new PdfExportHandler(cfg);
            var tp = new TourPlannerServer(db, map, handler);
            var tour = new Tour(1000, string.Empty, string.Empty, "Name", 10.0, "", 
                new List<TourLog>());

            var (_, error) = tp.AddTour(tour);

            Assert.AreNotEqual("", error);
        }
        
        [Test, TestCase(TestName = "Invalid Tour Update", Description =
             "Invalid Tour Update because 'From' parameter is missing."
         )]
        public void InvalidTourUpdateEmptyFrom()
        {
            var cfg = new Configuration(routePath, exportPath, connString);
            var db = new MockDataManagement();
            var map = new MockMapApi();
            var handler = new PdfExportHandler(cfg);
            var tp = new TourPlannerServer(db, map, handler);
            var tour = new Tour(1000, string.Empty, "To", "Name", 10.0, "", 
                new List<TourLog>());

            var (_, error) = tp.UpdateTour(tour);

            Assert.AreNotEqual("", error);
        }
        
        [Test, TestCase(TestName = "Invalid Tour Update", Description =
             "Invalid Tour Update because 'To' parameter is missing."
         )]
        public void InvalidTourUpdateEmptyTo()
        {
            var cfg = new Configuration(routePath, exportPath, connString);
            var db = new MockDataManagement();
            var map = new MockMapApi();
            var handler = new PdfExportHandler(cfg);
            var tp = new TourPlannerServer(db, map, handler);
            var tour = new Tour(1000, "From", string.Empty, "Name", 10.0, "", 
                new List<TourLog>());

            var (_, error) = tp.UpdateTour(tour);

            Assert.AreNotEqual("", error);
        }
        
        [Test, TestCase(TestName = "Invalid Tour Update", Description =
             "Invalid Tour Update because 'From' and 'To' parameter are missing."
         )]
        public void InvalidTourUpdateEmptyFromAndTo()
        {
            var cfg = new Configuration(routePath, exportPath, connString);
            var db = new MockDataManagement();
            var map = new MockMapApi();
            var handler = new PdfExportHandler(cfg);
            var tp = new TourPlannerServer(db, map, handler);
            var tour = new Tour(1000, string.Empty, string.Empty, "Name", 10.0, "", 
                new List<TourLog>());

            var (_, error) = tp.AddTour(tour);

            Assert.AreNotEqual("", error);
        }
        
        [Test, TestCase(TestName = "Invalid Tour Update", Description =
             "Invalid Tour Update because id of 0 is given (not allowed)."
         )]
        public void InvalidTourUpdatedUnsupportedId()
        {
            var cfg = new Configuration(routePath, exportPath, connString);
            var db = new MockDataManagement();
            var map = new MockMapApi();
            var handler = new PdfExportHandler(cfg);
            var tp = new TourPlannerServer(db, map, handler);
            var tour = new Tour(0, "From", "To", "Name", 10.0, "", 
                new List<TourLog>());

            var (_, error) = tp.UpdateTour(tour);

            Assert.AreNotEqual("", error);
        }
        
    }
}