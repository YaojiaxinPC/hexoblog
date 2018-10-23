using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgorithmDemo
{
    /// <summary>
    /// 斐波那契数列
    /// </summary>
    public class FibonacciNum : IGetResult
    {
        public void ConsoleOut()
        {
            Console.Write(GetNum(30));
        }

        private int GetNum(int index)
        {
            if (index <= 0) return 0;
            else if (index <= 2) return 1;
            else
                return (GetNum(index - 1) + GetNum(index - 2));

        }
    }
}