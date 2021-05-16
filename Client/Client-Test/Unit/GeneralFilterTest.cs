using System;
using System.Collections.Generic;
using Client.Logic.BL;
using Client.Logic.DAL;
using Model;
using Moq;
using NUnit.Framework;
using Type = Model.Type;

namespace Client_Test.Unit
{
    /// <summary>
    /// Tests the <c>GeneralFilter</c> class invoked via the
    /// <c>TourPlannerClient</c> wrapper.
    /// </summary>
    public class GeneralFilterTest
    {
        private Tour tour = null!;
        
        [OneTimeSetUp]
        public void Setup()
        {
            tour = new Tour(
                1000,
                "A", "B",
                "TourA",
                22,
                "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.",
                new List<TourLog>()
                {
                    new TourLog(
                        DateTime.Now,
                        Type.Car,
                        TimeSpan.Zero,
                        100,
                        Rating.Great,
                        "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.",
                        55.5,
                        60,
                        100,
                        2
                    ),
                    new TourLog(
                        DateTime.Now,
                        Type.Vacation,
                        TimeSpan.Zero,
                        100,
                        Rating.Good,
                        "Great",
                        55.6,
                        60,
                        120,
                        3
                    )
                });
        }

        [OneTimeTearDown]
        public void Clear()
        {
        }
        
        [Test, TestCase(TestName = "Filter by valid Name", Description =
             "Filter by valid Name."
         )]
        public void FilterByValidName()
        {
            var api = new Mock<ITourApi>().Object;
            var handler = new Mock<IImportExportHandler>().Object;
            var filter = new GeneralFilter();
            var tp = new TourPlannerClient(api, handler, filter);

            tp.UpdateFilter("TourA");
            var ok = tp.FilterMethod(tour);

            Assert.IsTrue(ok);
        }
        
        [Test, TestCase(TestName = "Filter by valid From", Description =
             "Filter by valid From."
         )]
        public void FilterByValidFrom()
        {
            var api = new Mock<ITourApi>().Object;
            var handler = new Mock<IImportExportHandler>().Object;
            var filter = new GeneralFilter();
            var tp = new TourPlannerClient(api, handler, filter);

            tp.UpdateFilter("A");
            var ok = tp.FilterMethod(tour);

            Assert.IsTrue(ok);
        }
        
        [Test, TestCase(TestName = "Filter by valid Description", Description =
             "Filter by valid Description."
         )]
        public void FilterByValidDescription()
        {
            var api = new Mock<ITourApi>().Object;
            var handler = new Mock<IImportExportHandler>().Object;
            var filter = new GeneralFilter();
            var tp = new TourPlannerClient(api, handler, filter);

            tp.UpdateFilter("ed diam nonumy");
            var ok = tp.FilterMethod(tour);

            Assert.IsTrue(ok);
        }
        
        [Test, TestCase(TestName = "Filter by valid Type", Description =
             "Filter by valid Type."
         )]
        public void FilterByValidType()
        {
            var api = new Mock<ITourApi>().Object;
            var handler = new Mock<IImportExportHandler>().Object;
            var filter = new GeneralFilter();
            var tp = new TourPlannerClient(api, handler, filter);

            tp.UpdateFilter("Vacation");
            var ok = tp.FilterMethod(tour);

            Assert.IsTrue(ok);
        }
        
        [Test, TestCase(TestName = "Filter by valid Rating", Description =
             "Filter by valid Rating."
         )]
        public void FilterByValidRating()
        {
            var api = new Mock<ITourApi>().Object;
            var handler = new Mock<IImportExportHandler>().Object;
            var filter = new GeneralFilter();
            var tp = new TourPlannerClient(api, handler, filter);

            tp.UpdateFilter("Good");
            var ok = tp.FilterMethod(tour);

            Assert.IsTrue(ok);
        }
        
        [Test, TestCase(TestName = "Filter by valid Average Speed", Description =
             "Filter by valid Average Speed."
         )]
        public void FilterByValidAvgSpeed()
        {
            var api = new Mock<ITourApi>().Object;
            var handler = new Mock<IImportExportHandler>().Object;
            var filter = new GeneralFilter();
            var tp = new TourPlannerClient(api, handler, filter);

            tp.UpdateFilter("55.6");
            var ok = tp.FilterMethod(tour);

            Assert.IsTrue(ok);
        }
        
        [Test, TestCase(TestName = "Filter by invalid Name", Description =
             "Filter by invalid Name not present in Tour model."
         )]
        public void FilterByInvalidName()
        {
            var api = new Mock<ITourApi>().Object;
            var handler = new Mock<IImportExportHandler>().Object;
            var filter = new GeneralFilter();
            var tp = new TourPlannerClient(api, handler, filter);

            tp.UpdateFilter("TourB");
            var ok = tp.FilterMethod(tour);

            Assert.IsFalse(ok);
        }
        
        [Test, TestCase(TestName = "Filter by invalid Type", Description =
             "Filter by invalid Type not present in Tour model."
         )]
        public void FilterByInvalidType()
        {
            var api = new Mock<ITourApi>().Object;
            var handler = new Mock<IImportExportHandler>().Object;
            var filter = new GeneralFilter();
            var tp = new TourPlannerClient(api, handler, filter);

            tp.UpdateFilter("Bike");
            var ok = tp.FilterMethod(tour);

            Assert.IsFalse(ok);
        }
        
        [Test, TestCase(TestName = "Filter by invalid Rating", Description =
             "Filter by invalid Rating not present in Tour model."
         )]
        public void FilterByInvalidRating()
        {
            var api = new Mock<ITourApi>().Object;
            var handler = new Mock<IImportExportHandler>().Object;
            var filter = new GeneralFilter();
            var tp = new TourPlannerClient(api, handler, filter);

            tp.UpdateFilter("Terrible");
            var ok = tp.FilterMethod(tour);

            Assert.IsFalse(ok);
        }
    }
}