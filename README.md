{project:description}

[url:@chsword on Weibo.com|http://weibo.com/chsword]
[url:linkedin|http://lnkd.in/b6vqP_c]


*Planning*
!! v1.2.0.1
Negative value can't used.

*Released*
! V1.2.0.0 Futures
Fix DateTime bug.
! V1.1.0.0 Futures
Support using string index to access member like:
{code:c#}
dynamic json = new JDynamic("{a:{a:1}}");
Assert.AreEqual(1, json["a"]["a"]);
{code:c#}

! *Test Case*

En, you can use this util as following :
!! 1. Get the value directly
{code:c#}
dynamic json = new JDynamic("1");
//json.Value
{code:c#}
!! 2.Get the member in the json object
{code:c#}
dynamic json = new JDynamic("{a:'abc'}");
//json.a is a string "abc"

dynamic json = new JDynamic("{a:3.1416}");
//json.a is 3.1416m

dynamic json = new JDynamic("{a:1}");
//json.a is integer: 1
{code:c#}
!! 3.IEnemerable
{code:c#}
dynamic json = new JDynamic("[1,2,3]");
/json.Length/json.Count is 3
//And you can use json[0]/ json[2] to get the elements

dynamic json = new JDynamic("{a:[1,2,3]}");
//json.a.Length /json.a.Count is 3.
//And you can use  json.a[0]/ json.a[2] to get the elements

dynamic json = new JDynamic("[{b:1},{c:1}]");
//json.Length/json.Count is 2.
//And you can use the  json[0].b/json[1].c to get the num.
{code:c#}
!! 4. Other
{code:c#}
dynamic json = new JDynamic("{a:{a:1} }");
//json.a.a is 1.
{code:c#}
