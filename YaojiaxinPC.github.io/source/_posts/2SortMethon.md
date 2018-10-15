---
title: 入门级的8种算法记录
categories:
  - 算法
tags:
  - C#
  - 数据结构
  - 算法
---
	
　　本文记录算法中入门级别的8种简单排序算法。
 1.  冒泡排序　　2.  选择排序　　3.  插入排序　　4.  堆排序
　　5.  归并排序　　6.  快速排序　　7.  希尔排序　　8.  基数排序
<!-- more -->
## 基础代码实现：

### 1.抽象公共接口部分

``` java
    interface ISortMethod
    {
        IList<int> GetResult(IList<int> sourcelist,out int countnum);
    }
```

#### 2.建立公共基类进行数据有效性校验和控制台输出的颜色标记

``` java
    public class SortBaseClass : ISortMethod
    {
        public IList<int> GetResult(IList<int> sourcelist, out int countnum)
        {
            return CheckEffect(sourcelist,out countnum);
        }

        /// <summary>
        /// 进行数据的有效性判断
        /// </summary>
        /// <param name="sourcelist"></param>
        /// <returns></returns>
        private IList<int> CheckEffect(IList<int> sourcelist, out int countnum)
        {
            IList<int> resultlist = null;
            countnum = 0;
            if (sourcelist != null)
            {
                if (sourcelist.Count > 0)
                {
                    resultlist = Sort(sourcelist,out countnum);
                }
                else
                {
                    resultlist = sourcelist;
                }
            }

            return resultlist;
        }

        /// <summary>
        /// 由具体的算法部分进行重写
        /// </summary>
        /// <param name="sourcelist"></param>
        /// <returns></returns>
        protected virtual IList<int> Sort(IList<int> sourcelist, out int countnum)
        {
            countnum = 0;
            return sourcelist;
        }

        /// <summary>
        /// 控制台过程输出
        /// </summary>
        /// <param name="sourcelist"></param>
        /// <param name="n">交换的前面一个数的序号</param>
        /// <param name="m">交换的后面一个数的序号</param>
        protected void ConsoleOutProcess(IList<int> sourcelist,int n,int m)
        {
            for (int i = 0; i < sourcelist.Count; i++)
            {
                if (i < sourcelist.Count - 1)
                {
                    if (i == n || i == m)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else
                        Console.ForegroundColor = ConsoleColor.White;

                    Console.Write(sourcelist[i]);

                    if (i != n)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("-->");
                    }
                    else
                    {
                        Console.Write("<--");
                    }    
                }
            }

            Console.ForegroundColor = ConsoleColor.White;
            if (sourcelist.Count - 1 == n || sourcelist.Count - 1 == m)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            Console.Write(sourcelist[sourcelist.Count - 1]);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }
    }
```

#### 3.main函数具体打印代码

``` java
    static void Main(string[] args)
    {
        List<int> numlst = new List<int>() { 23, 44, 66, 66, 76, 98, 11, 3, 9, 7 };
        ISortMethod sort = new BubbleSortMethod();

        Console.WriteLine("原数列为：");
        StringBuilder str = new StringBuilder();
        for (int i = 0; i < numlst.Count; i++)
        {
            str.Append(numlst[i] + "   ");
        }
        Console.WriteLine(str.Remove(str.Length - 3, 3).ToString());

        Console.WriteLine(System.Environment.NewLine);
        int countnum = 0;
        List<int> result = sort.GetResult(numlst,out countnum) as List<int>;

        Console.WriteLine(System.Environment.NewLine);
        Console.WriteLine("经过了{0}次数据对比 ", countnum);
        Console.WriteLine("排序后为：");
        str.Clear();
        for (int i = 0; i < result.Count; i++)
        {
            str.Append(result[i] + "-->");
        }
        Console.WriteLine(str.Remove(str.Length - 3, 3).ToString());
            
        Console.ReadKey();
    }
```

#### 4.具体算法实现部分

具体实现算法，同时添加过程打印以便分析。

## 一. 冒泡排序

### 1.1原理

 [冒泡排序](https://baike.baidu.com/item/%E5%86%92%E6%B3%A1%E6%8E%92%E5%BA%8F/4602306?fr=aladdin) （Bubble Sort）算法的原理如下:

1.  比较相邻的元素。如果第一个比第二个大，就交换他们两个。
1.  对每一对相邻元素做同样的工作，从开始第一对到结尾的最后一对。在这一点，最后的元素应该会是最大的数。
1.  针对所有的元素重复以上的步骤，除了最后一个。
1.  持续每次对越来越少的元素重复上面的步骤，直到没有任何一对数字需要比较。

### 1.2具体代码实现

``` java
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
```

　　同时修改main函数的算法为具体的： ISortMethod sort = new BubbleSortMethod();

　　执行结果为：

![Result pic 1](/contentimg/2/1.png "冒泡排序")

## 二.选择排序

### 2.1原理

 [选择排序](https://baike.baidu.com/item/%E9%80%89%E6%8B%A9%E6%8E%92%E5%BA%8F/9762418?fr=aladdin) （Selection sort）算法的原理如下：

　每一次从待排序的[数据元素](https://baike.baidu.com/item/%E6%95%B0%E6%8D%AE%E5%85%83%E7%B4%A0) 中选出最小（或最大）的一个元素，存放在序列的起始位置，直到全部待排序的数据元素排完。
 
### 2.2具体代码实现

``` java
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
```

　　同时修改main函数的算法为具体的： ISortMethod sort = new SelectionSortMethod();

　　执行结果为：

![Result pic 2](/contentimg/2/2.png "选择排序")

## 三. 插入排序

### 3.1原理

 [插入排序](https://www.douban.com/note/396407547/) （Insert Sort）算法的原理如下:

　插入即表示将一个新的数据插入到一个有序数组中，并继续保持有序。例如有一个长度为N的无序数组，进行N-1次的插入即能完成排序。
　第一次，数组第1个数认为是有序的数组，将数组第二个元素插入仅有1个有序的数组中；
　第二次，数组前两个元素组成有序的数组，将数组第三个元素插入由两个元素构成的有序数组中.....
　第N-1次，数组前N-1个元素组成有序的数组，将数组的第N个元素插入由N-1个元素构成的有序数组中，则完成了整个插入排序。
　....
　按序比较直到集合排序完毕。

### 3.2具体代码实现

``` java
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
```

　　同时修改main函数的算法为具体的： ISortMethod sort = new NormalInsertSortMethod();

　　执行结果为：

![Result pic 3](/contentimg/2/3.png "插入排序")

## 四. 堆排序

### 4.1原理

 [堆排序](https://www.cnblogs.com/0zcl/p/6737944.html) （Heap Sort）算法的原理如下:

　利用二叉树的特性，将剩余数组中的最大值（或最小值）排到开头处。然后去掉该值（提取到新数组开头数值）再次排序得到最值排去新数组那边开头，依次重复操作就能得到结果。
　这里用到的 [二叉树的特性](https://www.jianshu.com/p/106fdd9976a8) 是节点i的左子节点为2i，右子节点为2i+1.
　结合集合的话-1。集合(n = i-1)（i&gt;0,集合第一个序号为0，故需要-1）,序号(n)的左子节点为序号(2n+1)，右子节点为序号(2n+2)。
　int child = 2 * parentid + 1;就是这里的特性使用。
　引用 [链接](https://www.cnblogs.com/0zcl/p/6737944.html) 中这个图比较直观。
 
 ![Result pic 4](/contentimg/2/4.1.png "堆排序图解")
 
 在下面的代码中是这样的：
 
 ![Result pic 5](/contentimg/2/4.2.png "堆排序实例")

### 4.2具体代码实现

``` java
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
```

　　同时修改main函数的算法为具体的： ISortMethod sort = new HeapSortMethod();

　　执行结果为：

![Result pic 6](/contentimg/2/4.3.png "堆排序")

## 五. 归并排序

### 5.1原理

 [归并排序](https://baike.baidu.com/item/%E5%BD%92%E5%B9%B6%E6%8E%92%E5%BA%8F/1639015?fr=aladdin) （Merge Sort）算法的原理如下:

　假设序列共有n个元素，将序列每相邻两个数字进行归并操作（merge)，形成floor(n/2)个序列，排序后每个序列包含两个元素。将上述序列再次归并，形成floor(n/4)个序列，每个序列包含四个元素...重复归并，直到所有元素排序完毕。
　引用 [链接](https://www.cnblogs.com/chengxiao/p/6194356.html) 中这个图比较直观。
 
 ![Result pic 7](/contentimg/2/5.1.png "归并排序图解")
 

### 5.2具体代码实现

``` java
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
```

　　同时修改main函数的算法为具体的： ISortMethod sort = new MergeSortMethod();

　　执行结果为：

![Result pic 8](/contentimg/2/5.2.png "归并排序")

## 六. 快速排序

### 6.1原理

 [快速排序](https://baike.baidu.com/item/%E5%BF%AB%E9%80%9F%E6%8E%92%E5%BA%8F%E7%AE%97%E6%B3%95/369842?fromtitle=%E5%BF%AB%E9%80%9F%E6%8E%92%E5%BA%8F&amp;fromid=2084344&amp;fr=aladdin) （Quick Sort）算法的原理如下:

　假设要排序的数组是A[0]……A[N-1]，首先任意选取一个数据（通常选用数组的第一个数）作为关键数据，然后将所有比它小的数都放到它前面，所有比它大的数都放到它后面：

1.  设置两个变量i、j，排序开始的时候：i=0，j=N-1；
1.  以第一个数组元素作为关键数据，赋值给<strong>key</strong>，即<strong>key</strong>=A[0]；
1.  从j开始向前搜索，即由后开始向前搜索(j--)，找到第一个小于<strong>key</strong>的值A[j]，将A[j]和A[i]互换；
1.  从i开始向后搜索，即由前开始向后搜索(i++)，找到第一个大于<strong>key</strong>的A[i]，将A[i]和A[j]互换；
1.  重复第3、4步，直到i=j； (3,4步中，没找到符合条件的值，即3中A[j]不小于<strong>key</strong>,4中A[i]不大于<strong>key</strong>的时候改变j、i的值，使得j=j-1，i=i+1，直至找到为止。找到符合条件的值，进行交换的时候i， j指针位置不变。另外，i==j这一过程一定正好是i+或j-完成的时候，此时令循环结束）。

　引用 [链接](http://blog.51cto.com/ahalei/1365285) 中这个图比较直观。
 
 ![Result pic 9](/contentimg/2/6.1.png "快速排序图解")
 

### 6.2具体代码实现

``` java
    public class QuickSortMethod : SortBaseClass
    {
        protected override IList<int> Sort(IList<int> sourcelist, out int countnum)
        {
            IList<int> resultlist = sourcelist;
            countnum = 0;
            QuickSort(resultlist, 0, resultlist.Count - 1,ref countnum);
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
                ConsoleOutProcess(sourcelist, rightid,leftid);
            }

            sourcelist[leftid] = key;
            ConsoleOutProcess(sourcelist, tempid, leftid);
            return rightid;
        }

        private void QuickSort(IList<int> sourcelist, int leftid, int rightid, ref int countnum)
        {
            if (leftid >= rightid) return;
            int indexid = SortUnit(sourcelist, leftid, rightid, ref countnum);
            QuickSort(sourcelist, leftid, indexid - 1,ref countnum);
            QuickSort(sourcelist, indexid + 1, rightid, ref countnum);
        }
    }
```

　　同时修改main函数的算法为具体的： ISortMethod sort = new QuickSortMethod();

　　执行结果为：

![Result pic 10](/contentimg/2/6.2.png "快速排序")

## 七. 希尔排序

### 7.1原理

 [希尔排序](https://baike.baidu.com/item/%E5%B8%8C%E5%B0%94%E6%8E%92%E5%BA%8F/3229428?fr=aladdin) （Shell's Sort）算法是直接插入排序算法的一种更高效的改进版本：

　假设要排序的数组是A[0]……A[N-1]，首先任意选取一个数据（通常选用数组的第一个数）作为关键数据，然后将所有比它小的数都放到它前面，所有比它大的数都放到它后面：

　先取一个小于n的整数增量（一般取n/2）d1，把数组中下标间隔d1的作为一组进行组内排序；然后取第二个增量d2（d1/2）重复操作，直到增量 = 1。

　引用 [链接](https://www.cnblogs.com/chengxiao/p/6104371.html) 中这个图比较直观。
 
 ![Result pic 11](/contentimg/2/7.1.png "希尔排序图解")
 

### 7.2具体代码实现

``` java
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
```

　　同时修改main函数的算法为具体的： ISortMethod sort = new ShellsSortMethod();

　　执行结果为：

![Result pic 12](/contentimg/2/7.2.png "希尔排序")

## 八. 基数排序

### 8.1原理

 [基数排序](https://www.cnblogs.com/dwj411024/p/5978821.html) （Radix Sort）算法的原理如下：

　基数排序是一种借助多关键字排序的思想对单逻辑关键字进行排序的方法。它是一种稳定的排序算法。多关键字排序中有两种方法：最高位优先法(MSD)和最低位优先法（LSD）。通常用于对数的排序选择的是最低位优先法，即先对最次位关键字进行排序，再对高一位的关键字进行排序，以此类推。

　引用 [链接](https://www.cnblogs.com/dwj411024/p/5978821.html) 中这个图比较直观。
 
 ![Result pic 11](/contentimg/2/8.1.png "基数排序图解")
 
### 8.2具体代码实现

``` java
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
                for(int j =0; j< sourcelist.Count; j++)
                {
                    int number = (sourcelist[j] % Convert.ToInt32(Math.Pow(10, i + 1))) / Convert.ToInt32(Math.Pow(10, i));  //得出i+1位上的数
                    indexCounter[number]++;
                }
                int[] indexBegin = new int[10];
                //统计数量分布,例如3，前面有多少个数，就+1放在第几位
                for(int k = 1; k < 10; k++)
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
```

　　同时修改main函数的算法为具体的： ISortMethod sort = new RadixSortMethod();

　　执行结果为：

![Result pic 12](/contentimg/2/8.2.png "基数排序")

　　该算法以空间换时间，不用进行数学上的比较就能进行排序。与前面几种算法有一定的区别。

