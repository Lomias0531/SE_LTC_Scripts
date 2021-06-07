namespace ParallelTasks
{
	public struct HashtableNode<TKey, TData>
	{
		public TKey Key;

		public TData Data;

		public HashtableToken Token;

		public HashtableNode(TKey key, TData data, HashtableToken token)
		{
			Key = key;
			Data = data;
			Token = token;
		}
	}
}
