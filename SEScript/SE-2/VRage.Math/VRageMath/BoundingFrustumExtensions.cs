using System;

namespace VRageMath
{
	public static class BoundingFrustumExtensions
	{
		/// <summary>
		/// Creates bounding sphere from bounding frustum.
		/// Implementation taken from XNA source, replace IEnumerable with array
		/// </summary>
		/// <param name="frustum">The bounding frustum.</param>
		/// <param name="corners">Temporary memory to save corner when getting from frustum.</param>
		/// <returns>BoundingSphere</returns>
		public static BoundingSphere ToBoundingSphere(this BoundingFrustum frustum, Vector3[] corners)
		{
			if (corners.Length < 8)
			{
				throw new ArgumentException("Corners length must be at least 8");
			}
			frustum.GetCorners(corners);
			Vector3 value5;
			Vector3 value4;
			Vector3 value3;
			Vector3 value2;
			Vector3 value;
			Vector3 value6 = value5 = (value4 = (value3 = (value2 = (value = corners[0]))));
			for (int i = 0; i < corners.Length; i++)
			{
				Vector3 vector = corners[i];
				if (vector.X < value6.X)
				{
					value6 = vector;
				}
				if (vector.X > value5.X)
				{
					value5 = vector;
				}
				if (vector.Y < value4.Y)
				{
					value4 = vector;
				}
				if (vector.Y > value3.Y)
				{
					value3 = vector;
				}
				if (vector.Z < value2.Z)
				{
					value2 = vector;
				}
				if (vector.Z > value.Z)
				{
					value = vector;
				}
			}
			Vector3.Distance(ref value5, ref value6, out float result);
			Vector3.Distance(ref value3, ref value4, out float result2);
			Vector3.Distance(ref value, ref value2, out float result3);
			Vector3 result4;
			float num;
			if (result > result2)
			{
				if (result > result3)
				{
					Vector3.Lerp(ref value5, ref value6, 0.5f, out result4);
					num = result * 0.5f;
				}
				else
				{
					Vector3.Lerp(ref value, ref value2, 0.5f, out result4);
					num = result3 * 0.5f;
				}
			}
			else if (result2 > result3)
			{
				Vector3.Lerp(ref value3, ref value4, 0.5f, out result4);
				num = result2 * 0.5f;
			}
			else
			{
				Vector3.Lerp(ref value, ref value2, 0.5f, out result4);
				num = result3 * 0.5f;
			}
			Vector3 value7 = default(Vector3);
			for (int j = 0; j < corners.Length; j++)
			{
				Vector3 vector2 = corners[j];
				value7.X = vector2.X - result4.X;
				value7.Y = vector2.Y - result4.Y;
				value7.Z = vector2.Z - result4.Z;
				float num2 = value7.Length();
				if (num2 > num)
				{
					num = (num + num2) * 0.5f;
					result4 += (1f - num / num2) * value7;
				}
			}
			BoundingSphere result5 = default(BoundingSphere);
			result5.Center = result4;
			result5.Radius = num;
			return result5;
		}
	}
}
