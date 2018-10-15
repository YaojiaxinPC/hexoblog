using System;
using System.Runtime.CompilerServices;

namespace FeaturesDemo
{
    public static class TestCall
    {
        [System.Diagnostics.DebuggerStepThrough]
        public static void CallMethod(string message, [CallerFilePath] string filepath = "", [CallerLineNumber]int linenumber = 0,
            [CallerMemberName]string membername = "")
        {
            Console.WriteLine(message);
            if (!string.IsNullOrEmpty(filepath))
                Console.WriteLine("filepath:" + filepath);
            if (!linenumber.Equals(0))
                Console.WriteLine("linenumber:" + linenumber);
            if (!string.IsNullOrEmpty(membername))
                Console.WriteLine("membername:" + membername);
        }
    }
}
