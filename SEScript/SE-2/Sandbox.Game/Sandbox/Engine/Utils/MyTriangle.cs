using VRage.Utils;
using VRageMath;

namespace Sandbox.Engine.Utils
{
	public struct MyTriangle
	{
		private Vector3 origin;

		private Vector3 edge0;

		private Vector3 edge1;

		public Vector3 Centre => origin + 0.333333343f * (edge0 + edge1);

		public Vector3 Origin
		{
			get
			{
				return origin;
			}
			set
			{
				origin = value;
			}
		}

		public Vector3 Edge0
		{
			get
			{
				return edge0;
			}
			set
			{
				edge0 = value;
			}
		}

		public Vector3 Edge1
		{
			get
			{
				return edge1;
			}
			set
			{
				edge1 = value;
			}
		}

		public Vector3 Edge2 => edge1 - edge0;

		public Plane Plane => new Plane(GetPoint(0), GetPoint(1), GetPoint(2));

		public Vector3 Normal => Vector3.Normalize(Vector3.Cross(MyUtils.Normalize(edge0), MyUtils.Normalize(edge1)));

		public MyTriangle(Vector3 pt0, Vector3 pt1, Vector3 pt2)
		{
			origin = pt0;
			edge0 = pt1 - pt0;
			edge1 = pt2 - pt0;
		}

		public MyTriangle(ref Vector3 pt0, ref Vector3 pt1, ref Vector3 pt2)
		{
			origin = pt0;
			edge0 = pt1 - pt0;
			edge1 = pt2 - pt0;
		}

		public Vector3 GetPoint(int i)
		{
			switch (i)
			{
			case 1:
				return origin + edge0;
			case 2:
				return origin + edge1;
			default:
				return origin;
			}
		}

		public void GetPoint(int i, out Vector3 point)
		{
			switch (i)
			{
			case 1:
				point = origin + edge0;
				break;
			case 2:
				point = origin + edge1;
				break;
			default:
				point = origin;
				break;
			}
		}

		public void GetPoint(ref Vector3 point, int i)
		{
			switch (i)
			{
			case 1:
				point.X = origin.X + edge0.X;
				point.Y = origin.Y + edge0.Y;
				point.Z = origin.Z + edge0.Z;
				break;
			case 2:
				point.X = origin.X + edge1.X;
				point.Y = origin.Y + edge1.Y;
				point.Z = origin.Z + edge1.Z;
				break;
			default:
				point.X = origin.X;
				point.Y = origin.Y;
				point.Z = origin.Z;
				break;
			}
		}

		public Vector3 GetPoint(float t0, float t1)
		{
			return origin + t0 * edge0 + t1 * edge1;
		}

		public void GetSpan(out float min, out float max, Vector3 axis)
		{
			float a = Vector3.Dot(GetPoint(0), axis);
			float b = Vector3.Dot(GetPoint(1), axis);
			float c = Vector3.Dot(GetPoint(2), axis);
			min = MathHelper.Min(a, b, c);
			max = MathHelper.Max(a, b, c);
		}

		public void GetSpan(out float min, out float max, ref Vector3 axis)
		{
			Vector3 point = default(Vector3);
			GetPoint(ref point, 0);
			float a = point.X * axis.X + point.Y * axis.Y + point.Z * axis.Z;
			GetPoint(ref point, 1);
			float b = point.X * axis.X + point.Y * axis.Y + point.Z * axis.Z;
			GetPoint(ref point, 2);
			float c = point.X * axis.X + point.Y * axis.Y + point.Z * axis.Z;
			min = MathHelper.Min(a, b, c);
			max = MathHelper.Max(a, b, c);
		}
	}
}
