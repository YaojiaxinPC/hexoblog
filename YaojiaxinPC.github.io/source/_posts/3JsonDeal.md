---
title: Json解析C#的四个库
categories:
  - 开源库分享
tags:
  - C#
  - Json
---

　　目前通讯中http是使用最多的，而其中Json基本是首选。大家平时解析时都是直接调用dll，但是有没有考虑过dll里面怎么处理？这个dll又从哪里来？本文将分享我接触到的4个解析Json的C#开源库。
<!-- more -->
　　目前我用得较多的是Newtonsoft ，后面了解到还有轻量的MiniJSON，SimpleJson以及litjson。

　　这里MiniJSON最精简，是一个class文件，通过对string进行简单字符串的处理来解析。

　　然后就是SimpleJson，也是一个class文件，不过代码量超级多，功能相对比较全面。看其他博客对这个评价蛮高的，不过我没使用过，暂不评价。

　　litjson，就文件多一点，好几个class组成。但是看部分博客提到跨平台上有问题。

　　由于之前是做windows应用，所以一直使用的是Newtonsoft，代码量远超上面的几个，编译出来的dll也是比他们的要大。所以，比较推荐用Newtonsoft，可定制化高很多，功能和内部考虑的东西也完善。

　　本文暂不讲怎么使用，只简单介绍怎么去github获取上面几个的源码，以及怎么编译。

　　提到github，这是一个好东西，好多好的开源项目都在上面。不过国内网速超级慢，使用起来不是很方便。一般情况下，可以不注册账号直接下载项目代码，下载下来是一个zip文件。也可以用VS里面的扩展插件GitHub Extension for Visual Studio，下载安装过程有点久，请耐心等待：

![Result pic 1](/contentimg/3/1.png "GitHub Extension for Visual Studio")

　　使用起来相对没svn好用(当然，这里是可以用svn来使用的)。

　　推荐使用客户端：[GitHubDesktop](https://desktop.github.com/) 下载下来的应该是一个77M的全量包，直接安装就可以使用：

![Result pic 2](/contentimg/3/2.png "GitHubDesktop")

　　这里使用到的就是File--&gt;Clone...；打开后

![Result pic 3](/contentimg/3/3.png "Clone a repository")

　　这里的url，就是网页上获取的：

![Result pic 4](/contentimg/3/4.png "Clone with HTTPS")

　　这里贴一下看到的其他博主写的专门介绍GitHub Desktop的：[Windows 上 GitHub Desktop 的操作](https://www.cnblogs.com/hanford/p/6038417.html) 

## 一.MiniJSON

　　github地址：[MiniJSON](https://gist.github.com/darktable/1411710) 

　　直接在你的项目中新建一个class，全选代码后复制过去，就可以使用了。

　　由于需要vpn，所以这里放一下代码吧：


``` java
/*
 * Copyright (c) 2013 Calvin Rien
 *
 * Based on the JSON parser by Patrick van Bergen
 * http://techblog.procurios.nl/k/618/news/view/14605/14863/How-do-I-write-my-own-parser-for-JSON.html
 *
 * Simplified it so that it doesn't throw exceptions
 * and can be used in Unity iPhone with maximum code stripping.
 *
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the
 * "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to
 * permit persons to whom the Software is furnished to do so, subject to
 * the following conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
 * CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
 * TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
 * SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
 
namespace MiniJSON {
    // Example usage:
    //
    //  using UnityEngine;
    //  using System.Collections;
    //  using System.Collections.Generic;
    //  using MiniJSON;
    //
    //  public class MiniJSONTest : MonoBehaviour {
    //      void Start () {
    //          var jsonString = "{ \"array\": [1.44,2,3], " +
    //                          "\"object\": {\"key1\":\"value1\", \"key2\":256}, " +
    //                          "\"string\": \"The quick brown fox \\\"jumps\\\" over the lazy dog \", " +
    //                          "\"unicode\": \"\\u3041 Men\u00fa sesi\u00f3n\", " +
    //                          "\"int\": 65536, " +
    //                          "\"float\": 3.1415926, " +
    //                          "\"bool\": true, " +
    //                          "\"null\": null }";
    //
    //          var dict = Json.Deserialize(jsonString) as Dictionary<string,object>;
    //
    //          Debug.Log("deserialized: " + dict.GetType());
    //          Debug.Log("dict['array'][0]: " + ((List<object>) dict["array"])[0]);
    //          Debug.Log("dict['string']: " + (string) dict["string"]);
    //          Debug.Log("dict['float']: " + (double) dict["float"]); // floats come out as doubles
    //          Debug.Log("dict['int']: " + (long) dict["int"]); // ints come out as longs
    //          Debug.Log("dict['unicode']: " + (string) dict["unicode"]);
    //
    //          var str = Json.Serialize(dict);
    //
    //          Debug.Log("serialized: " + str);
    //      }
    //  }
 
    /// <summary>
    /// This class encodes and decodes JSON strings.
    /// Spec. details, see http://www.json.org/
    ///
    /// JSON uses Arrays and Objects. These correspond here to the datatypes IList and IDictionary.
    /// All numbers are parsed to doubles.
    /// </summary>
    public static class Json {
        /// <summary>
        /// Parses the string json into a value
        /// </summary>
        /// <param name="json">A JSON string.</param>
        /// <returns>An List&lt;object&gt;, a Dictionary&lt;string, object&gt;, a double, an integer,a string, null, true, or false</returns>
        public static object Deserialize(string json) {
            // save the string for debug information
            if (json == null) {
                return null;
            }
 
            return Parser.Parse(json);
        }
 
        sealed class Parser : IDisposable {
            const string WORD_BREAK = "{}[],:\"";
 
            public static bool IsWordBreak(char c) {
                return Char.IsWhiteSpace(c) || WORD_BREAK.IndexOf(c) != -1;
            }
 
            enum TOKEN {
                NONE,
                CURLY_OPEN,
                CURLY_CLOSE,
                SQUARED_OPEN,
                SQUARED_CLOSE,
                COLON,
                COMMA,
                STRING,
                NUMBER,
                TRUE,
                FALSE,
                NULL
            };
 
            StringReader json;
 
            Parser(string jsonString) {
                json = new StringReader(jsonString);
            }
 
            public static object Parse(string jsonString) {
                using (var instance = new Parser(jsonString)) {
                    return instance.ParseValue();
                }
            }
 
            public void Dispose() {
                json.Dispose();
                json = null;
            }
 
            Dictionary<string, object> ParseObject() {
                Dictionary<string, object> table = new Dictionary<string, object>();
 
                // ditch opening brace
                json.Read();
 
                // {
                while (true) {
                    switch (NextToken) {
                    case TOKEN.NONE:
                        return null;
                    case TOKEN.COMMA:
                        continue;
                    case TOKEN.CURLY_CLOSE:
                        return table;
                    default:
                        // name
                        string name = ParseString();
                        if (name == null) {
                            return null;
                        }
 
                        // :
                        if (NextToken != TOKEN.COLON) {
                            return null;
                        }
                        // ditch the colon
                        json.Read();
 
                        // value
                        table[name] = ParseValue();
                        break;
                    }
                }
            }
 
            List<object> ParseArray() {
                List<object> array = new List<object>();
 
                // ditch opening bracket
                json.Read();
 
                // [
                var parsing = true;
                while (parsing) {
                    TOKEN nextToken = NextToken;
 
                    switch (nextToken) {
                    case TOKEN.NONE:
                        return null;
                    case TOKEN.COMMA:
                        continue;
                    case TOKEN.SQUARED_CLOSE:
                        parsing = false;
                        break;
                    default:
                        object value = ParseByToken(nextToken);
 
                        array.Add(value);
                        break;
                    }
                }
 
                return array;
            }
 
            object ParseValue() {
                TOKEN nextToken = NextToken;
                return ParseByToken(nextToken);
            }
 
            object ParseByToken(TOKEN token) {
                switch (token) {
                case TOKEN.STRING:
                    return ParseString();
                case TOKEN.NUMBER:
                    return ParseNumber();
                case TOKEN.CURLY_OPEN:
                    return ParseObject();
                case TOKEN.SQUARED_OPEN:
                    return ParseArray();
                case TOKEN.TRUE:
                    return true;
                case TOKEN.FALSE:
                    return false;
                case TOKEN.NULL:
                    return null;
                default:
                    return null;
                }
            }
 
            string ParseString() {
                StringBuilder s = new StringBuilder();
                char c;
 
                // ditch opening quote
                json.Read();
 
                bool parsing = true;
                while (parsing) {
 
                    if (json.Peek() == -1) {
                        parsing = false;
                        break;
                    }
 
                    c = NextChar;
                    switch (c) {
                    case '"':
                        parsing = false;
                        break;
                    case '\\':
                        if (json.Peek() == -1) {
                            parsing = false;
                            break;
                        }
 
                        c = NextChar;
                        switch (c) {
                        case '"':
                        case '\\':
                        case '/':
                            s.Append(c);
                            break;
                        case 'b':
                            s.Append('\b');
                            break;
                        case 'f':
                            s.Append('\f');
                            break;
                        case 'n':
                            s.Append('\n');
                            break;
                        case 'r':
                            s.Append('\r');
                            break;
                        case 't':
                            s.Append('\t');
                            break;
                        case 'u':
                            var hex = new char[4];
 
                            for (int i=0; i< 4; i++) {
                                hex[i] = NextChar;
                            }
 
                            s.Append((char) Convert.ToInt32(new string(hex), 16));
                            break;
                        }
                        break;
                    default:
                        s.Append(c);
                        break;
                    }
                }
 
                return s.ToString();
            }
 
            object ParseNumber() {
                string number = NextWord;
 
                if (number.IndexOf('.') == -1) {
                    long parsedInt;
                    Int64.TryParse(number, out parsedInt);
                    return parsedInt;
                }
 
                double parsedDouble;
                Double.TryParse(number, out parsedDouble);
                return parsedDouble;
            }
 
            void EatWhitespace() {
                while (Char.IsWhiteSpace(PeekChar)) {
                    json.Read();
 
                    if (json.Peek() == -1) {
                        break;
                    }
                }
            }
 
            char PeekChar {
                get {
                    return Convert.ToChar(json.Peek());
                }
            }
 
            char NextChar {
                get {
                    return Convert.ToChar(json.Read());
                }
            }
 
            string NextWord {
                get {
                    StringBuilder word = new StringBuilder();
 
                    while (!IsWordBreak(PeekChar)) {
                        word.Append(NextChar);
 
                        if (json.Peek() == -1) {
                            break;
                        }
                    }
 
                    return word.ToString();
                }
            }
 
            TOKEN NextToken {
                get {
                    EatWhitespace();
 
                    if (json.Peek() == -1) {
                        return TOKEN.NONE;
                    }
 
                    switch (PeekChar) {
                    case '{':
                        return TOKEN.CURLY_OPEN;
                    case '}':
                        json.Read();
                        return TOKEN.CURLY_CLOSE;
                    case '[':
                        return TOKEN.SQUARED_OPEN;
                    case ']':
                        json.Read();
                        return TOKEN.SQUARED_CLOSE;
                    case ',':
                        json.Read();
                        return TOKEN.COMMA;
                    case '"':
                        return TOKEN.STRING;
                    case ':':
                        return TOKEN.COLON;
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    case '-':
                        return TOKEN.NUMBER;
                    }
 
                    switch (NextWord) {
                    case "false":
                        return TOKEN.FALSE;
                    case "true":
                        return TOKEN.TRUE;
                    case "null":
                        return TOKEN.NULL;
                    }
 
                    return TOKEN.NONE;
                }
            }
        }
 
        /// <summary>
        /// Converts a IDictionary / IList object or a simple type (string, int, etc.) into a JSON string
        /// </summary>
        /// <param name="json">A Dictionary&lt;string, object&gt; / List&lt;object&gt;</param>
        /// <returns>A JSON encoded string, or null if object 'json' is not serializable</returns>
        public static string Serialize(object obj) {
            return Serializer.Serialize(obj);
        }
 
        sealed class Serializer {
            StringBuilder builder;
 
            Serializer() {
                builder = new StringBuilder();
            }
 
            public static string Serialize(object obj) {
                var instance = new Serializer();
 
                instance.SerializeValue(obj);
 
                return instance.builder.ToString();
            }
 
            void SerializeValue(object value) {
                IList asList;
                IDictionary asDict;
                string asStr;
 
                if (value == null) {
                    builder.Append("null");
                } else if ((asStr = value as string) != null) {
                    SerializeString(asStr);
                } else if (value is bool) {
                    builder.Append((bool) value ? "true" : "false");
                } else if ((asList = value as IList) != null) {
                    SerializeArray(asList);
                } else if ((asDict = value as IDictionary) != null) {
                    SerializeObject(asDict);
                } else if (value is char) {
                    SerializeString(new string((char) value, 1));
                } else {
                    SerializeOther(value);
                }
            }
 
            void SerializeObject(IDictionary obj) {
                bool first = true;
 
                builder.Append('{');
 
                foreach (object e in obj.Keys) {
                    if (!first) {
                        builder.Append(',');
                    }
 
                    SerializeString(e.ToString());
                    builder.Append(':');
 
                    SerializeValue(obj[e]);
 
                    first = false;
                }
 
                builder.Append('}');
            }
 
            void SerializeArray(IList anArray) {
                builder.Append('[');
 
                bool first = true;
 
                foreach (object obj in anArray) {
                    if (!first) {
                        builder.Append(',');
                    }
 
                    SerializeValue(obj);
 
                    first = false;
                }
 
                builder.Append(']');
            }
 
            void SerializeString(string str) {
                builder.Append('\"');
 
                char[] charArray = str.ToCharArray();
                foreach (var c in charArray) {
                    switch (c) {
                    case '"':
                        builder.Append("\\\"");
                        break;
                    case '\\':
                        builder.Append("\\\\");
                        break;
                    case '\b':
                        builder.Append("\\b");
                        break;
                    case '\f':
                        builder.Append("\\f");
                        break;
                    case '\n':
                        builder.Append("\\n");
                        break;
                    case '\r':
                        builder.Append("\\r");
                        break;
                    case '\t':
                        builder.Append("\\t");
                        break;
                    default:
                        int codepoint = Convert.ToInt32(c);
                        if ((codepoint >= 32) && (codepoint <= 126)) {
                            builder.Append(c);
                        } else {
                            builder.Append("\\u");
                            builder.Append(codepoint.ToString("x4"));
                        }
                        break;
                    }
                }
 
                builder.Append('\"');
            }
 
            void SerializeOther(object value) {
                // NOTE: decimals lose precision during serialization.
                // They always have, I'm just letting you know.
                // Previously floats and doubles lost precision too.
                if (value is float) {
                    builder.Append(((float) value).ToString("R"));
                } else if (value is int
                    || value is uint
                    || value is long
                    || value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is ulong) {
                    builder.Append(value);
                } else if (value is double
                    || value is decimal) {
                    builder.Append(Convert.ToDouble(value).ToString("R"));
                } else {
                    SerializeString(value.ToString());
                }
            }
        }
    }
}
```

## 二.SimpleJson

　　github地址：[SimpleJson](https://github.com/facebook-csharp-sdk/simple-json) 

　　你可以下载下来然后用vs打开选Net2.0编译出来一个dll（这里可能会报错，将文件EscapeToJavascriptStringTests去掉就正常了）。但是你也可以像前面那样，新建一个class，然后复制SimpleJson.cs的代码，点击SimpleJson.cs后会进到下面的详情网页，选择图中的Raw，就能进到代码页面，然后Ctrl+A进行全选复制。

![Result pic 5](/contentimg/3/5.png "Get Raw Codes")


## 三.LitJSON

　　github地址：[LitJSON](https://github.com/LitJSON/litjson) 

　　这个生成超级烦，要搭.net core的环境。由于没接触core开发，后面我还是修改vs工程文件，去掉core后才能生成。

　　（后面发现，其实这里是因为VS的版本问题，要新版本。这里有篇文章就是问这个的：[msbuild-of-vs2017-cannot-compile-net-standard-2-0-project](https://stackoverflow.com/questions/45979627/msbuild-of-vs2017-cannot-compile-net-standard-2-0-project)  查官网，只提到装2017，但是明明我的是2017（但是是16年底下载的，Core2.0是17年底出的），.NET Core 2.0.0 SDK这个也安装了，就是无法编译，其实是msbuild 版本。这个问题，和VS2010的msbuild编译不了2015版本的项目是一样的。所以如果装了新版本的，这里不用修改，是可以直接编译的。）

　　这里也贴一下主页：[LitJSON](https://litjson.net/) 

　　以及一个老版本的直接dll下载地址：[DownloadDll](https://sourceforge.net/projects/litjson/) 

　　推荐直接下载github的然后自己编译，因为后面有更新，而网上那些，好多都是好几年前的版本。

下面记录一下我怎么编译的：

　　实际是修改vs工程文件，然后直接用vs打开就可以编译的，不过这里我也记录一下他自带的那些工具是怎么配置到可以用的。

　　先记录怎么修改vs工程文件，目录中，实际只要用到\litjson-develop\src\LitJson里的东西：

![Result pic 6](/contentimg/3/6.png "修改vs工程文件 LitJSON.csproj")

　　用记事本打开，ctrl+f查找包含“netstandard”的字段，就是下面截图红色部分，删掉：

![Result pic 7](/contentimg/3/7.png "修改LitJSON.csproj，删除部分编译条件")

　　这里简单说明一下为什么要删掉：

　　“netstandard”部分是core环境的，目前我没搭这个环境，所以直接编译是会报错的。

　　而中间的那个，是检查git的，如果你是部署了git，登陆了帐号，就没影响，可以更新并编译，不然就会报错。

　　删掉上面红色部分后，用vs打开，直接编译就成功了。

接下来介绍怎么修改自带的那些工具来生成，过程繁琐，不过不用修改vs工程文件：

　　首先运行build.ps1，记住不是sh（这个是linux的），右键--&gt;使用PowerShell运行：

![Result pic 8](/contentimg/3/8.png "find build.ps1")

![Result pic 9](/contentimg/3/9.png "run build.ps1")

　　出现下面这样的窗口，实际是里面代码，分析出你要安装这个环境的这个包，正常情况是一直下载不下来的，vpn也救不了，后面挂[百度云离线](https://pan.baidu.com/s/1qJMXKCqwyuIbe73ub3uueA%C2%A0)  （ 提取码：0osd）总算下载下来了。

![Result pic 10](/contentimg/3/10.png "get download link")

　　现在目录中多了一个文件夹，里面是安装core的环境的，可以安装，目前我系统是win10版：

![Result pic 11](/contentimg/3/11.png "Window Version")

　　这里也简单记一下怎么修改代码让dotnet-install.ps1文件能运行：

　　首先右键-->编辑；里面好多脚本代码，和C#超级像，有兴趣可以去了解：

![Result pic 12](/contentimg/3/12.png "Open dotnet-install.ps1")

　　这里意思是传一个下载链接给它，然后下载解压（上面一堆脚本都是下载函数）。而现在这个url，是下不下来的，不过我们可以通过IIS，进行localhost下载：

　　windows启用IIS，在程序与功能-->启用或关闭windows功能，然后你看到有iis的就打勾就行了。

![Result pic 13](/contentimg/3/13.png "Run IIS")

　　然后找到上面图中这个，点击“浏览”，把刚才通过百度云下载的dotnet-sdk-2.1.4-win-x64.zip复制过去，当然还有复制dotnet-install.ps1这个文件，不过.ps1这个后缀，在iis里面不能访问，所以我修改成txt，反正脚本中只是找文件下载，下载后保存的命名是自己定的。

![Result pic 14](/contentimg/3/14.png "Files")

　　然后就是修改脚本：

![Result pic 15](/contentimg/3/15.png "Edit Scripts")

　　修改build.ps1，告诉它去localhost目录下载这个txt；同时修改这个txt去localhost下载这个zip。

　　这样就能正常运行了，但是这里由于网络原因，还会报错，就安装cake部分，不过可以重新启动就行，cake的下载和安装过程较顺利，安装好后多了3个文件夹：

![Result pic 16](/contentimg/3/16.png "Cake Files")

　　一个是检查git的，一个是编译工具cake。

　　所以如果想用这个工具编译，最好是登陆git的，这样就能用自带工具编译了，core也能编译：

![Result pic 17](/contentimg/3/17.png "Run build.ps1")

　　这里我删了core的环境安装部分，然后直接运行build.ps1就可以了。

　　这种方式就不用修改vs工程文件。

　　从ps1文件里面了解到，是调用cake来编译的，所以，其实我们可以直接cmd来操作：

![Result pic 18](/contentimg/3/18.png "build by cake")

　　cmd到build.cake所在目录，然后调用cake.exe。

## 四.Newtonsoft.Json

　　主页：[www.newtonsoft.com](https://www.newtonsoft.com/json) 

　　github地址：[Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json) 

　　这个就编译比较简单，但是会报错readonly struct（去掉readonly就行），用vs编辑工程文件，像前面那个一样删掉core和phone等等那些选项，然后再编译就能编译通过。自带的脚本也是，下载部分很顺利就能下载，155m左右；但是还有附加的编译环境，也是core的。

　　这里这个错，其实是C# 7的特性，老版本的vs都不支持。然后core的问题，其实也是更新新版本vs就能解决，后面我更新了新版本后，msbuild版本上来了，就能正常编译了。为什么在第三个那里没注意到时msbuild，因为那里用的是cake，所以当时是能用脚本编译，但是不能vs编译，后面找多点资料后才想起是这个可能。所以这里我就直接更新vs，然后就能直接编译了。什么都不用改。

　　这里放一下vs2017最新版的安装索引包吧：

两个官网下载地址：

　　地址1：[vs2017-relnotes#15.1.26430.06](https://docs.microsoft.com/en-us/visualstudio/releasenotes/vs2017-relnotes#15.1.26430.06) 

　　地址2：[visualstudio.microsoft.com](https://visualstudio.microsoft.com/zh-hans/downloads/) 

　　以及.NET downloads：[.NET](https://www.microsoft.com/net/download) 

　　附加一下百度云，就是本篇文章用到的配置环境所要下载的东西（要vpn的那几个）：

　　[链接](https://pan.baidu.com/s/1X0HYx4geqPqSh-WD5xBbXg)  提取码：1t2x

　　这里有个文章提到这个新特性：[C# 7 Series, Part 6: Read-only structs](https://blogs.msdn.microsoft.com/mazhou/2017/11/21/c-7-series-part-6-read-only-structs/) 

![Result pic 19](/contentimg/3/19.png "Git edit History")

　　以及两篇介绍的：

　　[.NET Core 2.0及.NET Standard 2.0](https://www.cnblogs.com/zjoch/p/6696986.html) 

　　[NET Standard/Core中配置使用TargetFrameworks输出多版本类库及测试：](https://blog.csdn.net/starfd/article/details/78839704) 

　　平时用这个库比较多的原因，就是可定制化高，可以设置[null值要不要序列化](https://www.cnblogs.com/sczmzx/p/7813715.html)  ，等等之类的定制。

结尾附几个博主写的库分享：

　　[序列化效率比拼——谁是最后的赢家Newtonsoft.Json](https://www.cnblogs.com/landeanfen/p/4627383.html) 

　　[几个常用Json组件的性能测试](https://www.cnblogs.com/blqw/p/3274229.html) 

