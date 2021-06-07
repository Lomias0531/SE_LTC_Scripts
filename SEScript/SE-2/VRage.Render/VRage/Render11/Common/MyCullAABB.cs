using System;
using VRageMath;

namespace VRage.Render11.Common
{
	public struct MyCullAABB
	{
		[Flags]
		public enum Axes
		{
			X = 0x4,
			Y = 0x2,
			Z = 0x1
		}

		private const int COUNT = 16;

		public const int MAX_OFFSET = 16;

		public readonly Vector3[] Data;

		public float LengthSquared
		{
			get
			{
				return Data[32].X;
			}
			set
			{
				Data[32].X = value;
			}
		}

		public MyCullAABB(BoundingBox aabb)
		{
			Data = new Vector3[33];
			Reset(ref aabb);
		}

		public void Reset(ref BoundingBox aabb)
		{
			Vector3 min = aabb.Min;
			Vector3 max = aabb.Max;
			for (int i = 0; i < 16; i++)
			{
				Axes axes = (Axes)i;
				Data[i] = new Vector3(((axes & Axes.X) > (Axes)0) ? min.X : max.X, ((axes & Axes.Y) > (Axes)0) ? min.Y : max.Y, ((axes & Axes.Z) > (Axes)0) ? min.Z : max.Z);
				Data[i + 16] = new Vector3(((axes & Axes.X) == 0) ? min.X : max.X, ((axes & Axes.Y) == 0) ? min.Y : max.Y, ((axes & Axes.Z) == 0) ? min.Z : max.Z);
			}
			LengthSquared = aabb.Size.LengthSquared();
		}
	}
}
