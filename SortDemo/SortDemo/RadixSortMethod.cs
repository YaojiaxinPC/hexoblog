using System;
using System.Collections.Generic;

namespace SortDemo
{
    public class RadixSortMethod : SortBaseClass
    {
        protected override IList<int> Sort(IList<int> sourcelist, out int countnum)
        {
            countnum = 0;
            int max = sourcelist[0];
            //找到最大的数字
            foreach (var item in sourcelist)
            {
                if (item > max) max = item;
            }
            //分析出有多少位数
            int digit = 1;
            while (max / 10 != 0)
            {
                digit++;
                max /= 10;
            }

            for (int i = 0; i < digit; i++)
            {
                int[] indexCounter = new int[10];
                IList<int> tempList = new List<int>();
                foreach (var item in sourcelist)
                {
                    tempList.Add(0);
                }
                //排桶
                for (int j = 0; j < sourcelist.Count; j++)
                {
                    int number = (sourcelist[j] % Convert.ToInt32(Math.Pow(10, i + 1))) / Convert.ToInt32(Math.Pow(10, i));  //得出i+1位上的数
                    indexCounter[number]++;
                }
                int[] indexBegin = new int[10];
                //统计数量分布,例如3，前面有多少个数，就+1放在第几位
                for (int k = 1; k < 10; k++)
                {
                    indexBegin[k] = indexBegin[k - 1] + indexCounter[k - 1];
                }

                for (int k = 0; k < sourcelist.Count; k++)
                {
                    int number = (sourcelist[k] % Convert.ToInt32(Math.Pow(10, i + 1))) / Convert.ToInt32(Math.Pow(10, i));
                    //indexBegin[number]++ 指出该数字该排的序号
                    tempList[indexBegin[number]++] = sourcelist[k];
                }
                sourcelist = tempList;
            }

            return sourcelist;
        }


    }
}
