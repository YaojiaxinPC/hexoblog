using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDemo
{
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

    //public class SingletonTest
    //{
    //    public static string x = EchoAndReturn("A_In type initializer");

    //    /// <summary>
    //    /// 当没有写静态构造函数时，框架会自动生成
    //    /// 导致静态字段的初始化跑到了静态方法调用之前
    //    /// 造成对象的提前初始化
    //    /// </summary>
    //    static SingletonTest()
    //    {
    //        x = "A_static";
    //        Console.WriteLine("A_static init");
    //    }

    //    public SingletonTest()
    //    {
    //        x = "A_nonstatic";
    //        Console.WriteLine("A_nonstatic init");
    //    }

    //    public static string EchoAndReturn(string s)
    //    {
    //        Console.WriteLine(s);
    //        return s;
    //    }
    //}

    //public class SingletonTest
    //{
    //    private static readonly SingletonTest _instance = new SingletonTest();
    //    static SingletonTest()
    //    {
    //        Console.WriteLine("static init console");
    //        x = "static init";
    //    }
    //    private SingletonTest()
    //    {
    //        Console.WriteLine("nonstatic init console");
    //        x = "nonstatic init";
    //    }
    //    public static SingletonTest Instance
    //    {
    //        get
    //        {
    //            Console.WriteLine("Instance");
    //            return _instance;
    //        }
    //    }
    //    //测试静态字段，对执行顺序无影响，实际使用应该为非static，同时操作应该通过XX，不能直接操作本字段
    //    private static string x = "xxxx";
    //    public string XX
    //    {
    //        get { return x; }
    //        private set { x = value; }
    //    }
    //}


    //public class SingletonTest_inherit : SingletonTest
    //{
    //    static SingletonTest_inherit()
    //    {
    //        x = "B_static";
    //        Console.WriteLine("B_static init");
    //    }
    //    public SingletonTest_inherit()
    //    {
    //        x = "B_nonstatic";
    //        Console.WriteLine("B_nonstatic init");
    //    }
    //}
}
