using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chsword.Test
{
    [TestClass]
    public class PerformanceTest
    {
        [TestMethod]
        public void Simple()
        {
            dynamic json = new JDynamic("{a:1}");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int a = 0;
            a = json.a;
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            sw.Reset();
            sw.Restart();
            for (int i = 0; i < 1000000; i++)
            {
                a = json.a;
            }
            sw.Stop();
            Console.WriteLine(a + "---------------");
            Console.WriteLine(sw.ElapsedMilliseconds);
            Assert.IsTrue(sw.ElapsedMilliseconds<1000);
            sw.Reset();
            sw.Restart();
            var obj = new {a = 1};
            for (int i = 0; i < 1000000; i++)
            {
                a = obj.a;
            }
            sw.Stop();
            Console.WriteLine(a+"---------------");
            Console.WriteLine(sw.ElapsedMilliseconds);
        }
        [TestMethod]
        public void Complex()
        {
            dynamic json = new JDynamic("{a:{b:{c:1}}}");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int a = 0;
            a = json.a.b.c;
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            sw.Reset();
            sw.Restart();
            for (int i = 0; i < 1000000; i++)
            {
                a = json.a.b.c;
            }
            sw.Stop();
            Console.WriteLine(a + "---------------");
            Console.WriteLine(sw.ElapsedMilliseconds);
        }
    }
}