using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> numlst = new List<int>() { 23, 44, 66, 66, 76, 98, 11, 3, 9, 7 };
            ISortMethod sort = new QuickSortMethod();

            Console.WriteLine("原数列为：");
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < numlst.Count; i++)
            {
                str.Append(numlst[i] + "   ");
            }
            Console.WriteLine(str.Remove(str.Length - 3, 3).ToString());

            Console.WriteLine(System.Environment.NewLine);
            int countnum = 0;
            List<int> result = sort.GetResult(numlst, out countnum) as List<int>;

            Console.WriteLine(System.Environment.NewLine);
            Console.WriteLine("经过了{0}次数据对比 ", countnum);
            Console.WriteLine("排序后为：");
            str.Clear();
            for (int i = 0; i < result.Count; i++)
            {
                str.Append(result[i] + "-->");
            }
            Console.WriteLine(str.Remove(str.Length - 3, 3).ToString());

            Console.ReadLine();
        }
    }
}
