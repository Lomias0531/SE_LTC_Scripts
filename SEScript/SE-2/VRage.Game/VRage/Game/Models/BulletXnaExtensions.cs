using BulletXNA.LinearMath;
using VRageMath;

namespace VRage.Game.Models
{
	public static class BulletXnaExtensions
	{
		public static IndexedVector3 ToBullet(this Vector3 v)
		{
			return new IndexedVector3(v.X, v.Y, v.Z);
		}

		public static IndexedVector3 ToBullet(this Vector3D v)
		{
			return new IndexedVector3((float)v.X, (float)v.Y, (float)v.Z);
		}
	}
}
