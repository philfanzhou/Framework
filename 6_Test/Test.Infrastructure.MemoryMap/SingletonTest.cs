using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.CompilerServices;

namespace Test.Infrastructure.MemoryMap
{
    [TestClass]
    public class SingletonTest
    {
        [TestMethod]
        public void TestSingleton()
        {
            ManualResetEvent startEvent = new ManualResetEvent(false);
            List<Task> taskList = new List<Task>();

            for (int i = 0; i < 5; i++)
            {
                taskList.Add(Task.Factory.StartNew(() =>
                {
                    startEvent.WaitOne();

                    var obj = new MyClass();
                }));
            }

            startEvent.Set();
            Task.WaitAll(taskList.ToArray());

            Assert.IsTrue(MySingleton.Instance.Count == 1);
        }

        public class MyClass
        {
            private MySingleton singletonObj;

            public MyClass()
            {
                singletonObj = MySingleton.Instance;
                Thread.Sleep(200);
            }
            
        }

        public class MySingleton
        {
            private static MySingleton instance;
            private static int count;

            private MySingleton()
            {
                count++;
            }

            public static MySingleton Instance
            {
                get
                {
                    // 这样写单例是有bug的
                    if (null == instance)
                    {
                        System.Threading.Interlocked.CompareExchange(ref instance, new MySingleton(), null);
                    }
                    return instance;
                }
            }

            public int Count
            {
                get { return count; }
            }
        }
    }
}
