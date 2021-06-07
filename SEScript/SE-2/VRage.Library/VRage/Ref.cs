using System.Runtime.CompilerServices;

namespace VRage
{
	/// <summary>
	/// Good for struct wrapping to store it as ref value in dictionary.
	/// </summary>
	public class Ref<T>
	{
		public T Value;
	}
	public static class Ref
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ref<T> Create<T>(T value) where T : struct
		{
			return new Ref<T>
			{
				Value = value
			};
		}
	}
}
