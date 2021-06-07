using System;
using System.Collections.Generic;
using VRageMath;

namespace Sandbox.Game.Entities
{
	internal static class MyCubeGridDeformationTables
	{
		public class DeformationTable
		{
			public readonly Dictionary<Vector3I, Matrix> OffsetTable = new Dictionary<Vector3I, Matrix>();

			public readonly HashSet<Vector3I> CubeOffsets = new HashSet<Vector3I>();

			public Vector3I Normal;

			public Vector3I MinOffset = Vector3I.MaxValue;

			public Vector3I MaxOffset = Vector3I.MinValue;
		}

		public static DeformationTable[] ThinUpper = new DeformationTable[3]
		{
			CreateTable(new Vector3I(1, 0, 0)),
			CreateTable(new Vector3I(0, 1, 0)),
			CreateTable(new Vector3I(0, 0, 1))
		};

		public static DeformationTable[] ThinLower = new DeformationTable[3]
		{
			CreateTable(new Vector3I(-1, 0, 0)),
			CreateTable(new Vector3I(0, -1, 0)),
			CreateTable(new Vector3I(0, 0, -1))
		};

		private static DeformationTable CreateTable(Vector3I normal)
		{
			DeformationTable deformationTable = new DeformationTable();
			deformationTable.Normal = normal;
			Vector3I a = new Vector3I(1, 1, 1);
			Vector3I b = Vector3I.Abs(normal);
			Vector3I vector3I = new Vector3I(1, 1, 1) - b;
			vector3I *= 2;
			for (int i = -vector3I.X; i <= vector3I.X; i++)
			{
				for (int j = -vector3I.Y; j <= vector3I.Y; j++)
				{
					for (int k = -vector3I.Z; k <= vector3I.Z; k++)
					{
						Vector3I value = new Vector3I(i, j, k);
						float num = Math.Max(Math.Abs(k), Math.Max(Math.Abs(i), Math.Abs(j)));
						float num2 = 1f;
						if (num > 1f)
						{
							num2 = 0.3f;
						}
						float num3 = num2 * 0.25f;
						Vector3I vector3I2 = a + new Vector3I(i, j, k) + normal;
						Matrix value2 = Matrix.CreateFromDir(-normal * num3);
						deformationTable.OffsetTable.Add(vector3I2, value2);
						Vector3I item = vector3I2 >> 1;
						Vector3I item2 = vector3I2 - Vector3I.One >> 1;
						deformationTable.CubeOffsets.Add(item);
						deformationTable.CubeOffsets.Add(item2);
						deformationTable.MinOffset = Vector3I.Min(deformationTable.MinOffset, value);
						deformationTable.MaxOffset = Vector3I.Max(deformationTable.MaxOffset, value);
					}
				}
			}
			return deformationTable;
		}
	}
}
