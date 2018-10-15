---
title: C# Obsolete、Contional等等一些特性的介绍
categories:
  - 编程技巧
tags:
  - C#
  - Attribute特性
  - enum特性
---
	
　　本文分享一些代码使用Attribute的高级写法部分，日常使用中应该很少用到，但是是能给代码书写带来很好的优化效果的。
<!-- more -->
　　这些东西其实和xml解析时定义的class中做的标记、自定义配置文件的class标记、枚举中文标记等等是类似的。都是在上面“[]”+标记。

　　日常编码中，应该Enum的中文标记是使用最多的，接下来就是xml解析，自定义配置文件应该用的人比较少，毕竟实用性不大，直接定义字段，或者使用系统自带的要容易用一些。接下来分享的几个，应该较少人接触过。

## 一.Obsolete

　　类似tooltip提示，一般写接口的人会用到，用于标记函数是否过时，以及强制让函数无法通过编译。

``` java
    public static class TestObsolete
    {
        [Obsolete("OldMethod,please use NewMethod", false)]
        public static void OldMethod()
        {
            Console.WriteLine("Old Method");
        }

        [Obsolete("Error Method,you can't use this method!", true)]
        public static void ErrorMethod()
        {
            Console.WriteLine("Error Method");
        }

        public static void NewMethod()
        {
            Console.WriteLine("New Method");
        }
    }
```
　　实例中的三个方法是一样的操作：调用输出信息。照理说，是能直接调用然后输出的，毕竟没有语法错误，编译器也没提示函数有问题。

　　但实际使用时是：一个警告，一个直接错误。

![Result pic 1](/contentimg/4/1.png "Obsolete")

　　直接给你一个error，不让编译通过。

![Result pic 2](/contentimg/4/2.png "ObsoleteAttribute")

　　查看定义，该属性是继承Attribute，这样用法就是直接标在函数或者成员上面，并用“[]”包起来。该特性有三种构造函数，第二种常用，就是让函数提示过时，同时里面有message提示信息。用法就是上面第一张图那样，让函数本身“过时”，给了一个警告提，同时鼠标移上去时，会提示message的信息。第三种构造函数，就是加了error，标记是显示成“警告”（可以编译通过），还是“错误”（无法编译通过）。

　　这个特性，一般写接口的人会使用到，在接口的版本更新后，如果替换了新接口，但是又想老程序能使用时，一般保留老接口的代码。不过这里就有问题了，接下来新使用的程序，应该让他们用新接口而不是用老接口：如果说写在文档里面说明这个情况，但是好多人是连接口文档都不看的，直接dll引用就开始写代码的；如果写在注释里面？那更加不行，一般没报错，是很少有人去看注释的。所以这时候就得用特性了，使用这个特性，写代码的时候就直接编译器提示了，使用者全部都会看到这个提示。（unity经常用这个特性提示每次更新版本后丢弃的老属性，不过现在国内绝大部分公司，都是直接删掉老函数，然后拉分支来处理的，这样导致后期一大堆分支，维护很麻烦）

## 二.Conditional

　　一个好玩的特性，类似于 #if XXXX  #elif  XXXX #else XXX #endif 这种使用：

![Result pic 3](/contentimg/4/3.png "预编译条件#if")

　　使用方式如下：

``` java
using System;
using System.Diagnostics;

namespace FeaturesDemo
{
    public class TestConditional
    {
        [Conditional("ConsoleOut")]
        public static void ConsoleOut(string message)
        {
            Console.WriteLine(message);
        }
    }
}
```

　　调用时：发现明明代码写在那里，但是却不执行：

![Result pic 4](/contentimg/4/4.1.png "no done?")

　　因为它的使用前提是你要提示它“要执行”，它才会执行。不然没提示，代码在，但是不执行。

怎么提示：

　　方法一：在开头写#define +构造函数中传递的字符串

![Result pic 5](/contentimg/4/4.2.png "预编译#define")

　　方法二：使用“条件编译符号”

![Result pic 6](/contentimg/4/4.3.png "条件编译符号")

这里插播一下怎么运行core程序，因为vs编译生成的是dll：

![Result pic 7](/contentimg/4/5.1.png "cmd run core")

　　cd到vs工程文件所在地方，然后“dotnet build”或者“dotnet run”都行，run是编译后同时运行，然后cd到dll所在地方，“dotnet”+项目名，就能运行。当然目录下要有“项目名.runtimeconfig.json”这个文件（标记目标环境）

![Result pic 8](/contentimg/4/5.2.png "项目名.runtimeconfig.json文件")

　　这里分享几个参考的博文：

[.NET Core全面扫盲贴](https://www.cnblogs.com/Wddpct/p/5694596.html#3.1.2)
 
[.NET Core - .NET 使用 .NET Core 跨平台运行](https://msdn.microsoft.com/zh-cn/magazine/mt694084#rd)
  
[.NET Core 跨平台执行命令、脚本](https://www.cnblogs.com/stulzq/p/9074965.html)

## 三.CallXXXX特性

　　这个东西实用性一般般，用于调试排错的时候，就是你找到在哪里出错了，但是看代码又不知道上一层是哪个函数（就是哪里调用这个函数导致出问题），就加这个特性，一层一层排上去，就能找到是哪里出问题了。

![Result pic 9](/contentimg/4/6.png "CallXXXX特性")

　　之前不知道有这个特性的时候，我都是直接用反射，找到哪个函数调用，然后一层一层反射上去。反射的过程超级麻烦，实用性也不好。当然，如果可以，建议直接用vs远程附加调试，打断点就能知道到底怎么出问题了。

## 四.DebuggerStepThrough

　　好吧，这个算凑数的。

　　这个特性是调试的时候，F10和F11的区别，就是如果函数加了这个特性，执行到这个函数，就算你使用F11（单步，逐语句），它也给你当F10（整个函数直接过，逐过程）。

　　"[System.Diagnostics.DebuggerStepThrough]"

``` java
using System;
using System.Runtime.CompilerServices;

namespace FeaturesDemo
{
    public static class TestCall
    {
        [System.Diagnostics.DebuggerStepThrough]
        public static void CallMethod(string message, [CallerFilePath] string filepath = "", [CallerLineNumber]int linenumber = 0,
            [CallerMemberName]string membername = "")
        {
            Console.WriteLine(message);
            if (!string.IsNullOrEmpty(filepath))
                Console.WriteLine("filepath:" + filepath);
            if (!linenumber.Equals(0))
                Console.WriteLine("linenumber:" + linenumber);
            if (!string.IsNullOrEmpty(membername))
                Console.WriteLine("membername:" + membername);
        }
    }
}
```

## 五.枚举中文Description

　　Description，这个最常用。

``` java
using System;
using System.ComponentModel;
using System.Reflection;

namespace FeaturesDemo
{
    public static class EnumCN
    {
        public static string GetDescription(Enum obj)
        {
            FieldInfo fi = obj.GetType().GetField(obj.ToString());
            DescriptionAttribute[] arrDesc = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (arrDesc != null && arrDesc.Length > 0)
                return arrDesc[0].Description;
            else
                return null;
        }
    }

    public enum PersonSex
    {
        [Description("男人")]
        Man = 0,
        [Description("女人")]
        Woman = 1,
        [Description("人妖")]
        OtherSex = 2
    }
}
```

　　这里就不多解释了。做界面的人建议多用，在一些选项框中，中文对应枚举，比对应源的第几个要好用，也不容易出问题。

![Result pic 10](/contentimg/4/7.png "枚举中文Description")

这里插播一下enum的另一个用法：权限校验

　　就是把枚举和二进制关联起来，0表示没这个权限，1表示有，然后进行与或非操作就能判断。

定义枚举：
``` java
    [Flags]
    public enum EnumPower
    {
        [Description("游客")]
        AllNull = 0,             //0x00 表示全部没有    0000
        [Description("创建")]
        Create = 1 << 0,         //0x01或者1  2的0次方  0001
        [Description("读取")]
        Read = 1 << 1,           //0x02或者2  2的1次方  0010
        [Description("更新")]
        Update = 1 << 2,         //0x04或者4  2的2次方  0100
        [Description("删除")]
        Delete = 1 << 3          //0x08或者8  2的3次方  1000
    }
```

　　接下来，把权限“与”/“或”操作起来，然后检查是否某位上为1就是有该权限。使用该用法，数据库保存用户权限就容易了，例如有read和delete权限的，直接0010|1000=1010。

　　判断是否有read权限时：1010&0010 = 0010->1表示有；当没有时：1000&0010=0000->0

　　判断是否有read或者delete其中之一：1010&(0010|1000)=1010->都有。或者其中一个有0010&(0010|1000)=0010。

　　可以发现，判断权限时，我们只要&|操作后，检查==0？就可以了。省了一堆if else if语句。

可以合并成一个函数：

``` java
    public static class CheckPower
    {
        /// <summary>
        /// 检查是否有权限，
        /// |操作会合并成一个值，表示满足之一就返回true
        /// 如果有多个值，表示需要满足全部值才返回true
        /// </summary>
        /// <param name="ower">用户的权限值</param>
        /// <param name="powers">要检查的权限值</param>
        /// <returns></returns>
        public static bool CheckHasPower(EnumPower owner, params EnumPower[] powers)
        {
            //没有传权限过来，直接返回false
            if (powers == null || powers.Length < 1)
                return false;
            //只传一个权限(单权限，或者几个权限的|操作)
            if (powers.Length <= 1)
            {
                //如果是EnumPower.AllNull，直接比较是否和owner一样
                if (powers[0].Equals(EnumPower.AllNull))
                    return owner.Equals(powers[0]);
                else
                    return (owner & powers[0]) != 0x00;
            }

            //合并要检查的权限值
            foreach (var item in powers)
            {
                //核对每一项，如果有一项不成立，返回false
                if ((owner & item) == 0x00)
                    return false;
            }

            return true;
        }
    }
```

　　使用：

``` java
    EnumPower owner = EnumPower.AllNull;
    owner = EnumPower.Delete | EnumPower.Create;

    Console.WriteLine(owner);

    Console.WriteLine("EnumPower.Delete:" + CheckPower.CheckHasPower(owner, EnumPower.Delete) + System.Environment.NewLine);
    Console.WriteLine("EnumPower.Read:" + CheckPower.CheckHasPower(owner, EnumPower.Read) + System.Environment.NewLine);
    Console.WriteLine("EnumPower.AllNull:" + CheckPower.CheckHasPower(owner, EnumPower.AllNull) + System.Environment.NewLine);
    Console.WriteLine("EnumPower.Delete | EnumPower.Update:" + CheckPower.CheckHasPower(owner, EnumPower.Delete | EnumPower.Update) + System.Environment.NewLine);
    Console.WriteLine("EnumPower.AllNull | EnumPower.Update:" + CheckPower.CheckHasPower(owner, EnumPower.AllNull | EnumPower.Update) + System.Environment.NewLine);
    Console.WriteLine("EnumPower.Update | EnumPower.Delete:" + CheckPower.CheckHasPower(owner, EnumPower.Update | EnumPower.Delete) + System.Environment.NewLine);
    Console.WriteLine("EnumPower.Update &EnumPower.Delete:" + CheckPower.CheckHasPower(owner, EnumPower.Update, EnumPower.Delete) + System.Environment.NewLine);

    Console.WriteLine("EnumPower.Read | EnumPower.Update | EnumPower.Create | EnumPower.Delete:" + CheckPower.CheckHasPower(owner, EnumPower.Read | EnumPower.Update | EnumPower.Create | EnumPower.Delete) + System.Environment.NewLine);

    int enumlength = Enum.GetNames(typeof(EnumPower)).Length - 1;//0开始，所以要-1
    //等价于前面全部|的操作
    Console.WriteLine(((EnumPower)(1 << enumlength) - 1) + ":" + CheckPower.CheckHasPower(owner, ((EnumPower)(1 << enumlength) - 1)) + System.Environment.NewLine);

	Console.WriteLine("EnumPower.Read &EnumPower.Update &EnumPower.Create &EnumPower.Delete:" + CheckPower.CheckHasPower(owner, EnumPower.Read, EnumPower.Update, EnumPower.Create, EnumPower.Delete));
```

![Result pic 11](/contentimg/4/8.png "权限校验")

　　这里比较好玩的是“1<<n”，表示2的n次方。注意“|”和“&”的区别就可以使用了，特别注意&操作，前后顺序，有没有加（），结果是不同的。以及拥有全部权限的另类算法，应该是1111（4个1）,即10000（5位）-1.

　　当然普遍写法是直接|或者&然后判断==0？，就不会写一个函数来增加多余的部分的。所以上面这个函数显得突兀，只是为了直观理解而写的。

## 六.自定义特性

　　特性类的后缀要以Attribute结尾，需要继承自System.Attribute，一般情况下声明为sealed。

示例如下：注意使用的时候自动去除Attribute后缀

![Result pic 12](/contentimg/4/9.1.png "自定义特性示例")

　　获取内容的方式：

``` java
    public static class TestMyMethod
    {
        public static void GetFromClass(TestMyClass t)
        {
            Console.WriteLine(t.TestProperty);
            MyAttribute[] arrDesc = Attribute.GetCustomAttributes(t.GetType(), typeof(MyAttribute)) as MyAttribute[];
            if (arrDesc != null)
                foreach (var item in arrDesc)
                {
                    Console.WriteLine("Description:" + item.Description);
                    Console.WriteLine("LineNumber:" + item.LineNumber);
                    Console.WriteLine("IsClass:" + item.IsClass);
                    Console.WriteLine();
                }
        }

        public static void GetFromProperty(TestMyProperty t)
        {
            Console.WriteLine(t.TestProperty);
            foreach (System.Reflection.PropertyInfo item in t.GetType().GetProperties())
            {
                MyAttribute[] atts = Attribute.GetCustomAttributes(item, typeof(MyAttribute)) as MyAttribute[];
                if (atts == null) continue;
                foreach (var at in atts)
                {
                    Console.WriteLine("Description:" + at.Description);
                    Console.WriteLine("LineNumber:" + at.LineNumber);
                    Console.WriteLine("IsClass:" + at.IsClass);
                    Console.WriteLine();
                }
            }
        }
    }
```

　　注意当特性放在不同的地方时获取方式不同，类中的字段是class.GetProperties()下的内容，然后才可以 Attribute.GetCustomAttributes

![Result pic 13](/contentimg/4/9.2.png "获取方式不同")

　　注意当命名不规范的时候，是不会自动裁剪后缀的：

![Result pic 14](/contentimg/4/9.3.png "命名不规范")

　　当然还有C++和其他dll导入时的特性，xml编辑的特性，以及ORM特性等等，由于那些的主题应该是对应的内容，特性只是一个小小的标记，所以那部分的内容到时放在具体项目中分享。

 本文测试程序工程可以从git直接获取：
 
 git代码库: [Codes](https://github.com/YaojiaxinPC/hexoblog/tree/master/FeaturesDemo)