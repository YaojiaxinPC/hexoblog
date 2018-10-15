using System;
using System.Collections.Generic;

namespace SortDemo
{
    public class SortBaseClass : ISortMethod
    {
        public IList<int> GetResult(IList<int> sourcelist, out int countnum)
        {
            return CheckEffect(sourcelist, out countnum);
        }

        /// <summary>
        /// 进行数据的有效性判断
        /// </summary>
        /// <param name="sourcelist"></param>
        /// <returns></returns>
        private IList<int> CheckEffect(IList<int> sourcelist, out int countnum)
        {
            IList<int> resultlist = null;
            countnum = 0;
            if (sourcelist != null)
            {
                if (sourcelist.Count > 0)
                {
                    resultlist = Sort(sourcelist, out countnum);
                }
                else
                {
                    resultlist = sourcelist;
                }
            }

            return resultlist;
        }

        /// <summary>
        /// 由具体的算法部分进行重写
        /// </summary>
        /// <param name="sourcelist"></param>
        /// <returns></returns>
        protected virtual IList<int> Sort(IList<int> sourcelist, out int countnum)
        {
            countnum = 0;
            return sourcelist;
        }

        /// <summary>
        /// 控制台过程输出
        /// </summary>
        /// <param name="sourcelist"></param>
        /// <param name="n">交换的前面一个数的序号</param>
        /// <param name="m">交换的后面一个数的序号</param>
        protected void ConsoleOutProcess(IList<int> sourcelist, int n, int m)
        {
            for (int i = 0; i < sourcelist.Count; i++)
            {
                if (i < sourcelist.Count - 1)
                {
                    if (i == n || i == m)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else
                        Console.ForegroundColor = ConsoleColor.White;

                    Console.Write(sourcelist[i]);

                    if (i != n)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("-->");
                    }
                    else
                    {
                        Console.Write("<--");
                    }
                }
            }

            Console.ForegroundColor = ConsoleColor.White;
            if (sourcelist.Count - 1 == n || sourcelist.Count - 1 == m)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            Console.Write(sourcelist[sourcelist.Count - 1]);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }
    }
}
