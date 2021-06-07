namespace VRage.Library.Collections
{
	public interface IMyQueue<T>
	{
		bool TryDequeueFront(out T value);
	}
}
