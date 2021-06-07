using System;
using VRageMath;
using VRageMath.PackedVector;

namespace VRage.Import
{
	public static class PositionPacker
	{
		public static HalfVector4 PackPosition(ref Vector3 position)
		{
			float num = Math.Min((float)Math.Floor(Math.Max(Math.Max(Math.Abs(position.X), Math.Abs(position.Y)), Math.Abs(position.Z))), 2048f);
			float num2 = 0f;
			if (num > 0f)
			{
				num2 = 1f / num;
			}
			else
			{
				num = (num2 = 1f);
			}
			return new HalfVector4(num2 * position.X, num2 * position.Y, num2 * position.Z, num);
		}

		public static Vector3 UnpackPosition(ref HalfVector4 position)
		{
			Vector4 vector = position.ToVector4();
			return vector.W * new Vector3(vector.X, vector.Y, vector.Z);
		}
	}
}
