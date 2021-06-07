using System;
using System.Collections.Generic;
using VRageMath;

namespace VRage.Game
{
	public class MyMeshHelper
	{
		private static readonly int C_BUFFER_CAPACITY = 5000;

		private static List<Vector3D> m_tmpVectorBuffer = new List<Vector3D>(C_BUFFER_CAPACITY);

		/// <summary>
		/// GenerateSphere
		/// </summary>
		/// <param name="vctPos"></param>
		/// <param name="radius"></param>
		/// <param name="steps"></param>
		public static void GenerateSphere(ref MatrixD worldMatrix, float radius, int steps, List<Vector3D> vertices)
		{
			m_tmpVectorBuffer.Clear();
			int num = 0;
			float num2 = 360 / steps;
			float num3 = 90f - num2;
			float num4 = 360f - num2;
			Vector3D item = default(Vector3D);
			for (float num5 = 0f; num5 <= num3; num5 += num2)
			{
				for (float num6 = 0f; num6 <= num4; num6 += num2)
				{
					item.X = (float)((double)radius * Math.Sin(MathHelper.ToRadians(num6)) * Math.Sin(MathHelper.ToRadians(num5)));
					item.Y = (float)((double)radius * Math.Cos(MathHelper.ToRadians(num6)) * Math.Sin(MathHelper.ToRadians(num5)));
					item.Z = (float)((double)radius * Math.Cos(MathHelper.ToRadians(num5)));
					m_tmpVectorBuffer.Add(item);
					num++;
					item.X = (float)((double)radius * Math.Sin(MathHelper.ToRadians(num6)) * Math.Sin(MathHelper.ToRadians(num5 + num2)));
					item.Y = (float)((double)radius * Math.Cos(MathHelper.ToRadians(num6)) * Math.Sin(MathHelper.ToRadians(num5 + num2)));
					item.Z = (float)((double)radius * Math.Cos(MathHelper.ToRadians(num5 + num2)));
					m_tmpVectorBuffer.Add(item);
					num++;
					item.X = (float)((double)radius * Math.Sin(MathHelper.ToRadians(num6 + num2)) * Math.Sin(MathHelper.ToRadians(num5)));
					item.Y = (float)((double)radius * Math.Cos(MathHelper.ToRadians(num6 + num2)) * Math.Sin(MathHelper.ToRadians(num5)));
					item.Z = (float)((double)radius * Math.Cos(MathHelper.ToRadians(num5)));
					m_tmpVectorBuffer.Add(item);
					num++;
					item.X = (float)((double)radius * Math.Sin(MathHelper.ToRadians(num6 + num2)) * Math.Sin(MathHelper.ToRadians(num5 + num2)));
					item.Y = (float)((double)radius * Math.Cos(MathHelper.ToRadians(num6 + num2)) * Math.Sin(MathHelper.ToRadians(num5 + num2)));
					item.Z = (float)((double)radius * Math.Cos(MathHelper.ToRadians(num5 + num2)));
					m_tmpVectorBuffer.Add(item);
					num++;
				}
			}
			_ = m_tmpVectorBuffer.Count;
			foreach (Vector3D item3 in m_tmpVectorBuffer)
			{
				vertices.Add(item3);
			}
			foreach (Vector3D item4 in m_tmpVectorBuffer)
			{
				Vector3D item2 = new Vector3D(item4.X, item4.Y, 0.0 - item4.Z);
				vertices.Add(item2);
			}
			for (int i = 0; i < vertices.Count; i++)
			{
				vertices[i] = Vector3D.Transform(vertices[i], worldMatrix);
			}
		}
	}
}
