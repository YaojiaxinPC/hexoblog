using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgorithmDemo
{
    /// <summary>
    /// 判断101-200之间有多少个素数。
    /// </summary>
    public class PrimeNum : IGetResult
    {
        public void ConsoleOut()
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start(); //  开始监视代码运行时间
            Console.WriteLine("共有：" + SuperNormalGetNum(100, 30000000));  //  需要测试的代码
            stopwatch.Stop(); //  停止监视
            TimeSpan timespan = stopwatch.Elapsed; //  获取当前实例测量得出的总时间
            //double hours = timespan.TotalHours; // 总小时
            //double minutes = timespan.TotalMinutes;  // 总分钟
            //double seconds = timespan.TotalSeconds;  //  总秒数
            double milliseconds = timespan.TotalMilliseconds;  //  总毫秒数
            Console.WriteLine("SuperNormalGetNum:" + milliseconds);

            stopwatch = new System.Diagnostics.Stopwatch();//为区分开，直接new，实际只要重新启动就可以归零
            stopwatch.Start();
            Console.WriteLine("共有：" + ListGetNum(100, 30000000));
            stopwatch.Stop();
            timespan = stopwatch.Elapsed;
            milliseconds = timespan.TotalMilliseconds;
            Console.WriteLine("ListGetNum:" + milliseconds);

            stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            Console.WriteLine("共有：" + SuperFilterNum(100, 30000000));
            stopwatch.Stop();
            timespan = stopwatch.Elapsed;
            milliseconds = timespan.TotalMilliseconds;
            Console.WriteLine("SuperFilterNum:" + milliseconds);
        }

        /// <summary>
        /// 简单处理，一个一个数字去除
        /// </summary>
        /// <param name="beginnum"></param>
        /// <param name="endnum"></param>
        /// <returns></returns>
        private int SimpleGetNum(int beginnum, int endnum)
        {
            int i, j;
            int counts = 0;
            for (i = beginnum; i <= endnum; i++)
            {
                for (j = 2; j < i; j++)
                {
                    //判断是否能整除
                    if (i % j == 0)
                        break;
                }

                //判断前面的循环是否会提前break，提前break时，j < k ；有break说明能获得整除
                if (j >= i && i != 0 && i != 1)
                {
                    counts++;
                }
            }

            return counts;
        }

        /// <summary>
        /// 结合算法思想处理，最多人使用的方式
        /// </summary>
        /// <param name="beginnum"></param>
        /// <param name="endnum"></param>
        /// <returns></returns>
        private int NormalGetNum(int beginnum, int endnum)
        {
            int i, j, k;
            int counts = 0;

            for (i = beginnum; i <= endnum; i++)
            {
                k = (int)Math.Sqrt(i);                for (j = 2; j <= k; j++)                {
                    //判断是否能整除
                    if (i % j == 0)                        break;                }

                //判断前面的循环是否会提前break，提前break时，j < k ；有break说明能获得整除
                if (j >= k && i != 0 && i != 1)                {                    counts++;                }
            }

            return counts;
        }

        /// <summary>
        /// 令x≥1，将大于等于5的自然数表示如下：
        /// ··· 6x，6x+1，6x+2，6x+3，6x+4，6x+5，6(x+1），6(x+1)+1 ···
        /// 故只需要判断6x+1和6x+5两个数，再间隔6个数再次判断
        /// </summary>
        /// <param name="beginnum"></param>
        /// <param name="endnum"></param>
        /// <returns></returns>
        private int SuperNormalGetNum(int beginnum, int endnum)
        {
            int i, j, k;
            int counts = 0;
            bool isnofrime = false;
            for (i = beginnum; i <= endnum; i++)
            {
                if (i % 6 != 1 && i % 6 != 5)
                    continue;

                isnofrime = false;
                k = (int)Math.Sqrt(i);                for (j = 5; j <= k; j += 6)                {
                    //判断是否能整除
                    if (i % j == 0 || i % (j + 2) == 0)
                    {
                        isnofrime = true;
                        break;
                    }
                }

                if (!isnofrime)
                {
                    counts++;
                }
            }

            return counts;
        }

        /// <summary>
        /// 从2开始，获取素数，保存起来，给后面的数判断素数，一直判断到最大值。
        /// </summary>
        /// <param name="beginnum"></param>
        /// <param name="endnum"></param>
        /// <returns></returns>
        private int ListGetNum(int beginnum, int endnum)
        {
            int i, j, counts, k;
            List<int> primelts = new List<int>();
            //加入第一个素数2
            primelts.Add(2);

            for (i = 3; i <= endnum; i++)
            {
                k = (int)Math.Sqrt(i) + 1;
                for (j = 0; j < primelts.Count; j++)
                {
                    if (primelts[j] > k)
                    {
                        primelts.Add(i);
                        break;
                    }

                    //能整除，不是素数
                    if (i % primelts[j] == 0)
                    {
                        break;
                    }
                    else if (j == primelts.Count - 1)
                    {
                        primelts.Add(i);
                        break;
                    }
                }
            }

            counts = 0;
            for (i = 0; i < primelts.Count; i++)
            {
                if (primelts[i] >= beginnum)
                {
                    counts++;
                }
            }

            return counts;
        }

        /// <summary>
        /// 简单线性筛法
        /// 从头开始，取到一个素数后，将后面对应该素数的合数全部删掉。
        /// 这样每一轮，剩下最小的那个数肯定是素数。
        /// </summary>
        /// <param name="beginnum"></param>
        /// <param name="endnum"></param>
        /// <returns></returns>
        private int FilterNum(int beginnum, int endnum)
        {
            Dictionary<int, bool> allnums = new Dictionary<int, bool>();

            int mininum = 2;

            //按照顺序排放
            for (int i = mininum; i <= endnum; i++)
            {
                allnums.Add(i, false);
            }

            List<int> primelst = new List<int>();

            while (allnums.Count >= 1)
            {
                mininum = allnums.ElementAt(0).Key;
                //第一个数是最小的，肯定是素数
                allnums.Remove(mininum);
                primelst.Add(mininum);
                //int会溢出，需要设置为double才能防止溢出
                for (int i = mininum; (double)i * mininum <= endnum; i++)
                {
                    //将该素数对应的合数全部删除
                    allnums.Remove((int)(double)i * mininum);
                }
            }

            for (int i = 0; true; i++)
            {
                if (primelst[0] < beginnum) primelst.RemoveAt(0);
                else break;
            }

            return primelst.Count;
        }

        /// <summary>
        /// 改进FilterNum存在重复操作的缺点
        /// </summary>
        /// <param name="beginnum"></param>
        /// <param name="endnum"></param>
        /// <returns></returns>
        private int SuperFilterNum(int beginnum, int endnum)
        {
            Dictionary<int, bool> allnums = new Dictionary<int, bool>();
            List<int> primelst = new List<int>();

            //先将所有数设为素数 
            for (int i = 0; i <= endnum; i++)
            {
                allnums[i] = false;
            }

            for (int i = 2; i <= endnum; i++)
            {
                if (!allnums[i])
                {
                    //false为素数
                    primelst.Add(i);
                }


                // j =0  ==> primelst[0] ==> 2 * i <= endnum 过滤掉后面一半的数, 因为 合数 = A x B ，必定存在 A(或者B) <= 二分之一 ，
                // 所以实际排查前面二分之一的值时 , 后面二分之一的也已经把合数去掉了
                for (int j = 0; j < primelst.Count && primelst[j] * i <= endnum; j++)
                {
                    // 用已获得的素数 x index , 排查出对应唯一确定的合数 
                    allnums[primelst[j] * i] = true;

                    // 重点！通过查找最小素数，防止了重复操作
                    // 合数 = A x B，当A/B又是合数时，重复下去，合数 = ...x...x素数 ==> 最大素数 x 第二大素数 x ... x 最小素数 
                    // i % primelst[j] 就break，说明已经找到最小素数（j从0开始++）
                    // 此时break，合数 = i x 最小素数primelst[j] ，能唯一定位到该合数。不会存在重复定位该合数
                    // 举例：合数12 (有两种定位方式)== 4 x 3 ==> 2 x 2 x3
                    //                              == 6 x 2 => 3 x 2 x 2 
                    // 实际i=4的时候，定位的是8=4x2就break；不会去定位12=4x3
                    // i=6的时候，才定位12；
                    //
                    //同理，合数18 == 6x3 ==> 2x3x3
                    //             == 9x2 ==> 3x3x2
                    //i=6，定位12就break；等到i=9才来定位18
                    if (i % primelst[j] == 0)
                        break;
                }
            }


            while (primelst[0] < beginnum)
            {
                primelst.RemoveAt(0);
            }


            return primelst.Count;
        }
    }
}