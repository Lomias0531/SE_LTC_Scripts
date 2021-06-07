using System.Collections.Generic;
using VRage.Generics;

namespace VRageRender
{
	public class MyBillboardBatch<T> where T : MyBillboard, new()
	{
		public readonly List<T> Billboards = new List<T>(3000);

		public readonly MyObjectsPool<T> Pool = new MyObjectsPool<T>(3000);

		public readonly Dictionary<int, MyBillboardViewProjection> Matrices = new Dictionary<int, MyBillboardViewProjection>(10);

		public void Clear()
		{
			Billboards.Clear();
			Pool.DeallocateAll();
		}
	}
}
