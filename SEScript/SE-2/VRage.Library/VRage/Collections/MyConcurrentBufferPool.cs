namespace VRage.Collections
{
	public class MyConcurrentBufferPool<TElement> : MyConcurrentBucketPool<TElement> where TElement : class
	{
		public MyConcurrentBufferPool(string debugName, IMyElementAllocator<TElement> allocator)
			: base(debugName, allocator)
		{
		}

		public new TElement Get(int bucketId)
		{
			return base.Get(GetNearestBiggerPowerOfTwo(bucketId));
		}

		/// <summary>
		///             Copy of VRageMath.MathHelper#GetNearestBiggerPowerOfTwo
		/// </summary>
		private static int GetNearestBiggerPowerOfTwo(int v)
		{
			v--;
			v |= v >> 1;
			v |= v >> 2;
			v |= v >> 4;
			v |= v >> 8;
			v |= v >> 16;
			v++;
			return v;
		}
	}
	public class MyConcurrentBufferPool<TElement, TAllocator> : MyConcurrentBufferPool<TElement> where TElement : class where TAllocator : IMyElementAllocator<TElement>, new()
	{
		public MyConcurrentBufferPool(string debugName)
			: base(debugName, (IMyElementAllocator<TElement>)new TAllocator())
		{
		}
	}
}
