namespace VRage.Profiler
{
	/// <summary>
	/// Profiler sorting order types
	/// </summary>
	public enum RenderProfilerSortingOrder
	{
		/// <summary>
		/// Order in which the elements are logged
		/// </summary>
		Id,
		/// <summary>
		/// Milliseconds spent in the previous frame, sorted from slowest to fastest
		/// </summary>
		MillisecondsLastFrame,
		/// <summary>
		/// Allocated amount of memory in the previous frame, sorted from highest to lowest
		/// </summary>
		AllocatedLastFrame,
		/// <summary>
		/// Milliseconds spent on average, sorted from slowest to fastest
		/// </summary>
		MillisecondsAverage,
		/// <summary>
		/// Total number of sorting types
		/// </summary>
		NumSortingTypes
	}
}
