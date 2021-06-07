using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Entities.Components;
using VRage.Game.Voxels;
using VRage.Native;
using VRageMath;

namespace VRage.Voxels.DualContouring
{
	public class MyDualContouringMesher : IMyIsoMesher
	{
		[ThreadStatic]
		private static MyDualContouringMesher m_threadInstance;

		/// <summary>
		/// Whether to postprocess generated meshes.
		///
		/// Can be set to false for debugging and testing purposes.
		/// </summary>
		public static bool Postprocess = true;

		private const int AFFECTED_RANGE_OFFSET = -1;

		private const int AFFECTED_RANGE_SIZE_CHANGE = 5;

		private MyStorageData m_storageData = new MyStorageData();

		private List<VrPostprocessing> m_postprocessing = new List<VrPostprocessing>();

		private int m_lastLod;

		private bool m_lastPhysics;

		private MyVoxelMesherComponent m_lastMesher;

		/// <summary>
		/// Tells based on a cube iso-configuration which edges are crossed by the isosurface
		///
		/// The standard MC table and diagrams from the original implementations and papers index the cube indices as a rotation in the xy plane:
		/// Namely the indices for (0,0), (1,0), (0,1) and (1,1) being 0, 1, 3 and 2 repectivelly.
		///
		/// This can be confusing in many cases where one would expect a mo0re traditional Z curve order.
		/// So the table here was re ordered in Z order.
		///
		///
		/// Here is a good page about marching cubes.
		/// http://paulbourke.net/geometry/polygonise/
		/// </summary>
		public static readonly int[] EdgeTable = (int[])(object)new int[256]
		{
			0,
			265,
			515,
			778,
			2060,
			2309,
			2575,
			2822,
			1030,
			1295,
			1541,
			1804,
			3082,
			3331,
			3593,
			3840,
			400,
			153,
			915,
			666,
			2460,
			2197,
			2975,
			2710,
			1430,
			1183,
			1941,
			1692,
			3482,
			3219,
			3993,
			3728,
			560,
			825,
			51,
			314,
			2620,
			2869,
			2111,
			2358,
			1590,
			1855,
			1077,
			1340,
			3642,
			3891,
			3129,
			3376,
			928,
			681,
			419,
			170,
			2988,
			2725,
			2479,
			2214,
			1958,
			1711,
			1445,
			1196,
			4010,
			3747,
			3497,
			3232,
			2240,
			2505,
			2755,
			3018,
			204,
			453,
			719,
			966,
			3270,
			3535,
			3781,
			4044,
			1226,
			1475,
			1737,
			1984,
			2384,
			2137,
			2899,
			2650,
			348,
			85,
			863,
			598,
			3414,
			3167,
			3925,
			3676,
			1370,
			1107,
			1881,
			1616,
			2800,
			3065,
			2291,
			2554,
			764,
			1013,
			255,
			502,
			3830,
			4095,
			3317,
			3580,
			1786,
			2035,
			1273,
			1520,
			2912,
			2665,
			2403,
			2154,
			876,
			613,
			367,
			102,
			3942,
			3695,
			3429,
			3180,
			1898,
			1635,
			1385,
			1120,
			1120,
			1385,
			1635,
			1898,
			3180,
			3429,
			3695,
			3942,
			102,
			367,
			613,
			876,
			2154,
			2403,
			2665,
			2912,
			1520,
			1273,
			2035,
			1786,
			3580,
			3317,
			4095,
			3830,
			502,
			255,
			1013,
			764,
			2554,
			2291,
			3065,
			2800,
			1616,
			1881,
			1107,
			1370,
			3676,
			3925,
			3167,
			3414,
			598,
			863,
			85,
			348,
			2650,
			2899,
			2137,
			2384,
			1984,
			1737,
			1475,
			1226,
			4044,
			3781,
			3535,
			3270,
			966,
			719,
			453,
			204,
			3018,
			2755,
			2505,
			2240,
			3232,
			3497,
			3747,
			4010,
			1196,
			1445,
			1711,
			1958,
			2214,
			2479,
			2725,
			2988,
			170,
			419,
			681,
			928,
			3376,
			3129,
			3891,
			3642,
			1340,
			1077,
			1855,
			1590,
			2358,
			2111,
			2869,
			2620,
			314,
			51,
			825,
			560,
			3728,
			3993,
			3219,
			3482,
			1692,
			1941,
			1183,
			1430,
			2710,
			2975,
			2197,
			2460,
			666,
			915,
			153,
			400,
			3840,
			3593,
			3331,
			3082,
			1804,
			1541,
			1295,
			1030,
			2822,
			2575,
			2309,
			2060,
			778,
			515,
			265,
			0
		};

		public static MyDualContouringMesher Static
		{
			get
			{
				if (m_threadInstance == null)
				{
					VRageNative.SetDebugMode(MyCompilationSymbols.IsDebugBuild);
					m_threadInstance = new MyDualContouringMesher();
				}
				return m_threadInstance;
			}
		}

		public int AffectedRangeOffset => -1;

		public int AffectedRangeSizeChange => 5;

		public int InvalidatedRangeInflate => 4;

		public MyStorageData StorageData => m_storageData;

		/// <summary>
		/// Calculate a voxel mesh.
		///
		/// Given the size sz of the range requested the resulting mesh will have sz -1 vertices (given it's dual nature),
		/// and sz -2 quads 9given that quads are dual to vertices.
		/// </summary>
		/// <param name="mesherComponent">The Voxel mesher entity component responsible for this request.</param>
		/// <param name="lod">The level of detail requested.</param>
		/// <param name="voxelStart">The starting range in voxels for the request.</param>
		/// <param name="voxelEnd">End range invoxels to use.</param>
		/// <param name="properties">Which properties are to be computed.</param>
		/// <param name="flags">Request flags to pass onto the data request performed by the contour.</param>
		/// <param name="target">Mesh to store results into, this mesh will be cleared regardless of the result being empty.</param>
		/// <returns></returns>
		public MyMesherResult Calculate(MyVoxelMesherComponent mesherComponent, int lod, Vector3I voxelStart, Vector3I voxelEnd, MyStorageDataTypeFlags properties = MyStorageDataTypeFlags.ContentAndMaterial, MyVoxelRequestFlags flags = (MyVoxelRequestFlags)0, VrVoxelMesh target = null)
		{
			bool flag = flags.HasFlags(MyVoxelRequestFlags.ForPhysics);
			if (m_lastMesher != mesherComponent || m_lastLod != lod || m_lastPhysics != flag)
			{
				m_lastLod = lod;
				m_lastMesher = mesherComponent;
				m_lastPhysics = flag;
				PreparePostprocessing(m_lastMesher.PostprocessingSteps, lod, flag);
			}
			return Calculate(mesherComponent.Storage, lod, voxelStart, voxelEnd, properties, flags | MyVoxelRequestFlags.Postprocess, target);
		}

		public unsafe MyMesherResult Calculate(IMyStorage storage, int lod, Vector3I voxelStart, Vector3I voxelEnd, MyStorageDataTypeFlags properties = MyStorageDataTypeFlags.ContentAndMaterial, MyVoxelRequestFlags flags = (MyVoxelRequestFlags)0, VrVoxelMesh target = null)
		{
			if (storage == null)
			{
				return MyMesherResult.Empty;
			}
			using (StoragePin storagePin = storage.Pin())
			{
				if (!storagePin.Valid)
				{
					return MyMesherResult.Empty;
				}
				MyVoxelRequestFlags requestFlags = flags | MyVoxelRequestFlags.UseNativeProvider;
				m_storageData.Resize(voxelStart, voxelEnd);
				storage.ReadRange(m_storageData, MyStorageDataTypeFlags.Content, lod, voxelStart, voxelEnd, ref requestFlags);
				if (requestFlags.HasFlags(MyVoxelRequestFlags.EmptyData))
				{
					return new MyMesherResult(MyVoxelContentConstitution.Empty);
				}
				if (requestFlags.HasFlags(MyVoxelRequestFlags.FullContent))
				{
					return new MyMesherResult(MyVoxelContentConstitution.Full);
				}
				if (!requestFlags.HasFlags(MyVoxelRequestFlags.ContentChecked))
				{
					MyVoxelContentConstitution myVoxelContentConstitution = m_storageData.ComputeContentConstitution();
					if (myVoxelContentConstitution != MyVoxelContentConstitution.Mixed)
					{
						return new MyMesherResult(myVoxelContentConstitution);
					}
				}
				if (properties.Requests(MyStorageDataTypeEnum.Material))
				{
					m_storageData.ClearMaterials(byte.MaxValue);
				}
				VrVoxelMesh vrVoxelMesh;
				if (target == null)
				{
					vrVoxelMesh = new VrVoxelMesh(voxelStart, voxelEnd, lod);
				}
				else
				{
					target.Clear();
					vrVoxelMesh = target;
				}
				IsoMesher isoMesher = new IsoMesher(vrVoxelMesh);
				try
				{
					fixed (byte* content = m_storageData[MyStorageDataTypeEnum.Content])
					{
						try
						{
							fixed (byte* material = m_storageData[MyStorageDataTypeEnum.Material])
							{
								Vector3I size3D = m_storageData.Size3D;
								isoMesher.Calculate(size3D.X, content, material);
							}
						}
						finally
						{
						}
					}
				}
				finally
				{
				}
				if (properties.Requests(MyStorageDataTypeEnum.Material))
				{
					requestFlags = flags;
					requestFlags &= ~MyVoxelRequestFlags.SurfaceMaterial;
					requestFlags |= MyVoxelRequestFlags.ConsiderContent;
					MyStorageDataTypeFlags dataToRead = properties.Without(MyStorageDataTypeEnum.Content);
					storage.ReadRange(m_storageData, dataToRead, lod, voxelStart, voxelEnd, ref requestFlags);
					try
					{
						fixed (byte* content2 = m_storageData[MyStorageDataTypeEnum.Content])
						{
							try
							{
								fixed (byte* material2 = m_storageData[MyStorageDataTypeEnum.Material])
								{
									int materialOverride = requestFlags.HasFlags(MyVoxelRequestFlags.OneMaterial) ? m_storageData.Material(0) : (-1);
									Vector3I size3D2 = m_storageData.Size3D;
									isoMesher.CalculateMaterials(size3D2.X, content2, material2, materialOverride);
								}
							}
							finally
							{
							}
						}
					}
					finally
					{
					}
				}
				if (vrVoxelMesh.VertexCount == 0)
				{
					if (vrVoxelMesh != target)
					{
						vrVoxelMesh.Dispose();
					}
					return MyMesherResult.Empty;
				}
				if (Postprocess && flags.HasFlags(MyVoxelRequestFlags.Postprocess))
				{
					isoMesher.PostProcess(m_postprocessing);
				}
				if (Postprocess && storage.DataProvider != null)
				{
					storage.DataProvider.PostProcess(vrVoxelMesh, properties);
				}
				return new MyMesherResult(vrVoxelMesh);
			}
		}

		MyIsoMesh IMyIsoMesher.Precalc(IMyStorage storage, int lod, Vector3I voxelStart, Vector3I voxelEnd, MyStorageDataTypeFlags properties, MyVoxelRequestFlags flags)
		{
			VrVoxelMesh mesh = Calculate(storage, lod, voxelStart, voxelEnd, properties, flags).Mesh;
			MyIsoMesh result = MyIsoMesh.FromNative(mesh);
			mesh?.Dispose();
			return result;
		}

		private void PreparePostprocessing(ListReader<MyVoxelPostprocessing> steps, int lod, bool physics)
		{
			m_postprocessing.Clear();
			foreach (MyVoxelPostprocessing item in steps)
			{
				if ((item.UseForPhysics || !physics) && item.Get(lod, out VrPostprocessing postprocess))
				{
					m_postprocessing.Add(postprocess);
				}
			}
		}

		/// <summary>
		/// Given a vertex configuration determine the quads required to correctly connect those vertices.
		///
		/// To build a cube mask:
		///  - assign every vertex in a cube an index equal to its (x,y,z) (dot) (1, 2, 4).
		///  - for every corner that has a positive sign, assign the mask bit given by it's index to one.
		///  - leave remaining bits in the mask assigned to 0.
		/// </summary>
		/// <param name="cubeMask"></param>
		public static void GenerateQuads(byte cubeMask, ushort[] corners, List<MyVoxelQuad> outQuads)
		{
			int num = EdgeTable[cubeMask];
			if (EdgeTable[cubeMask] == 0)
			{
				return;
			}
			ushort num2 = corners[0];
			for (int i = 0; i < 3; i++)
			{
				ushort v;
				ushort num3;
				ushort v2;
				bool flag;
				if (i == 0 && (num & 0x400) != 0)
				{
					v = corners[1];
					num3 = corners[3];
					v2 = corners[2];
					flag = ((cubeMask & 0x80) != 0);
				}
				else if (i == 1 && (num & 0x40) != 0)
				{
					v = corners[4];
					num3 = corners[6];
					v2 = corners[2];
					flag = ((cubeMask & 0x40) != 0);
				}
				else
				{
					if (i != 2 || (num & 0x20) == 0)
					{
						continue;
					}
					v = corners[1];
					num3 = corners[5];
					v2 = corners[4];
					flag = ((cubeMask & 0x20) != 0);
				}
				if (flag)
				{
					outQuads.Add(new MyVoxelQuad(num2, v, num3, v2));
				}
				else
				{
					outQuads.Add(new MyVoxelQuad(num3, v, num2, v2));
				}
			}
		}
	}
}
