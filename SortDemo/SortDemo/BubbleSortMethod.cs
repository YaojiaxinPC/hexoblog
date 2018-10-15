using System.Collections.Generic;


namespace SortDemo
{
    public class BubbleSortMethod : SortBaseClass
    {
        protected override IList<int> Sort(IList<int> sourcelist, out int countnum)
        {
            IList<int> resultlist = sourcelist;
            countnum = 0;
            int temp = int.MinValue;
            for (int i = 0; i < sourcelist.Count - 1; i++)
            {
                //最后一个数最大，所以后面的数不必再次比较 -i
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
}
