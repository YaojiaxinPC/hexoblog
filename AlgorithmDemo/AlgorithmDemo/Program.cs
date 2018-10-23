using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgorithmDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            IGetResult method = new StringToInt();
            method.ConsoleOut();

            Console.ReadLine();
        }
    }
}
