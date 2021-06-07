namespace VRage.Compiler
{
	/// <summary>
	/// Type name which does not make allocations
	/// </summary>
	public static class TypeNameHelper<T>
	{
		public static readonly string Name;

		static TypeNameHelper()
		{
			Name = typeof(T).Name;
		}
	}
}
