using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace BenchmarkStaticNet
{
	public class Program
	{
		public static void Main()
		{
			//Util.AutoScrollResults = true;
			BenchmarkRunner.Run<MethodCalls>();
		}

		[ShortRunJob]
		[MinColumn, MaxColumn, MeanColumn, MedianColumn]
		[MemoryDiagnoser]
		[MarkdownExporter]
		public class MethodCalls
		{
			private MethodsContainer _c = new MethodsContainer();

			[Benchmark]
			public int Static()
			{
				return MethodsContainer.StaticMethod(1);
			}

			[Benchmark]
			public async Task<int> StaticAsync()
			{
				return await MethodsContainer.StaticAsyncMethod(1);
			}

			[Benchmark]
			public int Dynamic()
			{
				return _c.DynamicMethod(1);
			}

			[Benchmark]
			public async Task<int> DynamicAsync()
			{
				return await _c.DynamicAsyncMethod(1);
			}

			[Benchmark]
			public int Virtual()
			{
				return _c.VirtualMethod(1);
			}

			[Benchmark]
			public async Task<int> VirtualAsync()
			{
				return await _c.VirtualAsyncMethod(1);
			}
		}

		public class MethodsContainer
		{
			public static int StaticMethod(int arg)
			{ return arg * 2; }

			public static Task<int> StaticAsyncMethod(int arg)
			{ return Task.FromResult<int>(arg * 2); }

			public int DynamicMethod(int arg)
			{ return arg * 2; }

			public Task<int> DynamicAsyncMethod(int arg)
			{ return Task.FromResult<int>(arg * 2); }

			public virtual int VirtualMethod(int arg)
			{ return arg * 2; }

			public virtual Task<int> VirtualAsyncMethod(int arg)
			{ return Task.FromResult<int>(arg * 2); }
		}
	}
}