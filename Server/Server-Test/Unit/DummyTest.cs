using System.Collections.Generic;
using Moq;
using NUnit.Framework;

namespace Server_Test.Unit
{
    public class BattleTest
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