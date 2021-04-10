using NUnit.Framework;

namespace Client_Test.Unit
{
    public class TourWrapperTest
    {
        [OneTimeSetUp]
        public void Setup()
        {
        }

        [Test, TestCase(TestName = "Dummy", Description =
             "Dummy"
         )]
        public void BattlePlayerAWin()
        {
            Assert.IsTrue(true);
        }
    }
}