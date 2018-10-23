using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgorithmDemo
{
    /// <summary>
    /// 题目：有数字：1、2、3、4，请问能组成多少个互不相同且无重复数字的三位数？请输出这些数字。
    /// </summary>
    public class NumToInt : IGetResult
    {
        public void ConsoleOut()
        {
            List<int> inputlst = new List<int>() { 1, 2, 3, 4 };

            int[] inputnums = inputlst.ToArray();
            List<int> result = NumToIntMethod(inputnums);
            StringBuilder txt = new StringBuilder();
            if (result.Count > 0)
            {
                txt.Append("结果共 " + result.Count + " 个；分别是： ");
                for (int i = 0; i < result.Count; i++)
                {
                    if (i < result.Count - 1) txt.Append(result[i] + "、 ");
                    else
                        txt.Append(result[i]);
                }
            }
            else
            {
                txt.Append("存在重复数字，请重新输入！");
            }

            Console.WriteLine(txt.ToString());
        }

        /// <summary>
        /// 输入有多少数字，组合排序后输出结果
        /// </summary>
        /// <param name="inputnum"></param>
        /// <returns></returns>
        private List<int> NumToIntMethod(int[] inputnum)
        {
            List<int> results = new List<int>();

            //去除重复值
            int[] hassamenum = inputnum.GroupBy(i => i).Select(i => i.Key).ToArray();

            //不存在重复值
            if (hassamenum.Count() == inputnum.Count())
            {
                GetNum(0, inputnum.ToList(),ref results);
            }

            return results;
        }

        /// <summary>
        ///  递归调用
        /// </summary>
        /// <param name="beforenum">前面组合的数字</param>
        /// <param name="leftlst">剔除掉已选数字后的集合</param>
        /// <param name="alllst">全部结果总集合</param>
        private void GetNum(int beforenum, List<int> leftlst, ref List<int> alllst)
        {
            //只剩最后一个数字，表示可以输出结果
            if (leftlst.Count == 1)
            {
                if (!alllst.Contains(beforenum))//检查是否重复，99.9%概率不会重复
                {
                    alllst.Add(beforenum);
                }
                else
                {
                    Console.WriteLine("重复！");
                }
                return;
            }

            for (int i = 0; i < leftlst.Count; i++)
            {
                //将前面的数字组合
                int tmpnum = beforenum * 10 + leftlst[i];
                List<int> tmplst = new List<int>();
                tmplst.AddRange(leftlst);
                //剔除已组合的数字
                tmplst.RemoveAt(i);

                GetNum(tmpnum, tmplst, ref alllst);
            }
        }

    }
}
