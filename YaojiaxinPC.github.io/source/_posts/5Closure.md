---
title: 闭包Closure
categories:
  - 编程疑难杂症
tags:
  - C#
  - 闭包  
---
	
　　本文记录在编程中遇到的一个疑难杂症，for循环中的异步赋值结果和理想情况对不上：全部赋值成同一个值。

<!-- more -->
　　先贴代码：

``` java
    for (int i = 0; i < 10; i++)
    {
        System.Threading.ThreadPool.QueueUserWorkItem((o) =>
        {
            Console.WriteLine(i);
        });
    }
```

　　上面这段代码，照理是进行异步处理，开了线程池来打印。理想输出是0~10这样。

　　但结果：

![Result pic 1](/contentimg/5/1.png "实际输出1")

![Result pic 2](/contentimg/5/2.png "理想输出1")

　　这里就是闭包现象。

[闭包：](https://www.cnblogs.com/jiejie_peng/p/3701070.html)

>　　***内层的函数可以引用包含在它外层的函数的变量，即使外层函数的执行已经终止。但该变量提供的值并非变量创建时的值，而是在父函数范围内的最终值。***

　　这里有篇博文也介绍这个，上面的[概念](https://www.cnblogs.com/jiejie_peng/p/3701070.html) 是复制该博文过来的。

　　这里简单理解，就是变量i，在lamdb表达式的时候，传的是地址，而实际上，这个地址上的值已经执行完for变成10了（理想情况下，传的应该是值0~9）。

　　照这样理解，可以搞一个中间缓冲值（即每次传的都不是同一个地址）来处理：（输出没按照顺序，是因为多核处理问题，这里不进行讨论）

![Result pic 3](/contentimg/5/3.png "使用中间缓冲值")

　　明显已经没有重复值了，但是记住，地址！就是说tmp的声明必须在for内，如果放到外面（或者放在线程池里面）：

![Result pic 4](/contentimg/5/4.png "错误使用中间缓冲值")

　　神秘现象，直接是9，不是10了？？？好吧，不小心给自己挖了个坑，接下来开始填坑吧。分析为什么是9，不是10：

![Result pic 5](/contentimg/5/5.png "分析：错误使用中间缓冲值")

　　循环中确实是0~9，但是最后i++变成10，进行i&lt;10的判断后false，不进入循环了，所以i最后结果为10，tmp=9.  坑解决。

　　但这里又有新坑，按前面的理解，不是全部应该“in :9___10”，怎么还出现"in :6___6"的？

![Result pic 6](/contentimg/5/6.png "console占用较多的进程时间")

　　其实是因为console占用了时间，导致线程池的在for还未执行完的时候就开始了（线程池创建后还得等待分配资源才能启动，所以有延迟），所以就可以看到，前面数字很乱（for还在执行，所以数字还在变），到后面才变成全部9（for执行完毕）。

　　这里有多线程的知识，这里暂时不讨论，简单讲，如string类型的，因为是每个线程会复制一份到自己环境中操作，修改完毕后才通知回主地址，如果这时出现A、B线程同时拷贝了数据去操作，又同时通知回来，就导致脏数据产生（互相覆盖），所以就得用多线程锁（实质是内存片段锁的处理，锁住主地址的访问，不允许拷贝处理，只能排队，变成类似单线程操作）来处理。

下面贴一下内存地址部分测试的demo，证明用的是同一个地址：

``` java
    public class test
    {
        //地址不变
        public int num;
        //string的内容变时，实际是重新占用内存，然后存入新值，所以只要内容发生变化，内存肯定变
        public string str;
 
        public void MethodA()
        {
            for (int i = 0; i < 10; i++)
            {
                 System.Threading.ThreadPool.QueueUserWorkItem((o) =>
                {
                    num = i;
                    str = i.ToString();
                    GetAddr();
                });
            }
        }
 
        public void MethodB()
        {
            for (int i = 0; i < 10; i++)
            {
                //System.Threading.ThreadPool.QueueUserWorkItem((o) =>
                //{
                    num = i;
                    str = i.ToString();
                    GetAddr();
                //});
            }
        }
 
 
        unsafe void GetAddr()
        {
            //这个是类对象，放在堆里面
            fixed (int* p = &num)
            {
                Console.WriteLine("{0}__:Address of numbe:0x{1:x}", num, (int)p);
            }
 
            //fixed(char* p= str)
            //{
            //    Console.WriteLine("{0}__:Address of char:0x{1:x}", str, (int)p);
            //}
 
            //GetAddrZ(num);
        }
 
        ////弄成函数传值的话，地址是变化的，因为是一份拷贝
        //unsafe void GetAddrZ(int n)
        //{
        //    //获取栈上变量的地址
        //    int* p = &n;
        //    Console.WriteLine("{0}__:Address of n:0x{1:x}\n", num, (int)p);
        //}
    }
```

　　先使用int：

![Result pic 7](/contentimg/5/7.png "整数型地址情况")

　　接下来是string：

![Result pic 8](/contentimg/5/8.png "字符串地址情况")

　　这里的main函数是：

``` java
    static void Main(string[] args)
    {
        test t = new test();
        t.MethodA();
        Console.WriteLine();
        Console.WriteLine();
        t.MethodB();
 
        Console.ReadLine();
    }
```

　　当然这里也会出现这种情况：

![Result pic 9](/contentimg/5/9.png "代码执行顺序有变化")

　　发现这里上面本应该一致的地址，也会出现不同。

　　重复多次执行经常出现这种情况。

![Result pic 10](/contentimg/5/10.png "地址发生变化")

　　这里就是引用类型在多线程情况下的问题。

　　这里简单提及“原子操作volatile”：

![Result pic 11](/contentimg/5/11.png "原子操作volatile")

　　原子操作 volatile。通知编译器，不允许拷贝，全部访问都是去主地址拿。算是最简单的多线程处理操作，实质这里没使用到加锁，还是会出现问题的：

![Result pic 12](/contentimg/5/12.png "原子操作volatile出问题")

这里放一下简单的加锁操作：

``` java
    private object lockstr = new object();
    private volatile string _str = "";
    public string Str
    {
        get { return _str; }
        set
        {
            lock (lockstr)
            {
                _str = value;
            }
        }
    }
```

![Result pic 13](/contentimg/5/13.png "加锁操作情况")

　　多线程内容超级多，等我整理好后再发专门的主题。这里只提初级的加锁方式lock(obj)。


 本文测试程序工程可以从git直接获取：
 
 git代码库: [Codes](https://github.com/YaojiaxinPC/hexoblog/tree/master/TestClosure)
 