using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Chsword.Reflections;

namespace Chsword.Test
{
    [TestClass]
    public class JDynamicUnitTest
    {

        [TestMethod]
        public void Self()
        {
            dynamic json = new JDynamic("1");
            Assert.AreEqual(1, json.Value);
        }
        [TestMethod]
        public void SelfDouble()
        {
            dynamic json = new JDynamic("1.1");
            Assert.AreEqual(1.1, json.Value);
        }
        [TestMethod]
        public void StringMember()
        {
            dynamic json = new JDynamic("{a:'abc'}");
            Assert.AreEqual("abc", json.a);
        }
        [TestMethod]
        public void DoubleMember()
        {
            dynamic json = new JDynamic("{a:3.1416}");
            Assert.AreEqual(3.1416, json.a);
        }
        [TestMethod]
        public void DoubleMember2()
        {
            dynamic json = new JDynamic("{a:-3.1416}");
            Assert.AreEqual(-3.1416, json.a);
        }
        [TestMethod]
        public void SimpleMember()
        {
            dynamic json = new JDynamic("{a:1}");
            Assert.AreEqual(1, json.a);
        }
        [TestMethod]
        public void IntMember()
        {
            dynamic json = new JDynamic("{a:-1}");
            Assert.AreEqual(-1, json.a);
        }
        [TestMethod]
        public void ArrayMember()
        {
            dynamic json = new JDynamic("{a:[-1,2,-3]}");
            Assert.AreEqual(-1, json.a[0]);
            Assert.AreEqual(2, json.a[1]);
            Assert.AreEqual(-3, json.a[2]);
        }
        [TestMethod]
        public void SimpleArray()
        {
            dynamic json = new JDynamic("[1,2,3]");
            Assert.AreEqual(3, json.Length);
            Assert.AreEqual(1, json[0]);
            Assert.AreEqual(3, json[2]);
        }
        [TestMethod]
        public void MemberArray()
        {
            dynamic json = new JDynamic("{a:[1,2,3]}");
            Assert.AreEqual(3, json.a.Length);
            Assert.AreEqual(1, json.a[0]);
            Assert.AreEqual(3, json.a[2]);
        }
        [TestMethod]
        public void ArrayObject()
        {
            dynamic json = new JDynamic("[{b:1},{c:1}]");
            Assert.AreEqual(2, json.Length);
            Assert.AreEqual(1, json[0].b);
            Assert.AreEqual(1, json[1].c);
            Assert.AreEqual(1, json[1]["c"]);
        }
        [TestMethod]
        public void InnerMember()
        {
            dynamic json = new JDynamic("{a:{a:1}}");
            Assert.AreEqual(1, json.a.a);
        }
        [TestMethod]
        public void StringIndex()
        {
            dynamic json = new JDynamic("{a:{a:1}}");
            Assert.AreEqual(1, json["a"]["a"]);
        }
        [TestMethod]
        public void SerializeObject()
        {
            var now = DateTime.Now;
            object obj = new { a = 1, b = "dd中文dd", c = now, d = 5.6, e = new { f = true, g = new { }, h = (string)null } };
            var expected = "{\"a\":1,\"b\":\"dd中文dd\",\"c\":\"" + now.ToString() + "\",\"d\":5.6,\"e\":{\"f\":true,\"g\":{},\"h\":null}}";
            Assert.AreEqual(expected, new JDynamic(obj).ToString());
        }

        [TestMethod]
        public void TestFastPropertyGet()
        {
            var a = new { ID = 1, Name = "张三" };
            TypeX type = a.GetType();

            Assert.AreEqual(1, type.GetProperty("ID").GetValue(a));
            Assert.AreEqual("张三", type.GetProperty("Name").GetValue(a));
        }

        [TestMethod]
        public void DynamicAttachMember()
        {
            //object obj = new {a = 1};
            //dynamic json = new JDynamic(obj);
            //json.b = 1;
            //Assert.AreEqual(1, json.b);
        }

        [TestMethod]
        public void TestDateTimeSupport()
        {
            var obj = new { a = DateTime.Now };
            var json = new JDynamic(obj).ToString();
            dynamic des = new JDynamic(json);
            Assert.AreEqual(obj.a.ToString(), des.a.ToString());
        }
    }
}
