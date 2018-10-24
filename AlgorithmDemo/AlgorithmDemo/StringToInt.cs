using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgorithmDemo
{
    /// <summary>
    /// 字符串转整型
    /// </summary>
    public class StringToInt : IGetResult
    {
        public void ConsoleOut()
        {
            Console.WriteLine(GetNum("12313123"));

            Console.WriteLine(GetNum("123asda13123"));
        }

        private int GetNum(string str)
        {
            int num = 0;
            int gap = 0;
            for (int i = 0; i < str.Length; i++)
            {
                gap = str[i] - '0';
                if (gap < 0 || gap >= 10) return -1;
                num = num * 10 + gap;
            }
            return num;
        }
    }
}