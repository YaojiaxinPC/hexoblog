using System.Collections.Generic;


namespace SortDemo
{
    interface ISortMethod
    {
        IList<int> GetResult(IList<int> sourcelist, out int countnum);
    }
}
