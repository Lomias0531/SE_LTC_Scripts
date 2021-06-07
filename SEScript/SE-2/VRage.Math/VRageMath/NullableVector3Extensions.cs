using System.Diagnostics;

namespace VRageMath
{
	public static class NullableVector3Extensions
	{
		public static bool IsValid(this Vector3? value)
		{
			if (value.HasValue)
			{
				return value.Value.IsValid();
			}
			return true;
		}

		[Conditional("DEBUG")]
		public static void AssertIsValid(this Vector3? value)
		{
		}
	}
}
