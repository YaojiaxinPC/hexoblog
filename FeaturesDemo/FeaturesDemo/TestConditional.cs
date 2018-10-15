using System;
using System.Diagnostics;

namespace FeaturesDemo
{
    public class TestConditional
    {
        [Conditional("ConsoleOut")]
        public static void ConsoleOut(string message)
        {
            Console.WriteLine(message);
        }
    }
}
