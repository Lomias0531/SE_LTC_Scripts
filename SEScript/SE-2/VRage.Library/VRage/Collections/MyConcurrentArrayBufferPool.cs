using System;

namespace VRage.Collections
{
	public class MyConcurrentArrayBufferPool<TElement> : MyConcurrentBufferPool<TElement[], MyConcurrentArrayBufferPool<TElement>.ArrayAllocator>
	{
		public class ArrayAllocator : IMyElementAllocator<TElement[]>
		{
			private static readonly int ElementSize;

			public bool ExplicitlyDisposeAllElements => false;

			static ArrayAllocator()
			{
				ElementSize = MyConcurrentArrayBufferPool<TElement>.SizeOf<TElement>();
			}

			public TElement[] Allocate(int size)
			{
				return new TElement[size];
			}

			public void Init(TElement[] item)
			{
			}

			public int GetBytes(TElement[] instance)
			{
				return ElementSize * instance.Length;
			}

			public int GetBucketId(TElement[] instance)
			{
				return instance.Length;
			}

			public void Dispose(TElement[] instance)
			{
			}
		}

		public MyConcurrentArrayBufferPool(string debugName)
			: base(debugName)
		{
		}

		private static int SizeOf<T>()
		{
			Type typeFromHandle = typeof(T);
			if (!typeFromHandle.IsValueType)
			{
				return IntPtr.Size;
			}
			return TypeExtensions.SizeOf<T>();
		}
	}
}
