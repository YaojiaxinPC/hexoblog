using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDemo
{
    /*
    string assambly = System.Reflection.Assembly.GetEntryAssembly().FullName;
    AppDomain domain = AppDomain.CreateDomain("NewDomain");
    A.Number = 10;
    string nameOfA = typeof(A).FullName;
    A a = domain.CreateInstanceAndUnwrap(assambly, nameOfA) as A;
    a.SetNumber(20);
    Console.WriteLine("Number in class A is {0}",A.Number);

    B.Number = 10;
    string nameOfB = typeof(B).FullName;
    B b = domain.CreateInstanceAndUnwrap(assambly, nameOfB) as B;
    b.SetNumber(20);
    Console.WriteLine("Number in class B is {0}", B.Number);
    */


    [Serializable]
    internal class A : MarshalByRefObject
    {
        public static int Number;
        public void SetNumber(int value)
        {
            Number = value;
        }
    }

    [Serializable]
    internal class B
    {
        public static int Number;
        public void SetNumber(int value)
        {
            Number = value;
        }
    }
}
