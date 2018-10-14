using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDemo
{
    interface ISortMethod
    {
        IList<int> GetResult(IList<int> sourcelist,out int countnum);
    }
}
