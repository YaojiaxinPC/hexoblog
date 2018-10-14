using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDemo
{
    /*
    List<int> numlst = new List<int>() { 23, 44, 66, 66, 76, 98, 11, 3, 9, 7 };
    ISortMethod sort = new RadixSortMethod();

    Console.WriteLine("原数列为：");
            StringBuilder str = new StringBuilder();
            for (int i = 0; i<numlst.Count; i++)
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
            for (int i = 0; i<result.Count; i++)
            {
                str.Append(result[i] + "-->");
            }
            Console.WriteLine(str.Remove(str.Length - 3, 3).ToString());
            */

    public class BubbleSortMethod : SortBaseClass
    {
        protected override IList<int> Sort(IList<int> sourcelist, out int countnum)
        {
            IList<int> resultlist = sourcelist;
            countnum = 0;
            int temp = int.MinValue;
            for (int i = 0; i < sourcelist.Count - 1; i++)
            {
                for (int j = 0; j < sourcelist.Count - 1 - i; j++)
                {
                    if (resultlist[j] > resultlist[j + 1])
                    {
                        temp = resultlist[j + 1];
                        resultlist[j + 1] = resultlist[j];
                        resultlist[j] = temp;

                        ConsoleOutProcess(resultlist, j, j + 1);
                    }
                    countnum++;
                }
            }

            return resultlist;
        }
    }

    public class SelectionSortMethod : SortBaseClass
    {
        protected override IList<int> Sort(IList<int> sourcelist, out int countnum)
        {
            IList<int> resultlist = sourcelist;

            int temp, pos = 0;
            countnum = 0;
            for (int i = 0; i < sourcelist.Count - 1; i++)
            {
                pos = i;
                for (int j = i + 1; j < sourcelist.Count; j++)
                {
                    if (resultlist[j] < resultlist[pos])
                    {
                        pos = j;
                    }
                    countnum++;
                }
                if (pos == i) continue;
                temp = resultlist[i];
                resultlist[i] = resultlist[pos];
                resultlist[pos] = temp;
                ConsoleOutProcess(resultlist, i, pos);
            }

            return resultlist;
        }
    }

    public class NormalInsertSortMethod : SortBaseClass
    {
        protected override IList<int> Sort(IList<int> sourcelist, out int countnum)
        {
            IList<int> resultlist = sourcelist;
            countnum = 0;
            int key = int.MinValue;
            int i, j = 0;
            for (j = 1; j < sourcelist.Count; j++)
            {
                key = resultlist[j];
                i = j - 1;
                while (i >= 0 && resultlist[i] > key)
                {
                    countnum++;
                    resultlist[i + 1] = resultlist[i];
                    i -= 1;
                }
                if (resultlist[i + 1] == key) continue;

                resultlist[i + 1] = key;
                ConsoleOutProcess(resultlist, -1, i + 1);
            }

            return resultlist;
        }
    }

    public class HeapSortMethod : SortBaseClass
    {
        private int HeapAdjust(IList<int> sourcelist, int parentid, int length)
        {
            int countnum = 0;
            int temp = sourcelist[parentid];
            int child = 2 * parentid + 1;

            while (child < length)
            {
                countnum++;
                if (child + 1 < length && sourcelist[child] < sourcelist[child + 1])
                    child++;
                if (temp >= sourcelist[child])
                    break;

                sourcelist[parentid] = sourcelist[child];
                ConsoleOutProcess(sourcelist, parentid, child);
                parentid = child;
                child = 2 * parentid + 1;
            }

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
            for (int i = sourcelist.Count / 2 - 1; i >= 0; i--)
            {
                countnum += HeapAdjust(sourcelist, i, sourcelist.Count);
            }
            for (int i = sourcelist.Count - 1; i > sourcelist.Count - top; i--)
            {

                int temp = sourcelist[0];
                sourcelist[0] = sourcelist[i];
                sourcelist[i] = temp;
                ConsoleOutProcess(sourcelist, 0, i);
                Console.WriteLine();
                countnum++;
                topNode.Add(temp);
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

    public class MergeSortMethod : SortBaseClass
    {
        protected override IList<int> Sort(IList<int> sourcelist, out int countnum)
        {
            IList<int> resultlist = sourcelist;
            countnum = 0;
            DivSort(resultlist, 0, resultlist.Count - 1, ref countnum);

            return resultlist;
        }

        private void DivSort(IList<int> sourcelist, int leftid, int rightid, ref int countnum)
        {
            if (leftid < rightid)
            {
                int midid = (leftid + rightid) / 2;

                DivSort(sourcelist, leftid, midid, ref countnum);
                DivSort(sourcelist, midid + 1, rightid, ref countnum);
                Merge(sourcelist, leftid, midid, rightid, ref countnum);
            }
        }

        private void Merge(IList<int> sourcelist, int leftid, int midid, int rightid, ref int countnum)
        {
            IList<int> temp = new List<int>();

            int i = leftid;
            int j = midid + 1;
            while (i <= midid && j <= rightid)
            {
                countnum++;
                if (sourcelist[i] <= sourcelist[j])
                    temp.Add(sourcelist[i++]);
                else
                    temp.Add(sourcelist[j++]);
            }
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

    public class QuickSortMethod : SortBaseClass
    {
        protected override IList<int> Sort(IList<int> sourcelist, out int countnum)
        {
            IList<int> resultlist = sourcelist;
            countnum = 0;
            QuickSort(resultlist, 0, resultlist.Count - 1, ref countnum);
            return resultlist;
        }

        private int SortUnit(IList<int> sourcelist, int leftid, int rightid, ref int countnum)
        {
            int key = sourcelist[leftid];
            int tempid = leftid;
            while (leftid < rightid)
            {
                while (sourcelist[rightid] >= key && rightid > leftid)
                {
                    countnum++;
                    --rightid;
                }

                sourcelist[leftid] = sourcelist[rightid];
                ConsoleOutProcess(sourcelist, leftid, rightid);

                while (sourcelist[leftid] <= key && rightid > leftid)
                {
                    countnum++;
                    ++leftid;
                }

                sourcelist[rightid] = sourcelist[leftid];
                ConsoleOutProcess(sourcelist, rightid, leftid);
            }

            sourcelist[leftid] = key;
            ConsoleOutProcess(sourcelist, tempid, leftid);
            return rightid;
        }

        private void QuickSort(IList<int> sourcelist, int leftid, int rightid, ref int countnum)
        {
            if (leftid >= rightid) return;
            int indexid = SortUnit(sourcelist, leftid, rightid, ref countnum);
            QuickSort(sourcelist, leftid, indexid - 1, ref countnum);
            QuickSort(sourcelist, indexid + 1, rightid, ref countnum);
        }
    }

    public class ShellsSortMethod : SortBaseClass
    {
        protected override IList<int> Sort(IList<int> sourcelist, out int countnum)
        {
            IList<int> resultlist = sourcelist;
            countnum = 0;

            int i, j, flag, tmp, gap = sourcelist.Count;
            while (gap > 1)
            {
                gap = gap / 2;
                do
                {
                    flag = 0;
                    for (i = 0; i < sourcelist.Count - gap; i++)
                    {
                        countnum++;
                        j = i + gap;
                        if (sourcelist[i] > sourcelist[j])
                        {
                            tmp = sourcelist[i];
                            sourcelist[i] = sourcelist[j];
                            sourcelist[j] = tmp;
                            flag = 1;
                            ConsoleOutProcess(resultlist, j, i);
                        }
                    }
                }
                //最后间隔为1时需要多次微调排序
                while (gap == 1 && flag != 0);
            }

            return resultlist;
        }
    }

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
