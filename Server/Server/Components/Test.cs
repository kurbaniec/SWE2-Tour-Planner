using System;
using WebService_Lib.Attributes;

namespace Server.Components
{
    [Component]
    public class Test : ITest
    {
        public void Hi()
        {
            Console.WriteLine("Hi!");
        }
    }
}