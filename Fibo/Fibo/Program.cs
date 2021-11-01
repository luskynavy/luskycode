using System;
using System.Diagnostics;

namespace Fibo
{
	internal class Program
	{
		//naive recursion
		private static long FiboRecur(int n)
		{
			if (n == 0)
			{
				return 0;
			}
			if (n == 1)
			{
				return 1;
			}
			return (FiboRecur(n - 1) + FiboRecur(n - 2));
		}

		//iterative version
		private static long FiboIter(int n)
		{
			if (n == 0)
			{
				return 0;
			}
			if (n == 1)
			{
				return 1;
			}

			long n1 = 1;
			long n2 = 0;
			long fiboN = 0;
			for (int i = 2; i <= n; i++)
			{
				fiboN = n1 + n2;
				n2 = n1;
				n1 = fiboN;
			}
			return fiboN;
		}

		//recursion terminal with accumulator
		private static long fibRt(int n, long a, long b)
		{
			if (n == 1)
			{
				return a;
			}

			return fibRt(n - 1, a + b, a);
		}

		private static long fibTerminal(int n)
		{
			if (n == 0)
			{
				return 0;
			}
			return fibRt(n, 1, 0);
		}

		private static void Main(string[] args)
		{
			int n = 40;
			Stopwatch stopWatch = Stopwatch.StartNew();
			Console.Write("FiboIter " + FiboIter(n));
			stopWatch.Stop();
			Console.WriteLine(" " + stopWatch.ElapsedMilliseconds + "ms");

			stopWatch = Stopwatch.StartNew();
			Console.Write("fibTerminal " + fibTerminal(n));
			stopWatch.Stop();
			Console.WriteLine(" " + stopWatch.ElapsedMilliseconds + "ms");

			if (n < 45)
			{
				stopWatch = Stopwatch.StartNew();
				Console.Write("FiboRecur " + FiboRecur(n));
				stopWatch.Stop();
				Console.WriteLine(" " + stopWatch.ElapsedMilliseconds + "ms");
			}
		}
	}
}