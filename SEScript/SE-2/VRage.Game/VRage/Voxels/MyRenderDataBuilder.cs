using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Library.Collections;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageMath.PackedVector;
using VRageRender.Voxels;

namespace VRage.Voxels
{
	public class MyRenderDataBuilder
	{
		/// <summary>
		/// Represents a material triple.
		/// </summary>
		private struct MaterialTriple
		{
			public readonly byte M0;

			public readonly byte M1;

			public readonly byte M2;

			public bool SingleMaterial => M1 == byte.MaxValue;

			public bool MultiMaterial => M1 != byte.MaxValue;

			public MaterialTriple(ref VrVoxelVertex v0, ref VrVoxelVertex v1, ref VrVoxelVertex v2)
			{
				M0 = v0.Material;
				M1 = v1.Material;
				M2 = v2.Material;
				if (M0 == M1)
				{
					M1 = byte.MaxValue;
				}
				if (M0 == M2)
				{
					M2 = byte.MaxValue;
				}
				if (M1 == M2)
				{
					M2 = byte.MaxValue;
				}
				if (M0 > M1)
				{
					MyUtils.Swap(ref M0, ref M1);
				}
				if (M1 > M2)
				{
					MyUtils.Swap(ref M1, ref M2);
				}
				if (M0 > M1)
				{
					MyUtils.Swap(ref M0, ref M1);
				}
			}

			public MaterialTriple(byte m0, byte m1, byte m2)
			{
				M0 = m0;
				M1 = m1;
				M2 = m2;
			}

			public static implicit operator int(MaterialTriple triple)
			{
				return -(triple.M0 | (triple.M1 << 8) | (triple.M2 << 16));
			}

			public static implicit operator MaterialTriple(int packed)
			{
				packed = -packed;
				return new MaterialTriple((byte)(packed & 0xFF), (byte)((packed >> 8) & 0xFF), (byte)((packed >> 16) & 0xFF));
			}

			public static implicit operator MyVoxelMaterialTriple(MaterialTriple triple)
			{
				return new MyVoxelMaterialTriple(triple.M0, triple.M1, triple.M2);
			}

			public override string ToString()
			{
				if (SingleMaterial)
				{
					return $"S{{{M0}}}";
				}
				return $"M{{{M0}, {M1}, {M2}}}";
			}
		}

		/// <summary>
		/// Container for vertices and triangles belonging to a given part.
		/// </summary>
		[GenerateActivator]
		private class Part
		{
			private class VRage_Voxels_MyRenderDataBuilder_003C_003EPart_003C_003EActor : IActivator, IActivator<Part>
			{
				private sealed override object CreateInstance()
				{
					return new Part();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override Part CreateInstance()
				{
					return new Part();
				}

				Part IActivator<Part>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			public MaterialTriple Material;

			private readonly Dictionary<ushort, ushort> m_indexMap = new Dictionary<ushort, ushort>();

			public readonly MyList<MyVertexFormatVoxelSingleData> Vertices = new MyList<MyVertexFormatVoxelSingleData>();

			public readonly MyList<VrVoxelTriangle> Triangles = new MyList<VrVoxelTriangle>();

			public void Init(MaterialTriple material)
			{
				Material = material;
			}

			public void Clear()
			{
				m_indexMap.Clear();
				Vertices.Clear();
				Triangles.Clear();
			}

			public unsafe void AddTriangle(VrVoxelTriangle triangle, VrVoxelVertex* vertices)
			{
				RemapVertex(ref triangle.V0, vertices);
				RemapVertex(ref triangle.V1, vertices);
				RemapVertex(ref triangle.V2, vertices);
				Triangles.Add(triangle);
			}

			private unsafe void RemapVertex(ref ushort vertex, VrVoxelVertex* vertices)
			{
				if (m_indexMap.TryGetValue(vertex, out ushort value))
				{
					vertex = value;
					return;
				}
				int count = Vertices.Count;
				MyVertexFormatVoxelSingleData item = default(MyVertexFormatVoxelSingleData);
				item.Position = vertices[(int)vertex].Position;
				item.Normal = vertices[(int)vertex].Normal;
				item.PackedColorShift = vertices[(int)vertex].Color.PackedValue;
				item.Material = new Byte4((int)Material.M0, (int)Material.M1, (int)Material.M2, GetMaterialIndex(vertices[(int)vertex].Material));
				Vertices.Add(item);
				m_indexMap[vertex] = (ushort)count;
				vertex = (ushort)count;
			}

			private int GetMaterialIndex(byte material)
			{
				if (material == Material.M0)
				{
					return 0;
				}
				if (material == Material.M1)
				{
					return 1;
				}
				if (material == Material.M2)
				{
					return 2;
				}
				return -1;
			}
		}

		[ThreadStatic]
		private static MyRenderDataBuilder m_instance;

		/// <summary>
		/// Shared part pool.
		/// </summary>
		private static readonly MyConcurrentPool<Part> m_partPool = new MyConcurrentPool<Part>();

		/// <summary>
		/// Parts for the currently processed mesh.
		/// </summary>
		private readonly SortedDictionary<int, Part> m_parts = new SortedDictionary<int, Part>();

		/// <summary>
		/// Current thread's static instance.
		/// </summary>
		public static MyRenderDataBuilder Instance => m_instance ?? (m_instance = new MyRenderDataBuilder());

		/// <summary>
		/// Build render cell data from a native mesh.
		/// </summary>
		/// <param name="mesh"></param>
		/// <param name="data"></param>
		public unsafe void Build(VrVoxelMesh mesh, out MyVoxelRenderCellData data, IMyVoxelRenderDataProcessorProvider dataProcessorProvider)
		{
			data = default(MyVoxelRenderCellData);
			if (mesh.TriangleCount == 0)
			{
				return;
			}
			VrVoxelVertex* vertices = mesh.Vertices;
			VrVoxelTriangle* triangles = mesh.Triangles;
			data.CellBounds = BoundingBox.CreateInvalid();
			int vertexCount = mesh.VertexCount;
			for (int i = 0; i < vertexCount; i++)
			{
				data.CellBounds.Include(vertices[i].Position);
			}
			int triangleCount = mesh.TriangleCount;
			for (int j = 0; j < triangleCount; j++)
			{
				VrVoxelTriangle triangle = triangles[j];
				MaterialTriple materialTriple = new MaterialTriple(ref vertices[(int)triangle.V0], ref vertices[(int)triangle.V1], ref vertices[(int)triangle.V2]);
				if (!m_parts.TryGetValue(materialTriple, out Part value))
				{
					value = m_partPool.Get();
					value.Init(materialTriple);
					m_parts[materialTriple] = value;
				}
				value.AddTriangle(triangle, vertices);
			}
			vertexCount = 0;
			triangleCount = 0;
			foreach (Part value2 in m_parts.Values)
			{
				vertexCount += value2.Vertices.Count;
				triangleCount += value2.Triangles.Count;
			}
			IMyVoxelRenderDataProcessor renderDataProcessor = dataProcessorProvider.GetRenderDataProcessor(vertexCount, triangleCount * 3, m_parts.Count);
			foreach (Part value3 in m_parts.Values)
			{
				try
				{
					fixed (VrVoxelTriangle* ptr = value3.Triangles.GetInternalArray())
					{
						ushort* indices = (ushort*)ptr;
						int indicesCount = value3.Triangles.Count * 3;
						renderDataProcessor.AddPart(value3.Vertices, indices, indicesCount, value3.Material);
					}
				}
				finally
				{
				}
				value3.Clear();
				m_partPool.Return(value3);
			}
			data.VertexCount = vertexCount;
			data.IndexCount = triangleCount * 3;
			renderDataProcessor.GetDataAndDispose(ref data);
			m_parts.Clear();
		}
	}
}
