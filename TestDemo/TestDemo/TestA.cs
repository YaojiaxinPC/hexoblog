using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDemo
{
    /// <summary>
    /// TestA a = new TestA(10);
    //TestA b = new TestA(a);
    //TestA c = null;
    //c = new TestA(c);
    //b.Printf();
    /// </summary>
    public class TestA
    {
        private int numa = 0;
        public TestA(int n)
        {
            numa = n;
        }

        public TestA(TestA a)
        {
            if (a == null) throw new NullReferenceException();
            numa = a.numa;
        }

        public void Printf()
        {
            Console.WriteLine("numa:" + numa);
        }
    }
}
