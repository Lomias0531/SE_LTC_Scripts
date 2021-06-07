namespace System.Collections.Generic
{
	public struct ClearToken<T> : IDisposable
	{
		public List<T> List;

		public void Dispose()
		{
			List.Clear();
		}
	}
}
