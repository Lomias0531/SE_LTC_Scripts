namespace VRage.GameServices
{
	/// <summary>
	/// Represents a type of item list to be queried.
	/// </summary>
	public enum WorkshopListType
	{
		/// <summary>
		/// Default list type, used of item content the user wants to have.
		/// </summary>
		Subscribed,
		/// <summary>
		/// Items the user wants to remember about.
		/// </summary>
		Favourited,
		/// <summary>
		/// All items.
		/// </summary>
		None
	}
}
