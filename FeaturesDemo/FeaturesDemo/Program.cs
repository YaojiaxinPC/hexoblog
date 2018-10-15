//#define ConsoleOut
using System;

namespace FeaturesDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestObsolete.OldMethod();

            //TestObsolete.ErrorMethod();

            //TestObsolete.NewMethod();

            //Console.WriteLine("1:");
            //TestConditional.ConsoleOut("AAAAAAAAAA");
            //Console.WriteLine("3:");

            //TestCall.CallMethod("Testtttttt");


            //Console.WriteLine(PersonSex.Man);
            //Console.WriteLine(EnumCN.GetDescription(PersonSex.Man));


            //EnumPower owner = EnumPower.AllNull;
            //owner = EnumPower.Delete | EnumPower.Create;

            //Console.WriteLine(owner);

            //Console.WriteLine("EnumPower.Delete:" + CheckPower.CheckHasPower(owner, EnumPower.Delete)+System.Environment.NewLine);            
            //Console.WriteLine("EnumPower.Read:" + CheckPower.CheckHasPower(owner, EnumPower.Read) + System.Environment.NewLine);
            //Console.WriteLine("EnumPower.AllNull:" + CheckPower.CheckHasPower(owner, EnumPower.AllNull) + System.Environment.NewLine);
            //Console.WriteLine("EnumPower.Delete | EnumPower.Update:" +
            //    CheckPower.CheckHasPower(owner, EnumPower.Delete | EnumPower.Update) + System.Environment.NewLine);
            //Console.WriteLine("EnumPower.AllNull | EnumPower.Update:" +
            //    CheckPower.CheckHasPower(owner, EnumPower.AllNull | EnumPower.Update) + System.Environment.NewLine);
            //Console.WriteLine("EnumPower.Update | EnumPower.Delete:" +
            //    CheckPower.CheckHasPower(owner, EnumPower.Update | EnumPower.Delete) + System.Environment.NewLine);
            //Console.WriteLine("EnumPower.Update &EnumPower.Delete:" +
            //    CheckPower.CheckHasPower(owner, EnumPower.Update, EnumPower.Delete) + System.Environment.NewLine);

            //Console.WriteLine("EnumPower.Read | EnumPower.Update | EnumPower.Create | EnumPower.Delete:" +
            //    CheckPower.CheckHasPower(owner, EnumPower.Read | EnumPower.Update | EnumPower.Create | EnumPower.Delete) + System.Environment.NewLine);

            //int enumlength = Enum.GetNames(typeof(EnumPower)).Length - 1;//0开始，所以要-1
            ////等价于前面全部|的操作
            //Console.WriteLine(((EnumPower)(1 << enumlength) - 1) + ":" +
            //    CheckPower.CheckHasPower(owner, ((EnumPower)(1 << enumlength) - 1)) + System.Environment.NewLine);


            //Console.WriteLine("EnumPower.Read &EnumPower.Update &EnumPower.Create &EnumPower.Delete:" +
            //    CheckPower.CheckHasPower(owner, EnumPower.Read, EnumPower.Update, EnumPower.Create, EnumPower.Delete));

            TestMyClass tc = new TestMyClass();
            tc.TestProperty = "getfromclass";
            TestMyMethod.GetFromClass(tc);
            TestMyProperty tp = new TestMyProperty();
            tp.TestProperty = "getfromproperty";
            TestMyMethod.GetFromProperty(tp);

            Console.ReadKey();
        }
    }
}
