//#define step1
//#define step1needstatic
//#define staticsingleton
//#define step21
//#define step22
//#define IoDHsingleton
#define step3
using System;

namespace SingletonTestDemo
{

#if step1 || step22
    public class SingletonTest
    {
        public static string x = EchoAndReturn("A_In type initializer");

#if step1needstatic || step22
        /// <summary>
        /// 当没有写静态构造函数时，框架会自动生成
        /// 导致静态字段的初始化跑到了静态方法调用之前
        /// 造成对象的提前初始化
        /// </summary>
        static SingletonTest()
        {
            x = "A_static init";
            Console.WriteLine(x);
        }
#endif

        public SingletonTest()
        {
            x = "A_nonstatic init";
            Console.WriteLine(x);
        }

        public static string EchoAndReturn(string s)
        {
            Console.WriteLine(s);
            return s;
        }
    }
#endif

#if staticsingleton
    public class SingletonTest
    {
        private static readonly SingletonTest _instance = new SingletonTest();
        static SingletonTest()
        {
        }
        private SingletonTest()
        {
        }
        public static SingletonTest Instance
        {
            get
            {
                return _instance;
            }
        }
    }
#endif

#if step21
    public class SingletonTest
    {
        private static readonly SingletonTest _instance = new SingletonTest();
        static SingletonTest()
        {
            x = "static init console";
            Console.WriteLine(x);
        }
        private SingletonTest()
        {
            x = "nonstatic init console";
            Console.WriteLine(x);
        }
        public static SingletonTest Instance
        {
            get
            {
                Console.WriteLine("Instance");
                return _instance;
            }
        }
        //测试静态字段，对执行顺序无影响，实际使用应该为非static，同时操作应该通过XX，不能直接操作本字段
        private static string x = "xxxx";
        public string XX
        {
            get { return x; }
            private set { x = value; }
        }
    }
#endif

#if step22
    public class SingletonTest_inherit : SingletonTest
    {
        static SingletonTest_inherit()
        {
            x = "B_static";
            Console.WriteLine("B_static init");
        }
        public SingletonTest_inherit()
        {
            x = "B_nonstatic";
            Console.WriteLine("B_nonstatic init");
        }
    }
#endif

#if IoDHsingleton
    public class SingletonTest
    {
        private class InnerCLass
        {
            static InnerCLass() { }
            internal static SingletonTest instance = new SingletonTest();
        }
        private SingletonTest() { }
        public static SingletonTest Instance
        {
            get { return InnerCLass.instance; }
        }
    }
#endif

#if step3
    public class SingletonTest
    {
        private class InnerCLass
        {
            static InnerCLass()
            {
                Console.WriteLine("InnerCLass static");
            }
            internal static SingletonTest instance = new SingletonTest();
        }
        private SingletonTest()
        {
            Console.WriteLine("SingletonTest nonstatic");
        }
        public static SingletonTest Instance
        {
            get
            {
                Console.WriteLine("Instance");
                return InnerCLass.instance;
            }
        }
        public string XX = "aaa";
    }
#endif




}
