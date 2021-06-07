namespace VRage.Collections
{
	public abstract class HeapItem<K>
	{
		public int HeapIndex
		{
			get;
			internal set;
		}

		public K HeapKey
		{
			get;
			internal set;
		}
	}
}
