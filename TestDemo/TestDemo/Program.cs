using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 1
            //Console.WriteLine("Starting Main");
            ////SingletonTest_inherit b = new SingletonTest_inherit();

            //SingletonTest.EchoAndReturn("Echo!");
            //Console.WriteLine("After echo");

            //string y = SingletonTest.x;
            //if (!string.IsNullOrEmpty(y))
            //{
            //    Console.WriteLine(y);
            //}
            ////y = SingletonTest_inherit.x;
            ////if (!string.IsNullOrEmpty(y))
            ////{
            ////    Console.WriteLine(y);
            ////}
            #endregion

            #region 2
            Console.WriteLine("Starting Main");

            string y = SingletonTest.Instance.XX;
            if (!string.IsNullOrEmpty(y))
            {
                Console.WriteLine(y);
            }
            #endregion

            #region 3
            //Console.WriteLine("Starting Main");
            ////SingletonTest_inherit b = new SingletonTest_inherit();

            //SingletonTest.EchoAndReturn("Echo!");
            //Console.WriteLine("After echo");

            //string y = SingletonTest.x;
            //if (!string.IsNullOrEmpty(y))
            //{
            //    Console.WriteLine(y);
            //}
            //y = SingletonTest_inherit.x;
            //if (!string.IsNullOrEmpty(y))
            //{
            //    Console.WriteLine(y);
            //}
            #endregion


            Console.ReadKey();
        }
    }
}
