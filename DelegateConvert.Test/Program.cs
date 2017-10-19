using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DelegateConvert;
using System.Diagnostics;
using JSocket.Utilities.Generic.Delegate.DelegateConvert;
using System.Reflection;

namespace DelegateConvert.Test
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            TimedTests.RawTest();
            TimedTests.MethodInvokeTest();
            TimedTests.DynamicInvokeTest();

            Console.ReadLine();
            ActionTest();
            FunctionTest();
            Debugger.Break();
        }

        private static void ActionTest()
        {
            Action<string, string, int> sourceAction = (x, y, z) => Console.WriteLine($"STR1: {x} STR2: {y} INT: {z}");
            Action<object[]> myDele = ActionConvert.Convert(sourceAction);
            myDele(new object[] { "Hey", "Yo", 123 });
        }

        private static void FunctionTest()
        {
            Func<int, int, int, bool> checksum = (x, y, z) => x + y == z;
            Func<object[], bool> myDele = FuncConvert.Convert<bool>(checksum);
            if (myDele(new object[] { 1, 2, 3 }))
            {
                Console.WriteLine("Yay");
            }
            else
            {
                Console.WriteLine("Nay");
            }
        }
    }
    public class TimedTests
    {
        private const int Loops = 5000000;

        private static MethodInfo GetMethodInfo()
        {
            return typeof(DummyCaller).GetMethod("Sum");
        }

        public static void RawTest()
        {
            var instance = new DummyCaller();
            instance.Sum(1, 2, 3, 4, 5, 6, 7, 8);

            var sw = Stopwatch.StartNew();
            for (int i = 0; i < Loops; i++)
            {
                instance.Sum(1, 2, 3, 4, 5, 6, 7, 8);
            }
            Console.WriteLine($"{Loops} Rawtests took {sw.Elapsed.TotalMilliseconds} ms");
        }

        public static void MethodInvokeTest()
        {
            var instance = new DummyCaller();
            var method = GetMethodInfo();
            var invokeParams = new object[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            var sw = Stopwatch.StartNew();
            for (int i = 0; i < Loops; i++)
            {
                // instance.Sum(1, 2, 3, 4, 5, 6, 7, 8);

                method.Invoke(instance, invokeParams);
            }
            Console.WriteLine($"{Loops} MethodInvokeTest took {sw.Elapsed.TotalMilliseconds} ms");
        }

        public static void DynamicInvokeTest()
        {
            var instance = new DummyCaller();
            var method = GetMethodInfo();
            var invokeParams = new object[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            var dele = method.CreateDelegate(typeof(Func<int, int, int, int, int, int, int, int, int>), instance);

            var sw = Stopwatch.StartNew();
            for (int i = 0; i < Loops; i++)
            {
                method.Invoke(instance, invokeParams);
            }
            Console.WriteLine($"{Loops} DynamicInvokeTest took {sw.Elapsed.TotalMilliseconds} ms");
        }
    }
    public class DummyCaller
    {
        public int Sum(int p1, int p2, int p3, int p4, int p5, int p6, int p7, int p8)
        {
            return p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8;
        }
    }
}