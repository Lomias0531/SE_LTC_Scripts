using System;

namespace VRageMath
{
	public struct CapsuleD
	{
		public Vector3D P0;

		public Vector3D P1;

		public float Radius;

		public CapsuleD(Vector3D p0, Vector3D p1, float radius)
		{
			P0 = p0;
			P1 = p1;
			Radius = radius;
		}

		public bool Intersect(RayD ray, ref Vector3D p1, ref Vector3D p2, ref Vector3 n1, ref Vector3 n2)
		{
			Vector3D vector3D = P1 - P0;
			Vector3D vector3D2 = ray.Position - P0;
			double num = vector3D.Dot(ray.Direction);
			double num2 = vector3D.Dot(vector3D2);
			double num3 = vector3D.Dot(vector3D);
			double num4 = (num3 > 0.0) ? (num / num3) : 0.0;
			double num5 = (num3 > 0.0) ? (num2 / num3) : 0.0;
			Vector3D v = ray.Direction - vector3D * num4;
			Vector3D v2 = vector3D2 - vector3D * num5;
			double num6 = v.Dot(v);
			double num7 = 2.0 * v.Dot(v2);
			double num8 = v2.Dot(v2) - (double)(Radius * Radius);
			if (num6 == 0.0)
			{
				BoundingSphereD boundingSphereD = default(BoundingSphereD);
				boundingSphereD.Center = P0;
				boundingSphereD.Radius = Radius;
				BoundingSphereD boundingSphereD2 = default(BoundingSphereD);
				boundingSphereD2.Center = P1;
				boundingSphereD2.Radius = Radius;
				if (!boundingSphereD.IntersectRaySphere(ray, out double tmin, out double tmax) || !boundingSphereD2.IntersectRaySphere(ray, out double tmin2, out double tmax2))
				{
					return false;
				}
				if (tmin < tmin2)
				{
					p1 = ray.Position + ray.Direction * tmin;
					n1 = p1 - P0;
					n1.Normalize();
				}
				else
				{
					p1 = ray.Position + ray.Direction * tmin2;
					n1 = p1 - P1;
					n1.Normalize();
				}
				if (tmax > tmax2)
				{
					p2 = ray.Position + ray.Direction * tmax;
					n2 = p2 - P0;
					n2.Normalize();
				}
				else
				{
					p2 = ray.Position + ray.Direction * tmax2;
					n2 = p2 - P1;
					n2.Normalize();
				}
				return true;
			}
			double num9 = num7 * num7 - 4.0 * num6 * num8;
			if (num9 < 0.0)
			{
				return false;
			}
			double num10 = (0.0 - num7 - Math.Sqrt(num9)) / (2.0 * num6);
			double num11 = (0.0 - num7 + Math.Sqrt(num9)) / (2.0 * num6);
			if (num10 > num11)
			{
				double num12 = num10;
				num10 = num11;
				num11 = num12;
			}
			double num13 = num10 * num4 + num5;
			if (num13 < 0.0)
			{
				BoundingSphereD boundingSphereD3 = default(BoundingSphereD);
				boundingSphereD3.Center = P0;
				boundingSphereD3.Radius = Radius;
				if (!boundingSphereD3.IntersectRaySphere(ray, out double tmin3, out double _))
				{
					return false;
				}
				p1 = ray.Position + ray.Direction * tmin3;
				n1 = p1 - P0;
				n1.Normalize();
			}
			else if (num13 > 1.0)
			{
				BoundingSphereD boundingSphereD4 = default(BoundingSphereD);
				boundingSphereD4.Center = P1;
				boundingSphereD4.Radius = Radius;
				if (!boundingSphereD4.IntersectRaySphere(ray, out double tmin4, out double _))
				{
					return false;
				}
				p1 = ray.Position + ray.Direction * tmin4;
				n1 = p1 - P1;
				n1.Normalize();
			}
			else
			{
				p1 = ray.Position + ray.Direction * num10;
				Vector3D value = P0 + vector3D * num13;
				n1 = p1 - value;
				n1.Normalize();
			}
			double num14 = num11 * num4 + num5;
			if (num14 < 0.0)
			{
				BoundingSphereD boundingSphereD5 = default(BoundingSphereD);
				boundingSphereD5.Center = P0;
				boundingSphereD5.Radius = Radius;
				if (!boundingSphereD5.IntersectRaySphere(ray, out double _, out double tmax5))
				{
					return false;
				}
				p2 = ray.Position + ray.Direction * tmax5;
				n2 = p2 - P0;
				n2.Normalize();
			}
			else if (num14 > 1.0)
			{
				BoundingSphereD boundingSphereD6 = default(BoundingSphereD);
				boundingSphereD6.Center = P1;
				boundingSphereD6.Radius = Radius;
				if (!boundingSphereD6.IntersectRaySphere(ray, out double _, out double tmax6))
				{
					return false;
				}
				p2 = ray.Position + ray.Direction * tmax6;
				n2 = p2 - P1;
				n2.Normalize();
			}
			else
			{
				p2 = ray.Position + ray.Direction * num11;
				Vector3D value2 = P0 + vector3D * num14;
				n2 = p2 - value2;
				n2.Normalize();
			}
			return true;
		}

		public bool Intersect(LineD line, ref Vector3D p1, ref Vector3D p2, ref Vector3 n1, ref Vector3 n2)
		{
			RayD ray = new RayD(line.From, line.Direction);
			if (Intersect(ray, ref p1, ref p2, ref n1, ref n2))
			{
				Vector3D vector = p1 - line.From;
				Vector3D vector2 = p2 - line.From;
				double num = vector.Normalize();
				vector2.Normalize();
				if (Vector3D.Dot(line.Direction, vector) < 0.9)
				{
					return false;
				}
				if (Vector3D.Dot(line.Direction, vector2) < 0.9)
				{
					return false;
				}
				if (line.Length < num)
				{
					return false;
				}
				return true;
			}
			return false;
		}
	}
}
