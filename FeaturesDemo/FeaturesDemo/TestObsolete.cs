using System;

namespace FeaturesDemo
{
    public static class TestObsolete
    {
        [Obsolete("OldMethod,please use NewMethod", false)]
        public static void OldMethod()
        {
            Console.WriteLine("Old Method");
        }

        [Obsolete("Error Method,you can't use this method!", true)]
        public static void ErrorMethod()
        {
            Console.WriteLine("Error Method");
        }

        public static void NewMethod()
        {
            Console.WriteLine("New Method");
        }
    }
}
