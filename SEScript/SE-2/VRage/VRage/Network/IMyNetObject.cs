namespace VRage.Network
{
	/// <summary>
	/// Base interface for networked object.
	/// Derived interfaces are so far IMyReplicable and IMyStateGroup.
	/// </summary>
	public interface IMyNetObject : IMyEventOwner
	{
		/// <summary>
		/// Is this still valid, i.e it should still be used and replicated.
		/// </summary>
		bool IsValid
		{
			get;
		}
	}
}
