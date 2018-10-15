using System.Collections.Generic;


namespace SortDemo
{
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

                //将数组往后面退1位
                while (i >= 0 && resultlist[i] > key)
                {
                    countnum++;
                    resultlist[i + 1] = resultlist[i];
                    i -= 1;
                }
                //过滤两个相同大小的数的左右交换
                if (resultlist[i + 1] == key) continue;

                //插入有序的位置（后面的都比这个数大）
                resultlist[i + 1] = key;
                ConsoleOutProcess(resultlist, -1, i + 1);
            }

            return resultlist;
        }
    }
}
