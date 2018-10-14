//#define step1
//#define step21
//#define step22
#define step3
using System;

namespace SingletonTestDemo
{
    class Program
    {
#if step1
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Main");
            //使用静态类
            SingletonTest.EchoAndReturn("Echo!");
            Console.WriteLine("After echo");
            //取出静态类中的静态成员x
            string y = SingletonTest.x;
            if (!string.IsNullOrEmpty(y))
            {
                Console.WriteLine(y);
            }
            Console.ReadKey();
        }
#endif

#if step21
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Main");

            string y = SingletonTest.Instance.XX;
            if (!string.IsNullOrEmpty(y))
            {
                Console.WriteLine(y);
            }
            Console.ReadKey();
        }
#endif

#if step22
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Main");
            SingletonTest_inherit b = new SingletonTest_inherit();
            b = new SingletonTest_inherit();
            b = new SingletonTest_inherit();

            SingletonTest.EchoAndReturn("Echo!");
            Console.WriteLine("After echo");
            //取出静态类中的静态成员x
            string y = SingletonTest.x;
            if (!string.IsNullOrEmpty(y))
            {
                Console.WriteLine(y);
            }
            y = SingletonTest_inherit.x;
            if (!string.IsNullOrEmpty(y))
            {
                Console.WriteLine(y);
            }
            Console.ReadKey();
        }
#endif

#if step3
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Main");

            string y = SingletonTest.Instance.XX;
            if (!string.IsNullOrEmpty(y))
            {
                Console.WriteLine(y);
            }

            Console.ReadKey();
        }
#endif

    }
}
