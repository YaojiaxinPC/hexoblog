using System.Collections.Generic;

namespace SortDemo
{
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
}
