using System;
using System.Collections.Generic;

namespace SortDemo
{
    public class MergeSortMethod : SortBaseClass
    {
        protected override IList<int> Sort(IList<int> sourcelist, out int countnum)
        {
            IList<int> resultlist = sourcelist;
            countnum = 0;
            DivSort(resultlist, 0, resultlist.Count - 1, ref countnum);

            return resultlist;
        }

        //对折拆分左右两组
        private void DivSort(IList<int> sourcelist, int leftid, int rightid, ref int countnum)
        {
            if (leftid < rightid)
            {
                int midid = (leftid + rightid) / 2;
                //前1/2
                DivSort(sourcelist, leftid, midid, ref countnum);

                //后1/2
                DivSort(sourcelist, midid + 1, rightid, ref countnum);

                //比较排序(拆分到最后要进行的比较和合并)
                Merge(sourcelist, leftid, midid, rightid, ref countnum);
            }
            //左右坐标相等，或者左边大于右边(midid + 1)，相当于拆到最后一个，进入下一步Merge
        }

        //主要排序操作
        private void Merge(IList<int> sourcelist, int leftid, int midid, int rightid, ref int countnum)
        {
            IList<int> temp = new List<int>();

            int i = leftid;
            int j = midid + 1;
            while (i <= midid && j <= rightid)
            {
                countnum++;
                //取最小值存入缓存
                if (sourcelist[i] <= sourcelist[j])
                    temp.Add(sourcelist[i++]);
                else
                    temp.Add(sourcelist[j++]);
            }
            //两种情况只会存在一种：左(/右)边还有值
            //左边
            while (j <= rightid)
            {
                countnum++;
                temp.Add(sourcelist[j++]);
            }
            //右边
            while (i <= midid)
            {
                countnum++;
                temp.Add(sourcelist[i++]);
            }

            foreach (var item in temp)
            {
                //leftid~midid~rightid区间为已排序，其他部分未排序
                if (leftid > rightid) break;
                sourcelist[leftid++] = item;
            }

            ConsoleOutProcess(temp, -1, temp.Count);
            Console.WriteLine();
        }
    }
}
