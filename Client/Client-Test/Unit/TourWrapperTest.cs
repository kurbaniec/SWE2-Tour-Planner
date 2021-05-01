using Client.ViewModels;
using Model;
using NUnit.Framework;

namespace Client_Test.Unit
{
    public class TourWrapperTest
    {
        [OneTimeSetUp]
        public void Setup()
        {
        }

        [Test, TestCase(TestName = "Check save changes method", Description =
             "Changes should be reflected on the internal `Tour` model."
         )]
        public void SaveChangesToModel()
        {
            var model = new Tour("A", "B", "AB", 10, "Cool!");
            var wrapper = new TourWrapper(model, null!);
            var newFrom = "C";
            var newTo = "D";
            var newName = "CD";
            var newDistance = 20;
            var newDescription = "Super!";

            wrapper.From = newFrom;
            wrapper.To = newTo;
            wrapper.Name = newName;
            wrapper.Distance = newDistance;
            wrapper.Description = newDescription;
            wrapper.SaveChanges();
            
            Assert.AreEqual(newFrom, model.From);
            Assert.AreEqual(newTo, model.To);
            Assert.AreEqual(newName, model.Name);
            Assert.AreEqual(newDistance, model.Distance);
            Assert.AreEqual(newDescription, model.Description);
        }
        
        [Test, TestCase(TestName = "Check save changes method", Description =
             "Changes should be reflected on the internal `Tour` model." +
             "Now also manipulates tour logs."
         )]
        public void SaveChangesToModelWithLogs()
        {
            var model = new Tour("A", "B", "AB", 10, "Cool!");
            var wrapper = new TourWrapper(model, null!);
            var newFrom = "C";
            var newTo = "D";
            var newName = "CD";
            var newDistance = 20;
            var newDescription = "Super!";

            wrapper.From = newFrom;
            wrapper.To = newTo;
            wrapper.Name = newName;
            wrapper.Distance = newDistance;
            wrapper.Description = newDescription;
            wrapper.AddNewLog();
            wrapper.SaveChanges();
            
            Assert.AreEqual(newFrom, model.From);
            Assert.AreEqual(newTo, model.To);
            Assert.AreEqual(newName, model.Name);
            Assert.AreEqual(newDistance, model.Distance);
            Assert.AreEqual(newDescription, model.Description);
            Assert.AreEqual(1, model.Logs.Count);
        }
        
        [Test, TestCase(TestName = "Check discard changes method", Description =
             "Changes should NOT be reflected on the internal `Tour` model. " +
             "Also the Wrapper should be reset."
         )]
        public void DiscardChangesToModel()
        {
            var model = new Tour("A", "B", "AB", 10, "Cool!");
            var wrapper = new TourWrapper(model, null!);
            var newFrom = "C";
            var newTo = "D";
            var newName = "CD";
            var newDistance = 20;
            var newDescription = "Super!";

            wrapper.From = newFrom;
            wrapper.To = newTo;
            wrapper.Name = newName;
            wrapper.Distance = newDistance;
            wrapper.Description = newDescription;
            wrapper.DiscardChanges();
            
            Assert.AreNotEqual(newFrom, wrapper.From);
            Assert.AreNotEqual(newTo, wrapper.To);
            Assert.AreNotEqual(newName, wrapper.Name);
            Assert.AreNotEqual(newDistance, wrapper.Distance);
            Assert.AreNotEqual(newDescription, wrapper.Description);

            Assert.AreNotEqual(newFrom, model.From);
            Assert.AreNotEqual(newTo, model.To);
            Assert.AreNotEqual(newName, model.Name);
            Assert.AreNotEqual(newDistance, model.Distance);
            Assert.AreNotEqual(newDescription, model.Description);
        }

        [Test, TestCase(TestName = "Check discard changes method", Description =
             "Changes should NOT be reflected on the internal `Tour` model. " +
             "Also the Wrapper should be reset. Also tests Logs."
         )]
        public void DiscardChangesToModelWithLogs()
        {
            var model = new Tour("A", "B", "AB", 10, "Cool!");
            var wrapper = new TourWrapper(model, null!);
            var newFrom = "C";
            var newTo = "D";
            var newName = "CD";
            var newDistance = 20;
            var newDescription = "Super!";

            wrapper.From = newFrom;
            wrapper.To = newTo;
            wrapper.Name = newName;
            wrapper.Distance = newDistance;
            wrapper.Description = newDescription;
            wrapper.AddNewLog();
            wrapper.DiscardChanges();
            
            Assert.AreNotEqual(newFrom, wrapper.From);
            Assert.AreNotEqual(newTo, wrapper.To);
            Assert.AreNotEqual(newName, wrapper.Name);
            Assert.AreNotEqual(newDistance, wrapper.Distance);
            Assert.AreNotEqual(newDescription, wrapper.Description);
            Assert.AreEqual(0, wrapper.Logs.Count);
            
            Assert.AreNotEqual(newFrom, model.From);
            Assert.AreNotEqual(newTo, model.To);
            Assert.AreNotEqual(newName, model.Name);
            Assert.AreNotEqual(newDistance, model.Distance);
            Assert.AreNotEqual(newDescription, model.Description);
            Assert.AreEqual(0, model.Logs.Count);
        }
        
        [Test, TestCase(TestName = "Check get request model method", Description =
             "Method is used to create a contract with the server for the updated Tour."
         )]
        public void GetRequestModel()
        {
            var model = new Tour("A", "B", "AB", 10, "Cool!");
            var wrapper = new TourWrapper(model, null!);

            var requestModel = wrapper.GetRequestTour();
            
            Assert.AreEqual("A", requestModel.From);
            Assert.AreEqual("B", requestModel.To);
            Assert.AreEqual("AB", requestModel.Name);
            Assert.AreEqual(10, requestModel.Distance);
            Assert.AreEqual("Cool!", requestModel.Description);
            Assert.AreEqual(0, requestModel.Logs.Count);
        }
        
        [Test, TestCase(TestName = "Check get request model method", Description =
             "Method is used to create a contract with the server for the updated Tour. " +
             "Also tests logs"
         )]
        public void GetRequestModelWithLogs()
        {
            var model = new Tour("A", "B", "AB", 10, "Cool!");
            var wrapper = new TourWrapper(model, null!);
            wrapper.AddNewLog();
            wrapper.SaveChanges();

            var requestModel = wrapper.GetRequestTour();
            
            Assert.AreEqual("A", requestModel.From);
            Assert.AreEqual("B", requestModel.To);
            Assert.AreEqual("AB", requestModel.Name);
            Assert.AreEqual(10, requestModel.Distance);
            Assert.AreEqual("Cool!", requestModel.Description);
            Assert.AreEqual(1, requestModel.Logs.Count);
        }

    }
}