using System;

namespace TestClosure
{
    class Program
    {
        static void Main(string[] args)
        {
            int tmp;
            for (int i = 0; i < 10; i++)
            {
                tmp = i;
                //Console.WriteLine("out :{0}____{1}", tmp,i);
                System.Threading.Thread.Sleep(1);
                System.Threading.ThreadPool.QueueUserWorkItem((o) =>
                {
                    Console.WriteLine("in :{0}____{1}", tmp, i);
                });
            }


            //test t = new test();
            //t.MethodA();
            //Console.WriteLine();
            //Console.WriteLine();
            //t.MethodB();

            Console.ReadLine();
        }
    }

    public class test
    {
        //地址不变
        public int num;
        //string的内容变时，实际是重新占用内存，然后存入新值，所以只要内容发生变化，内存肯定变
        public volatile string str; //原子操作 volatile

        private object lockstr = new object();
        private volatile string _str = "";
        public string Str
        {
            get { return _str; }
            set
            {
                lock (lockstr)
                {
                    _str = value;
                }
            }
        }


        public void MethodA()
        {
            for (int i = 0; i < 10; i++)
            {
                System.Threading.ThreadPool.QueueUserWorkItem((o) =>
                {
                    num = i;
                    Str = i.ToString();
                    GetAddr();
                });
            }
        }

        public void MethodB()
        {
            for (int i = 0; i < 10; i++)
            {
                //System.Threading.ThreadPool.QueueUserWorkItem((o) =>
                //{
                    num = i;
                    Str = i.ToString();
                    GetAddr();
                //});
            }
        }


        unsafe void GetAddr()
        {
            //这个是类对象，放在堆里面
            //fixed (int* p = &num)
            //{
            //    Console.WriteLine("{0}__:Address of numbe:0x{1:x}", num, (int)p);
            //}

            fixed (char* p = Str)
            {
                Console.WriteLine("{0}__:Address of char:0x{1:x}", Str, (int)p);
            }

            //GetAddrZ(num);
        }

        ////弄成函数传值的话，地址是变化的，因为是一份拷贝
        //unsafe void GetAddrZ(int n)
        //{
        //    //获取栈上变量的地址
        //    int* p = &n;
        //    Console.WriteLine("{0}__:Address of n:0x{1:x}\n", num, (int)p);
        //}
    }
}
