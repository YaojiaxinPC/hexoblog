using System.Collections.Generic;

namespace SortDemo
{
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
}
