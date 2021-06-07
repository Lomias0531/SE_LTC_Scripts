namespace VRage.Collections
{
	public interface IMyElementAllocator<T> where T : class
	{
		bool ExplicitlyDisposeAllElements
		{
			get;
		}

		T Allocate(int bucketId);

		void Dispose(T instance);

		void Init(T instance);

		int GetBytes(T instance);

		int GetBucketId(T instance);
	}
}
