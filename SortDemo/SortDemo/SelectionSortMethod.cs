using System.Collections.Generic;

namespace SortDemo
{
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
}
