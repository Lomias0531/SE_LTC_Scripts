using System;

namespace VRageMath
{
	public static class HyperSphereHelpers
	{
		public static double DistanceToTangentProjected(ref Vector3D center, ref Vector3D point, double radius, out double distance)
		{
			Vector3D.Distance(ref point, ref center, out double result);
			double num = radius * radius;
			double num2 = result;
			double num3 = Math.Sqrt(num2 * num2 - num);
			double num4 = (num2 + radius + num3) / 2.0;
			double d = num4 * (num4 - num2) * (num4 - radius) * (num4 - num3);
			double num5 = 2.0 * Math.Sqrt(d) / num2;
			distance = num2 - Math.Sqrt(num - num5 * num5);
			return num5;
		}

		public static double DistanceToTangent(ref Vector3D center, ref Vector3D point, double radius)
		{
			Vector3D.Distance(ref point, ref center, out double result);
			return Math.Sqrt(result * result - radius * radius);
		}

		public static double DistanceToTangent(ref Vector2D center, ref Vector2D point, double radius)
		{
			Vector2D.Distance(ref point, ref center, out double result);
			return Math.Sqrt(result * result - radius * radius);
		}
	}
}
