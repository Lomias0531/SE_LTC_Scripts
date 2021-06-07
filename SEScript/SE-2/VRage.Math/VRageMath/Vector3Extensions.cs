namespace VRageMath
{
	/// <summary>
	/// Usefull Vector3 extensions
	/// </summary>
	public static class Vector3Extensions
	{
		/// <summary>
		/// Calculates projection vector
		/// </summary>
		/// <param name="sb">The sb.</param>
		/// <param name="length">The length.</param>
		public static Vector3 Project(this Vector3 projectedOntoVector, Vector3 projectedVector)
		{
			float num = projectedOntoVector.LengthSquared();
			if (num == 0f)
			{
				return Vector3.Zero;
			}
			return Vector3.Dot(projectedVector, projectedOntoVector) / num * projectedOntoVector;
		}
	}
}
