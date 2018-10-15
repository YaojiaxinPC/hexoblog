using System;

namespace FeaturesDemo
{
    [AttributeUsage(AttributeTargets.All)]
    public sealed class MyAttribute : System.Attribute
    {
        public string Description { get; set; }
        public int LineNumber { get; set; }
        public bool IsClass { get; set; }

    }

    [My(Description = "测试Class", LineNumber = 16, IsClass = true)]
    public class TestMyClass
    {
        public string TestProperty { get; set; }
    }

    public class TestMyProperty
    {
        [My(Description = "测试Property", LineNumber = 24, IsClass = false)]
        public string TestProperty { get; set; }
    }


    public static class TestMyMethod
    {
        public static void GetFromClass(TestMyClass t)
        {
            Console.WriteLine(t.TestProperty);
            MyAttribute[] arrDesc = Attribute.GetCustomAttributes(t.GetType(), typeof(MyAttribute)) as MyAttribute[];
            if (arrDesc != null)
                foreach (var item in arrDesc)
                {
                    Console.WriteLine("Description:" + item.Description);
                    Console.WriteLine("LineNumber:" + item.LineNumber);
                    Console.WriteLine("IsClass:" + item.IsClass);
                    Console.WriteLine();
                }
        }

        public static void GetFromProperty(TestMyProperty t)
        {
            Console.WriteLine(t.TestProperty);
            foreach (System.Reflection.PropertyInfo item in t.GetType().GetProperties())
            {
                MyAttribute[] atts = Attribute.GetCustomAttributes(item, typeof(MyAttribute)) as MyAttribute[];
                if (atts == null) continue;
                foreach (var at in atts)
                {
                    Console.WriteLine("Description:" + at.Description);
                    Console.WriteLine("LineNumber:" + at.LineNumber);
                    Console.WriteLine("IsClass:" + at.IsClass);
                    Console.WriteLine();
                }
            }
        }
    }
}
