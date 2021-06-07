using System;
using VRageMath;

namespace VRage.Sync
{
	public static class SyncExtensions
	{
		/// <summary>
		/// Sets validation handler to always return false.
		/// </summary>
		public static void AlwaysReject<T, TSyncDirection>(this Sync<T, TSyncDirection> sync) where TSyncDirection : SyncDirection
		{
			sync.Validate = ((T value) => false);
		}

		/// <summary>
		/// Sets validate handler to validate that value is in range.
		/// </summary>
		public static void ValidateRange<TSyncDirection>(this Sync<float, TSyncDirection> sync, float inclusiveMin, float inclusiveMax) where TSyncDirection : SyncDirection
		{
			sync.Validate = ((float value) => value >= inclusiveMin && value <= inclusiveMax);
		}

		/// <summary>
		/// Sets validate handler to validate that value is in range.
		/// </summary>
		public static void ValidateRange<TSyncDirection>(this Sync<float, TSyncDirection> sync, Func<float> inclusiveMin, Func<float> inclusiveMax) where TSyncDirection : SyncDirection
		{
			sync.Validate = ((float value) => value >= inclusiveMin() && value <= inclusiveMax());
		}

		/// <summary>
		/// Sets validate handler to validate that value is withing bounds.
		/// </summary>
		public static void ValidateRange<TSyncDirection>(this Sync<float, TSyncDirection> sync, Func<MyBounds> bounds) where TSyncDirection : SyncDirection
		{
			sync.Validate = delegate(float value)
			{
				MyBounds myBounds = bounds();
				return value >= myBounds.Min && value <= myBounds.Max;
			};
		}
	}
}
