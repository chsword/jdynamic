# JDynamic 
A tools transfer json to .NET Dynamic Object

[![install from nuget](http://img.shields.io/nuget/v/jDynamic.svg?style=flat-square)](https://www.nuget.org/packages/jDynamic)[![downloads](http://img.shields.io/nuget/dt/jDynamic.svg?style=flat-square)](https://www.nuget.org/packages/jDynamic)


@chsword on Weibo.com
linkedin

### NuGet Install
``` powershell
PM> Install-Package JDynamic
```

### Planning
v1.2.0.1

Negative value can't used.

### Released
V1.2.0.0 Futures
Fix DateTime bug.
V1.1.0.0 Futures

Support using string index to access member like:
``` csharp
dynamic json = new JDynamic("{a:{a:1}}");
Assert.AreEqual(1, json["a"]["a"]);
```
### Test Case

En, you can use this util as following :

1. Get the value directly
``` csharp
dynamic json = new JDynamic("1");

//json.Value
```
2.Get the member in the json object
``` csharp
dynamic json = new JDynamic("{a:'abc'}");
//json.a is a string "abc"

dynamic json = new JDynamic("{a:3.1416}");
//json.a is 3.1416m

dynamic json = new JDynamic("{a:1}");
//json.a is integer: 1
```
3.IEnemerable
``` csharp
dynamic json = new JDynamic("[1,2,3]");
/json.Length/json.Count is 3
//And you can use json[0]/ json[2] to get the elements

dynamic json = new JDynamic("{a:[1,2,3]}");
//json.a.Length /json.a.Count is 3.
//And you can use  json.a[0]/ json.a[2] to get the elements

dynamic json = new JDynamic("[{b:1},{c:1}]");
//json.Length/json.Count is 2.
//And you can use the  json[0].b/json[1].c to get the num.
```
4. Other
``` csharp
dynamic json = new JDynamic("{a:{a:1} }");
//json.a.a is 1.
```
