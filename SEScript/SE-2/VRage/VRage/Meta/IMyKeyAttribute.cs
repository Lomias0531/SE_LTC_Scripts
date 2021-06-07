namespace VRage.Meta
{
	/// <summary>
	/// Interface used by the attribute indexer base.
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	public interface IMyKeyAttribute<TKey>
	{
		TKey Key
		{
			get;
		}
	}
}
