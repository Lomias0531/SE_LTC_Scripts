using VRage.Game.Voxels;
using VRage.Library.Collections;
using VRageMath;
using VRageMath.PackedVector;

namespace VRage.Voxels
{
	public sealed class MyIsoMesh
	{
		public readonly MyList<Vector3> Positions = new MyList<Vector3>();

		public readonly MyList<Vector3> Normals = new MyList<Vector3>();

		public readonly MyList<byte> Materials = new MyList<byte>();

		public readonly MyList<sbyte> Densities = new MyList<sbyte>();

		public readonly MyList<Vector3I> Cells = new MyList<Vector3I>();

		public readonly MyList<uint> ColorShiftHSV = new MyList<uint>();

		public readonly MyList<MyVoxelTriangle> Triangles = new MyList<MyVoxelTriangle>();

		public int Lod;

		public Vector3 PositionScale;

		public Vector3D PositionOffset;

		public Vector3I CellStart;

		public Vector3I CellEnd;

		public int VerticesCount => Positions.Count;

		public int TrianglesCount => Triangles.Count;

		public Vector3I Size => CellEnd - CellStart + 1;

		public static MyIsoMesh FromNative(VrVoxelMesh nativeMesh)
		{
			if (nativeMesh == null)
			{
				return null;
			}
			MyIsoMesh myIsoMesh = new MyIsoMesh();
			myIsoMesh.CopyFromNative(nativeMesh);
			return myIsoMesh;
		}

		public void Reserve(int vertexCount, int triangleCount)
		{
			if (Positions.Capacity < vertexCount)
			{
				Positions.Capacity = vertexCount;
				Normals.Capacity = vertexCount;
				Materials.Capacity = vertexCount;
				ColorShiftHSV.Capacity = vertexCount;
				Cells.Capacity = vertexCount;
				Densities.Capacity = vertexCount;
			}
			if (Triangles.Capacity < triangleCount)
			{
				Triangles.Capacity = triangleCount;
			}
		}

		public void Resize(int vertexCount, int triangleCount)
		{
			if (Positions.Capacity >= vertexCount)
			{
				Positions.SetSize(vertexCount);
				Normals.SetSize(vertexCount);
				Materials.SetSize(vertexCount);
				ColorShiftHSV.SetSize(vertexCount);
				Cells.SetSize(vertexCount);
				Densities.SetSize(vertexCount);
			}
			if (Triangles.Capacity >= triangleCount)
			{
				Triangles.SetSize(triangleCount);
			}
		}

		public void WriteTriangle(int v0, int v1, int v2)
		{
			Triangles.Add(new MyVoxelTriangle
			{
				V0 = (ushort)v0,
				V1 = (ushort)v1,
				V2 = (ushort)v2
			});
		}

		public int WriteVertex(ref Vector3I cell, ref Vector3 position, ref Vector3 normal, byte material, uint colorShift)
		{
			int count = Positions.Count;
			Positions.Add(position);
			Normals.Add(normal);
			Materials.Add(material);
			Cells.Add(cell);
			ColorShiftHSV.Add(colorShift);
			return count;
		}

		public void Clear()
		{
			Cells.Clear();
			Positions.Clear();
			Normals.Clear();
			Materials.Clear();
			Densities.Clear();
			ColorShiftHSV.Clear();
			Triangles.Clear();
		}

		public void GetUnpackedPosition(int idx, out Vector3 position)
		{
			position = Positions[idx] * PositionScale + PositionOffset;
		}

		public void GetUnpackedVertex(int idx, out MyVoxelVertex vertex)
		{
			vertex.Position = Positions[idx] * PositionScale + PositionOffset;
			vertex.Normal = Normals[idx];
			vertex.Material = Materials[idx];
			vertex.ColorShiftHSV = ColorShiftHSV[idx];
			vertex.Cell = Cells[idx];
		}

		public static bool IsEmpty(MyIsoMesh self)
		{
			if (self != null)
			{
				return self.Triangles.Count == 0;
			}
			return true;
		}

		public unsafe void CopyFromNative(VrVoxelMesh vrMesh)
		{
			Lod = vrMesh.Lod;
			CellStart = vrMesh.Start;
			CellEnd = vrMesh.End;
			PositionScale = new Vector3(1 << Lod);
			PositionOffset = CellStart * PositionScale;
			int vertexCount = vrMesh.VertexCount;
			int triangleCount = vrMesh.TriangleCount;
			Reserve(vertexCount, triangleCount);
			fixed (Vector3* positions = Positions.GetInternalArray())
			{
				fixed (Vector3* normals = Normals.GetInternalArray())
				{
					fixed (byte* materials = Materials.GetInternalArray())
					{
						fixed (Vector3I* cells = Cells.GetInternalArray())
						{
							fixed (MyVoxelTriangle* triangles = Triangles.GetInternalArray())
							{
								fixed (uint* color = ColorShiftHSV.GetInternalArray())
								{
									vrMesh.GetMeshData(positions, normals, materials, cells, (Byte4*)color, (VrVoxelTriangle*)triangles);
								}
							}
						}
					}
				}
			}
			Positions.SetSize(vertexCount);
			Normals.SetSize(vertexCount);
			Materials.SetSize(vertexCount);
			Cells.SetSize(vertexCount);
			ColorShiftHSV.SetSize(vertexCount);
			Triangles.SetSize(triangleCount);
		}

		public bool IsEdge(ref Vector3I cell)
		{
			Vector3I vector3I = CellEnd - CellStart - 1;
			if (cell.X != 0 && cell.X != vector3I.X && cell.Y != 0 && cell.Y != vector3I.Y && cell.Z != 0)
			{
				return cell.Z == vector3I.Z;
			}
			return true;
		}
	}
}
