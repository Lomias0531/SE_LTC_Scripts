using VRage.Collections;
using VRage.Library.Memory;

namespace VRage.Library
{
	public class NativeArrayAllocator : IMyElementAllocator<NativeArray>
	{
		private class NativeArrayImpl : NativeArray
		{
			private MyMemorySystem.AllocationRecord m_allocationRecord;

			public NativeArrayImpl(int size, MyMemorySystem memorySystem)
				: base(size)
			{
				m_allocationRecord = memorySystem.RegisterAllocation("NativeArray", size);
			}

			public override void Dispose()
			{
				base.Dispose();
				m_allocationRecord.Dispose();
			}
		}

		private readonly MyMemorySystem m_memoryTracker;

		public bool ExplicitlyDisposeAllElements => true;

		public NativeArrayAllocator(MyMemorySystem memoryTracker)
		{
			m_memoryTracker = memoryTracker;
		}

		public NativeArray Allocate(int size)
		{
			return new NativeArrayImpl(size, m_memoryTracker);
		}

		public void Dispose(NativeArray instance)
		{
			instance.Dispose();
		}

		public void Init(NativeArray instance)
		{
		}

		public int GetBytes(NativeArray instance)
		{
			return instance.Size;
		}

		public int GetBucketId(NativeArray instance)
		{
			return instance.Size;
		}
	}
}
