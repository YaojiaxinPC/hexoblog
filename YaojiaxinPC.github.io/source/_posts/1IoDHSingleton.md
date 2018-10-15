---
title: 单例模式_静态构造函数实现和IoDH实现
categories:
  - 设计模式
tags:
  - C#
  - IoDH
---
	
　　单例模式，简单字面意思就是整个程序中只进行一次初始化的操作。相对于静态类，单例模式能做到延迟加载，以及类继承。本文记录“静态构造函数下的单例模式”，以及介绍“Initialization Demand Holder(IoDH)”模式实现。该示例使用VS2017以及C#进行编程。
<!-- more -->
## 一.前言

　　单例模式，是编程中使用频率相当高的一种最简单的设计模式。可以简单理解为静态类的设计模式化。为什么会区别于静态类？

　　静态类，一般用于“工具类”。在数据库连接等等场景下需要多线程的就不能使用静态类，其他应用于单线程下的操作，如文件读取，一般使用静态类。在软件优化中，第一个问题是初始化：静态类，会在程序启动的时候就会初始化，然后全局生存到进程结束。真有这个必要吗？我们打开程序时，一般先是登陆窗口，如果这个时候，一堆静态工具类也要加载初始化，那不就要等好久，还特占内存。

　　所以在这个情况下产生了能做到延迟加载的单例模式，当然，它还能进行类继承等等其他解耦操作（不过该部分涉及到其他涉及模式，本文不深入讨论）。

## 二.代码记录

### 2.1 静态构造函数实现单例模式

#### 2.1.1 静态类何时初始化

　　这里我们直接上代码，根据结果进行记录：

　　字段x初始化会在构造函数前面先执行。（重点。后面单例模式里面执行代码和想象中不同，考虑是这个原因。）

``` java
#if step1
    public class SingletonTest
    {
        public static string x = EchoAndReturn("A_In type initializer");

#if step1needstatic
        /// <summary>
        /// 当没有写静态构造函数时，框架会自动生成
        /// 导致静态字段的初始化跑到了静态方法调用之前
        /// 造成对象的提前初始化
        /// </summary>
        static SingletonTest()
        {
            x = "A_static init";
            Console.WriteLine(x);
        }
#endif

        public SingletonTest()
        {
            x = "A_nonstatic init";
            Console.WriteLine(x);
        }

        public static string EchoAndReturn(string s)
        {
            Console.WriteLine(s);
            return s;
        }
    }
#endif
```

　　对应main函数如下：

``` java
#if step1
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Main");
            //使用静态类
            SingletonTest.EchoAndReturn("Echo!");
            Console.WriteLine("After echo");
            //取出静态类中的静态成员x
            string y = SingletonTest.x;
            if (!string.IsNullOrEmpty(y))
            {
                Console.WriteLine(y);
            }
            Console.ReadKey();
        }
#endif
```

![Result pic 1](/contentimg/1/1.png "2.1.1.1执行情况")

　　根据main函数代码，直觉上，应该先输出“Starting Main”，然后输出“Echo!”，最后输出"After echo"和SingletonTest.x。总共4个输出。

　　但是实际：

　　1.程序运行–》2.执行main函数–》3.使用到静态SingletonTest–》4.静态字段x的初始化–》5.静态构造函数的初始化–》6.静态函数EchoAndReturn的使用
　　–》字段x的值由于执行顺序，不是默认赋值的”A_In type initializer”，而是静态构造函数中赋值的“A_static”;

　　如果这里不加静态构造函数？

![Result pic 2](/contentimg/1/2.png "2.1.1.2执行情况")

　　1.程序运行–》2.静态类型的初始化，静态SingletonTest、字段x提前初始化–》3.执行main函数。

　　这里在文末的博文中提到是：

>　　在类中实现静态构造函数，那beforefieldinit属性就会被precise属性替换，确保静态成员会在类第一次使用之前的那一刻进行初始化。如果不显式实现，静态成员会在类第一次使用之前的任何时间初始化（由CLR智能决定）。

　　比较得出：

>　　***显式静态构造函数使对象在被调用的时候才初始化，避免了static类型在程序启动的时候就提前初始化的问题。有利于程序的快速启动与内存的控制。***

#### 2.1.2 简单静态类单例模式实现

``` java
    public class SingletonTest
    {
        private static readonly SingletonTest _instance = new SingletonTest();
        static SingletonTest()
        {
        }
        private SingletonTest()
        {
        }
        public static SingletonTest Instance
        {
            get
            {
                return _instance;
            }
        }
    }
```

1.  将构造函数private；
1.  显式实现static构造函数；
1.  创建私有静态实例_instance并初始化；
1.  public开放实例对外访问接口 Instance 属性。
 
这样避免了静态类型的提前初始化，同时直接在初始化时候赋值的方式也避免了需要加锁的问题。

#### 2.1.3 单例模式执行顺序

``` java
    #if step21
    public class SingletonTest
    {
        private static readonly SingletonTest _instance = new SingletonTest();
        static SingletonTest()
        {
            x = "static init console";
            Console.WriteLine(x);
        }
        private SingletonTest()
        {
            x = "nonstatic init console";
            Console.WriteLine(x);
        }
        public static SingletonTest Instance
        {
            get
            {
                Console.WriteLine("Instance");
                return _instance;
            }
        }
        //测试静态字段，对执行顺序无影响，实际使用应该为非static，同时操作应该通过XX，不能直接操作本字段
        private static string x = "xxxx";
        public string XX
        {
            get { return x; }
            private set { x = value; }
        }
    }
#endif

```

　　main函数中输出 Console.WriteLine( SingletonTest.Instance.XX);
　　执行结果：

![Result pic 3](/contentimg/1/3.png "2.1.3执行情况")

　　1.程序启动–》2.遇到类SingletonTest–》3.优先初始化static属性_instance–》4._instance=new SingletonTest()进行实例化–》5.完成static属性的初始化后进行static构造函数的初始化–》6.调用SingletonTest.Instance–》7.获取结果XX得”static init”
　　–》注意这里不是先static init，然后才nonstatic init。因为属性的初始化优先于static构造函数。所以最后的结果是静态构造函数得执行顺序在nonstatic的后面。

#### 2.1.4 继承问题

　　这里题外话记录一下继承时的表现，进一步展示函数执行顺序：

　　增加子类型：

``` java
    public class SingletonTest_inherit : SingletonTest
    {
        static SingletonTest_inherit()
        {
            x = "B_static";
            Console.WriteLine("B_static init");
        }
        public SingletonTest_inherit()
        {
            x = "B_nonstatic";
            Console.WriteLine("B_nonstatic init");
        }
    }
```
　　执行结果：

![Result pic 4](/contentimg/1/4.png "2.1.4.1执行情况")


　　子对象的静态构造函数没有执行。

　　如果这里增加实例化？

![Result pic 5](/contentimg/1/5.png "2.1.4.2执行情况")

　　1.父类型字段x初始化–》2.父类型static构造函数执行–》3.子类型static构造函数执行–》4.父类型构造函数执行–》5.子类型构造函数执行
　　–》这里父类型和子类型共用同一个字段x，导致字段x的赋值被子类型重写。

![Result pic 6](/contentimg/1/6.png "2.1.4.3执行情况")

### 2.2 IoDH实现

#### 2.2.1 单例模式实现

　　推荐单例模式使用该方式。在类内部定义内部类来实现单例模式。

``` java
    public class SingletonTest
    {
        private class InnerCLass
        {
            static InnerCLass() { }
            internal static SingletonTest instance = new SingletonTest();
        }
        private SingletonTest() { }
        public static SingletonTest Instance
        {
            get { return InnerCLass.instance; }
        }
    }
```

#### 2.2.2 单例模式执行顺序

``` java
    public class SingletonTest
    {
        private class InnerCLass
        {
            static InnerCLass()
            {
                Console.WriteLine("InnerCLass static");
            }
            internal static SingletonTest instance = new SingletonTest();
        }
        private SingletonTest() {
            Console.WriteLine("SingletonTest nonstatic");
        }
        public static SingletonTest Instance
        {
            get {
                Console.WriteLine("Instance");
                return InnerCLass.instance; }
        }
        public string XX = "aaa";
    }
```

![Result pic 7](/contentimg/1/7.png "2.2.2执行情况")


## 三.相关参考链接

 主要参考博文: [https://www.cnblogs.com/rush/archive/2011/10/30/2229565.html](https://www.cnblogs.com/rush/archive/2011/10/30/2229565.html)

 提供继承方面的探讨: [https://www.cnblogs.com/jiagoushi/p/3775046.html](https://www.cnblogs.com/jiagoushi/p/3775046.html)

 对第一个博文进行扩展和实践: [https://blog.csdn.net/abc524061/article/details/57086267?utm_source=itdadao&amp;utm_medium=referral](https://blog.csdn.net/abc524061/article/details/57086267?utm_source=itdadao&amp;utm_medium=referral)

 本文测试程序工程可以从git直接获取：
 
 git代码库: [Codes](https://github.com/YaojiaxinPC/hexoblog/tree/master/SingletonTestDemo)
