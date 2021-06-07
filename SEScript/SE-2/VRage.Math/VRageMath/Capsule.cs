using System;

namespace VRageMath
{
	public struct Capsule
	{
		public Vector3 P0;

		public Vector3 P1;

		public float Radius;

		public Capsule(Vector3 p0, Vector3 p1, float radius)
		{
			P0 = p0;
			P1 = p1;
			Radius = radius;
		}

		public bool Intersect(Ray ray, ref Vector3 p1, ref Vector3 p2, ref Vector3 n1, ref Vector3 n2)
		{
			Vector3 vector = P1 - P0;
			Vector3 vector2 = ray.Position - P0;
			float num = vector.Dot(ray.Direction);
			float num2 = vector.Dot(vector2);
			float num3 = vector.Dot(vector);
			float num4 = (num3 > 0f) ? (num / num3) : 0f;
			float num5 = (num3 > 0f) ? (num2 / num3) : 0f;
			Vector3 v = ray.Direction - vector * num4;
			Vector3 v2 = vector2 - vector * num5;
			float num6 = v.Dot(v);
			float num7 = 2f * v.Dot(v2);
			float num8 = v2.Dot(v2) - Radius * Radius;
			if (num6 == 0f)
			{
				BoundingSphere boundingSphere = default(BoundingSphere);
				boundingSphere.Center = P0;
				boundingSphere.Radius = Radius;
				BoundingSphere boundingSphere2 = default(BoundingSphere);
				boundingSphere2.Center = P1;
				boundingSphere2.Radius = Radius;
				if (!boundingSphere.IntersectRaySphere(ray, out float tmin, out float tmax) || !boundingSphere2.IntersectRaySphere(ray, out float tmin2, out float tmax2))
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
			float num9 = num7 * num7 - 4f * num6 * num8;
			if (num9 < 0f)
			{
				return false;
			}
			float num10 = (0f - num7 - (float)Math.Sqrt(num9)) / (2f * num6);
			float num11 = (0f - num7 + (float)Math.Sqrt(num9)) / (2f * num6);
			if (num10 > num11)
			{
				float num12 = num10;
				num10 = num11;
				num11 = num12;
			}
			float num13 = num10 * num4 + num5;
			if (num13 < 0f)
			{
				BoundingSphere boundingSphere3 = default(BoundingSphere);
				boundingSphere3.Center = P0;
				boundingSphere3.Radius = Radius;
				if (!boundingSphere3.IntersectRaySphere(ray, out float tmin3, out float _))
				{
					return false;
				}
				p1 = ray.Position + ray.Direction * tmin3;
				n1 = p1 - P0;
				n1.Normalize();
			}
			else if (num13 > 1f)
			{
				BoundingSphere boundingSphere4 = default(BoundingSphere);
				boundingSphere4.Center = P1;
				boundingSphere4.Radius = Radius;
				if (!boundingSphere4.IntersectRaySphere(ray, out float tmin4, out float _))
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
				Vector3 value = P0 + vector * num13;
				n1 = p1 - value;
				n1.Normalize();
			}
			float num14 = num11 * num4 + num5;
			if (num14 < 0f)
			{
				BoundingSphere boundingSphere5 = default(BoundingSphere);
				boundingSphere5.Center = P0;
				boundingSphere5.Radius = Radius;
				if (!boundingSphere5.IntersectRaySphere(ray, out float _, out float tmax5))
				{
					return false;
				}
				p2 = ray.Position + ray.Direction * tmax5;
				n2 = p2 - P0;
				n2.Normalize();
			}
			else if (num14 > 1f)
			{
				BoundingSphere boundingSphere6 = default(BoundingSphere);
				boundingSphere6.Center = P1;
				boundingSphere6.Radius = Radius;
				if (!boundingSphere6.IntersectRaySphere(ray, out float _, out float tmax6))
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
				Vector3 value2 = P0 + vector * num14;
				n2 = p2 - value2;
				n2.Normalize();
			}
			return true;
		}

		public bool Intersect(Line line, ref Vector3 p1, ref Vector3 p2, ref Vector3 n1, ref Vector3 n2)
		{
			Ray ray = new Ray(line.From, line.Direction);
			if (Intersect(ray, ref p1, ref p2, ref n1, ref n2))
			{
				Vector3 vector = p1 - line.From;
				Vector3 vector2 = p2 - line.From;
				float num = vector.Normalize();
				vector2.Normalize();
				if (Vector3.Dot(line.Direction, vector) < 0.9f)
				{
					return false;
				}
				if (Vector3.Dot(line.Direction, vector2) < 0.9f)
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
