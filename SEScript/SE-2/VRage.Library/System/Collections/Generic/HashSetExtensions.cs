namespace System.Collections.Generic
{
	public static class HashSetExtensions
	{
		public static T FirstElement<T>(this HashSet<T> hashset)
		{
			using (HashSet<T>.Enumerator enumerator = hashset.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw new InvalidOperationException();
				}
				return enumerator.Current;
			}
		}
	}
}
