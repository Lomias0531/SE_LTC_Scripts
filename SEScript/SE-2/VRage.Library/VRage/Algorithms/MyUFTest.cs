using System;
using System.Diagnostics;
using System.IO;

namespace VRage.Algorithms
{
	public static class MyUFTest
	{
		public static void Test()
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			int num = 10000000;
			MyUnionFind myUnionFind = new MyUnionFind();
			myUnionFind.Resize(num);
			for (int i = 0; i < num; i++)
			{
				myUnionFind.Union(i, i >> 1);
			}
			int num2 = myUnionFind.Find(0);
			for (int j = 0; j < num; j++)
			{
				if (num2 != myUnionFind.Find(j))
				{
					File.AppendAllText("C:\\Users\\daniel.ilha\\Desktop\\perf.log", "FAIL!\n");
					Environment.Exit(1);
				}
			}
			long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
			File.AppendAllText("C:\\Users\\daniel.ilha\\Desktop\\perf.log", $"Test took {elapsedMilliseconds:N}ms\n");
		}
	}
}
