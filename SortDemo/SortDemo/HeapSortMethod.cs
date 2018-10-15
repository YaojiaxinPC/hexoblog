using System;
using System.Collections.Generic;

namespace SortDemo
{
    public class HeapSortMethod : SortBaseClass
    {
        //从parentid往下面排到length位
        private int HeapAdjust(IList<int> sourcelist, int parentid, int length)
        {
            int countnum = 0;
            int temp = sourcelist[parentid];
            //左子节点
            int child = 2 * parentid + 1;

            while (child < length)
            {
                countnum++;
                //取 左右 子节点中的较大值
                if (child + 1 < length && sourcelist[child] < sourcelist[child + 1])
                    child++;
                //与父节点比较，大于父节点就得交换，否则继续下个循环
                if (temp >= sourcelist[child])
                    break;

                sourcelist[parentid] = sourcelist[child];
                ConsoleOutProcess(sourcelist, parentid, child);
                parentid = child;
                child = 2 * parentid + 1;
            }
            //检查是否进行过交换，并将缓存中的值赋回去
            if (sourcelist[parentid] != temp)
            {
                countnum++;
                sourcelist[parentid] = temp;
            }
            ConsoleOutProcess(sourcelist, -1, parentid);

            return countnum;
        }

        private IList<int> HeapSort(IList<int> sourcelist, int top, out int countnum)
        {
            IList<int> topNode = new List<int>();
            countnum = 0;
            //第一遍，取出最大值，排到第一个(从倒数第二层左子节点开始排)
            //i--循环到顶0，排出最大值
            for (int i = sourcelist.Count / 2 - 1; i >= 0; i--)
            {
                countnum += HeapAdjust(sourcelist, i, sourcelist.Count);
            }

            //主要排序过程(从最上面到最下面)
            for (int i = sourcelist.Count - 1; i > sourcelist.Count - top; i--)
            {
                //将第一个与最后一个交换位置（最大值装入末尾）
                int temp = sourcelist[0];
                sourcelist[0] = sourcelist[i];
                sourcelist[i] = temp;
                ConsoleOutProcess(sourcelist, 0, i);
                Console.WriteLine();
                countnum++;
                //i，最后一个，是当前最大值
                //topNode 大根堆
                topNode.Add(temp);

                //将去掉最后一个值的集合重新排
                countnum += HeapAdjust(sourcelist, 0, i);
            }
            //topNode 大根堆
            //sourcelist 小根堆

            return sourcelist;
        }

        protected override IList<int> Sort(IList<int> sourcelist, out int countnum)
        {
            IList<int> resultlist = sourcelist;

            return HeapSort(sourcelist, sourcelist.Count, out countnum);
        }
    }
}
