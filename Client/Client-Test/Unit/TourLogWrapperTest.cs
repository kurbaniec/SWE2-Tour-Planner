using System;
using Client.ViewModels;
using NUnit.Framework;
using Type = Model.Type;

namespace Client_Test.Unit
{
    public class TourLogWrapperTest
    {
        [OneTimeSetUp]
        public void Setup()
        {
        }

        [Test, TestCase(TestName = "Check default constructor", Description =
             "Check default constructor values."
         )]
        public void DefaultConstructor()
        {
            var wrapper = new TourLogWrapper();
            Assert.AreEqual(DateTime.Today.Day, wrapper.Date.Day);
            Assert.AreEqual(Type.Car, wrapper.Type);
            Assert.AreEqual(TimeSpan.FromHours(1), wrapper.Duration);
            Assert.AreEqual(10, wrapper.Distance);
            Assert.AreEqual(10, wrapper.Rating);
            Assert.AreEqual("Report goes here...", wrapper.Report);
            Assert.AreEqual(10.0, wrapper.AvgSpeed);
            Assert.AreEqual(20.0, wrapper.MaxSpeed);
            Assert.AreEqual(100, wrapper.HeightDifference);
            Assert.AreEqual(0, wrapper.Stops);
        }
        
        [Test, TestCase(TestName = "Check save changes method", Description =
             "Changes should be reflected on the internal `TourLog` model."
         )]
        public void SaveChangesToModel()
        {
            var wrapper = new TourLogWrapper();
            var newDate = DateTime.Today - TimeSpan.FromDays(10);
            var newType = Type.Bicycle;
            var newDuration = TimeSpan.FromHours(2);
            var newDistance = 1000;
            var newRating = 0;
            var newReport = ":/";
            var newAvgSpeed = 15.5;
            var newMaxSpeed = 25.5;
            var newHeightDifference = 10;
            var newStops = 2;

            wrapper.Date = newDate;
            wrapper.Type = newType;
            wrapper.Duration = newDuration;
            wrapper.Distance = newDistance;
            wrapper.Rating = newRating;
            wrapper.Report = newReport;
            wrapper.AvgSpeed = newAvgSpeed;
            wrapper.MaxSpeed = newMaxSpeed;
            wrapper.HeightDifference = newHeightDifference;
            wrapper.Stops = newStops;
            wrapper.SaveChanges();
            var model = wrapper.Model;

            Assert.AreEqual(newDate, model.Date);
            Assert.AreEqual(newType, model.Type);
            Assert.AreEqual(newDuration, model.Duration);
            Assert.AreEqual(newDistance, model.Distance);
            Assert.AreEqual(newRating, model.Rating);
            Assert.AreEqual(newReport, model.Report);
            Assert.AreEqual(newAvgSpeed, model.AvgSpeed);
            Assert.AreEqual(newMaxSpeed, model.MaxSpeed);
            Assert.AreEqual(newHeightDifference, model.HeightDifference);
            Assert.AreEqual(newStops, model.Stops);
        }
        
        [Test, TestCase(TestName = "Check discard changes method", Description =
             "Changes should NOT be reflected on the internal `TourLog` model. " +
             "Also the Wrapper should be reset."
         )]
        public void DiscardChangesToModel()
        {
            var wrapper = new TourLogWrapper();
            var newDate = DateTime.Today - TimeSpan.FromDays(10);
            var newType = Type.Bicycle;
            var newDuration = TimeSpan.FromHours(2);
            var newDistance = 1000;
            var newRating = 0;
            var newReport = ":/";
            var newAvgSpeed = 15.5;
            var newMaxSpeed = 25.5;
            var newHeightDifference = 10;
            var newStops = 2;

            wrapper.Date = newDate;
            wrapper.Type = newType;
            wrapper.Duration = newDuration;
            wrapper.Distance = newDistance;
            wrapper.Rating = newRating;
            wrapper.Report = newReport;
            wrapper.AvgSpeed = newAvgSpeed;
            wrapper.MaxSpeed = newMaxSpeed;
            wrapper.HeightDifference = newHeightDifference;
            wrapper.Stops = newStops;
            wrapper.DiscardChanges();
            var model = wrapper.Model;

            Assert.AreNotEqual(newDate, wrapper.Date);
            Assert.AreNotEqual(newType, wrapper.Type);
            Assert.AreNotEqual(newDuration, wrapper.Duration);
            Assert.AreNotEqual(newDistance, wrapper.Distance);
            Assert.AreNotEqual(newRating, wrapper.Rating);
            Assert.AreNotEqual(newReport, wrapper.Report);
            Assert.AreNotEqual(newAvgSpeed, wrapper.AvgSpeed);
            Assert.AreNotEqual(newMaxSpeed, wrapper.MaxSpeed);
            Assert.AreNotEqual(newHeightDifference, wrapper.HeightDifference);
            Assert.AreNotEqual(newStops, wrapper.Stops);
            
            Assert.AreNotEqual(newDate, model.Date);
            Assert.AreNotEqual(newType, model.Type);
            Assert.AreNotEqual(newDuration, model.Duration);
            Assert.AreNotEqual(newDistance, model.Distance);
            Assert.AreNotEqual(newRating, model.Rating);
            Assert.AreNotEqual(newReport, model.Report);
            Assert.AreNotEqual(newAvgSpeed, model.AvgSpeed);
            Assert.AreNotEqual(newMaxSpeed, model.MaxSpeed);
            Assert.AreNotEqual(newHeightDifference, model.HeightDifference);
            Assert.AreNotEqual(newStops, model.Stops);
        }

        [Test, TestCase(TestName = "Check get request model method", Description =
             "Method is used to create a contract with the server for the updated TourLog"
         )]
        public void GetRequestModel()
        {
            var wrapper = new TourLogWrapper();

            var requestModel = wrapper.GetRequestTourLog();
            
            Assert.AreEqual(DateTime.Today.Day, requestModel.Date.Day);
            Assert.AreEqual(Type.Car, requestModel.Type);
            Assert.AreEqual(TimeSpan.FromHours(1), requestModel.Duration);
            Assert.AreEqual(10, requestModel.Distance);
            Assert.AreEqual(10, requestModel.Rating);
            Assert.AreEqual("Report goes here...", requestModel.Report);
            Assert.AreEqual(10.0, requestModel.AvgSpeed);
            Assert.AreEqual(20.0, requestModel.MaxSpeed);
            Assert.AreEqual(100, requestModel.HeightDifference);
            Assert.AreEqual(0, requestModel.Stops);
            
        }
    }
}