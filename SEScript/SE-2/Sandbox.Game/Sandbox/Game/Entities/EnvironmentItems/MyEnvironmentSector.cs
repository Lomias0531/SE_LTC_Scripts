using Sandbox.Engine.Utils;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Game.Models;
using VRage.Utils;
using VRageMath;
using VRageMath.PackedVector;
using VRageRender;
using VRageRender.Import;
using VRageRender.Messages;

namespace Sandbox.Game.Entities.EnvironmentItems
{
	public class MyEnvironmentSector
	{
		private struct MySectorInstanceData
		{
			public int LocalId;

			public MyInstanceData InstanceData;
		}

		private class MyModelInstanceData
		{
			public MyEnvironmentSector Parent;

			public int Model;

			public readonly MyStringHash SubtypeId;

			public readonly MyInstanceFlagsEnum Flags = MyInstanceFlagsEnum.CastShadows | MyInstanceFlagsEnum.ShowLod1 | MyInstanceFlagsEnum.EnableColorMask;

			public readonly float MaxViewDistance = float.MaxValue;

			public readonly Dictionary<int, MyInstanceData> InstanceData = new Dictionary<int, MyInstanceData>();

			public readonly Dictionary<int, int> InstanceIds = new Dictionary<int, int>();

			private int m_keyIndex;

			public readonly BoundingBox ModelBox;

			public uint InstanceBuffer = uint.MaxValue;

			public uint RenderObjectId = uint.MaxValue;

			public FastResourceLock InstanceBufferLock = new FastResourceLock();

			private bool m_changed;

			public int InstanceCount => InstanceData.Count;

			public MyModelInstanceData(MyEnvironmentSector parent, MyStringHash subtypeId, int model, MyInstanceFlagsEnum flags, float maxViewDistance, BoundingBox modelBox)
			{
				Parent = parent;
				SubtypeId = subtypeId;
				Flags = flags;
				MaxViewDistance = maxViewDistance;
				ModelBox = modelBox;
				Model = model;
			}

			public int AddInstanceData(ref MySectorInstanceData instanceData)
			{
				using (InstanceBufferLock.AcquireExclusiveUsing())
				{
					while (InstanceData.ContainsKey(m_keyIndex) && InstanceData.Count < int.MaxValue)
					{
						m_keyIndex++;
					}
					if (InstanceData.ContainsKey(m_keyIndex))
					{
						throw new Exception("No available keys to add new instance data to sector!");
					}
					InstanceData.Add(m_keyIndex, instanceData.InstanceData);
					InstanceIds.Add(m_keyIndex, instanceData.LocalId);
					return m_keyIndex;
				}
			}

			public void UnloadRenderObjects()
			{
				if (InstanceBuffer != uint.MaxValue)
				{
					MyRenderProxy.RemoveRenderObject(InstanceBuffer, MyRenderProxy.ObjectType.InstanceBuffer);
					InstanceBuffer = uint.MaxValue;
				}
				if (RenderObjectId != uint.MaxValue)
				{
					MyRenderProxy.RemoveRenderObject(RenderObjectId, MyRenderProxy.ObjectType.Entity, fadeOut: true);
					RenderObjectId = uint.MaxValue;
				}
			}

			public void UpdateRenderInstanceData()
			{
				if (InstanceData.Count != 0)
				{
					if (InstanceBuffer == uint.MaxValue)
					{
						InstanceBuffer = MyRenderProxy.CreateRenderInstanceBuffer($"EnvironmentSector{Parent.SectorId} - {SubtypeId}", MyRenderInstanceBufferType.Generic);
					}
					MyRenderProxy.UpdateRenderInstanceBufferRange(InstanceBuffer, InstanceData.Values.ToArray());
				}
			}

			public bool DisableInstance(int sectorInstanceId)
			{
				using (InstanceBufferLock.AcquireExclusiveUsing())
				{
					if (!InstanceData.ContainsKey(sectorInstanceId))
					{
						_ = MyFakes.ENABLE_FLORA_COMPONENT_DEBUG;
						return false;
					}
					InstanceData.Remove(sectorInstanceId);
					InstanceIds.Remove(sectorInstanceId);
				}
				return true;
			}

			internal void UpdateRenderEntitiesData(ref MatrixD worldMatrixD, bool useTransparency, float transparency)
			{
				int model = Model;
				bool num = InstanceCount > 0;
				bool flag = RenderObjectId != uint.MaxValue;
				if (!num)
				{
					if (flag)
					{
						UnloadRenderObjects();
					}
					return;
				}
				RenderFlags flags = RenderFlags.CastShadows | RenderFlags.Visible;
				if (!flag)
				{
					string byId = MyModel.GetById(model);
					RenderObjectId = MyRenderProxy.CreateRenderEntity("Instance parts, part: " + model, byId, Parent.SectorMatrix, MyMeshDrawTechnique.MESH, flags, CullingOptions.Default, Vector3.One, Vector3.Zero, useTransparency ? transparency : 0f, MaxViewDistance, 0, 1f, fadeIn: true);
				}
				MyRenderProxy.SetInstanceBuffer(RenderObjectId, InstanceBuffer, 0, InstanceData.Count, Parent.SectorBox);
				MyRenderProxy.UpdateRenderEntity(RenderObjectId, Vector3.One, Vector3.Zero, useTransparency ? transparency : 0f, fadeIn: true);
				MatrixD sectorMatrix = Parent.SectorMatrix;
				MyRenderProxy.UpdateRenderObject(RenderObjectId, sectorMatrix);
			}
		}

		private readonly Vector3I m_id;

		private MatrixD m_sectorMatrix;

		private MatrixD m_sectorInvMatrix;

		private FastResourceLock m_instancePartsLock = new FastResourceLock();

		private Dictionary<int, MyModelInstanceData> m_instanceParts = new Dictionary<int, MyModelInstanceData>();

		private List<MyInstanceData> m_tmpInstanceData = new List<MyInstanceData>();

		private BoundingBox m_AABB = BoundingBox.CreateInvalid();

		private bool m_invalidateAABB;

		private int m_sectorItemCount;

		public Vector3I SectorId => m_id;

		public MatrixD SectorMatrix => m_sectorMatrix;

		public bool IsValid => m_sectorItemCount > 0;

		public BoundingBox SectorBox
		{
			get
			{
				if (m_invalidateAABB)
				{
					m_invalidateAABB = false;
					m_AABB = GetSectorBoundingBox();
				}
				return m_AABB;
			}
		}

		public BoundingBoxD SectorWorldBox => SectorBox.Transform(m_sectorMatrix);

		public int SectorItemCount => m_sectorItemCount;

		public MyEnvironmentSector(Vector3I id, Vector3D sectorOffset)
		{
			m_id = id;
			m_sectorMatrix = MatrixD.CreateTranslation(sectorOffset);
			m_sectorInvMatrix = MatrixD.Invert(m_sectorMatrix);
		}

		public void UnloadRenderObjects()
		{
			using (m_instancePartsLock.AcquireExclusiveUsing())
			{
				foreach (KeyValuePair<int, MyModelInstanceData> instancePart in m_instanceParts)
				{
					instancePart.Value.UnloadRenderObjects();
				}
			}
		}

		public void ClearInstanceData()
		{
			m_tmpInstanceData.Clear();
			m_AABB = BoundingBox.CreateInvalid();
			m_sectorItemCount = 0;
			using (m_instancePartsLock.AcquireExclusiveUsing())
			{
				foreach (KeyValuePair<int, MyModelInstanceData> instancePart in m_instanceParts)
				{
					instancePart.Value.InstanceData.Clear();
				}
			}
		}

		public int AddInstance(MyStringHash subtypeId, int modelId, int localId, ref Matrix localMatrix, BoundingBox localAabb, MyInstanceFlagsEnum instanceFlags, float maxViewDistance, Vector3 colorMaskHsv = default(Vector3))
		{
			MyModelInstanceData value;
			using (m_instancePartsLock.AcquireExclusiveUsing())
			{
				if (!m_instanceParts.TryGetValue(modelId, out value))
				{
					value = new MyModelInstanceData(this, subtypeId, modelId, instanceFlags, maxViewDistance, localAabb);
					m_instanceParts.Add(modelId, value);
				}
			}
			MySectorInstanceData mySectorInstanceData = default(MySectorInstanceData);
			mySectorInstanceData.LocalId = localId;
			MyInstanceData myInstanceData = mySectorInstanceData.InstanceData = new MyInstanceData
			{
				ColorMaskHSV = new HalfVector4(colorMaskHsv.X, colorMaskHsv.Y, colorMaskHsv.Z, 0f),
				LocalMatrix = localMatrix
			};
			MySectorInstanceData instanceData = mySectorInstanceData;
			int num = value.AddInstanceData(ref instanceData);
			myInstanceData = value.InstanceData[num];
			localMatrix = myInstanceData.LocalMatrix;
			m_AABB = m_AABB.Include(localAabb.Transform(localMatrix));
			m_sectorItemCount++;
			m_invalidateAABB = true;
			return num;
		}

		public bool DisableInstance(int sectorInstanceId, int modelId)
		{
			MyModelInstanceData value = null;
			m_instanceParts.TryGetValue(modelId, out value);
			if (value == null)
			{
				return false;
			}
			if (value.DisableInstance(sectorInstanceId))
			{
				m_sectorItemCount--;
				m_invalidateAABB = true;
				return true;
			}
			return false;
		}

		public void UpdateRenderInstanceData()
		{
			using (m_instancePartsLock.AcquireSharedUsing())
			{
				foreach (KeyValuePair<int, MyModelInstanceData> instancePart in m_instanceParts)
				{
					instancePart.Value.UpdateRenderInstanceData();
				}
			}
		}

		public void UpdateRenderInstanceData(int modelId)
		{
			using (m_instancePartsLock.AcquireSharedUsing())
			{
				MyModelInstanceData value = null;
				m_instanceParts.TryGetValue(modelId, out value);
				value?.UpdateRenderInstanceData();
			}
		}

		public void UpdateRenderEntitiesData(MatrixD worldMatrixD, bool useTransparency = false, float transparency = 0f)
		{
			foreach (MyModelInstanceData value in m_instanceParts.Values)
			{
				value.UpdateRenderEntitiesData(ref worldMatrixD, useTransparency, transparency);
			}
		}

		public static Vector3I GetSectorId(Vector3D position, float sectorSize)
		{
			return Vector3I.Floor(position / sectorSize);
		}

		internal void DebugDraw(Vector3I sectorPos, float sectorSize)
		{
			using (m_instancePartsLock.AcquireSharedUsing())
			{
				foreach (MyModelInstanceData value2 in m_instanceParts.Values)
				{
					using (value2.InstanceBufferLock.AcquireSharedUsing())
					{
						foreach (KeyValuePair<int, MyInstanceData> instanceDatum in value2.InstanceData)
						{
							MyInstanceData value = instanceDatum.Value;
							Vector3D vector3D = Vector3D.Transform(value.LocalMatrix.Translation, m_sectorMatrix);
							MyRenderProxy.DebugDrawOBB(Matrix.Rescale(value.LocalMatrix * m_sectorMatrix, value2.ModelBox.HalfExtents * 2f), Color.OrangeRed, 0.5f, depthRead: true, smooth: true);
							if (Vector3D.Distance(MySector.MainCamera.Position, vector3D) < 250.0)
							{
								MyStringHash subtypeId = value2.SubtypeId;
								MyRenderProxy.DebugDrawText3D(vector3D, subtypeId.ToString(), Color.White, 0.7f, depthRead: true);
							}
						}
					}
				}
			}
			MyRenderProxy.DebugDrawAABB(SectorWorldBox, Color.OrangeRed);
		}

		internal void GetItems(List<Vector3D> output)
		{
			foreach (KeyValuePair<int, MyModelInstanceData> instancePart in m_instanceParts)
			{
				MyModelInstanceData value = instancePart.Value;
				using (value.InstanceBufferLock.AcquireSharedUsing())
				{
					foreach (KeyValuePair<int, MyInstanceData> instanceDatum in value.InstanceData)
					{
						MyInstanceData value2 = instanceDatum.Value;
						if (!value2.LocalMatrix.EqualsFast(ref Matrix.Zero))
						{
							output.Add(Vector3D.Transform(value2.LocalMatrix.Translation, m_sectorMatrix));
						}
					}
				}
			}
		}

		internal void GetItemsInRadius(Vector3D position, float radius, List<Vector3D> output)
		{
			Vector3D value = Vector3D.Transform(position, m_sectorInvMatrix);
			foreach (KeyValuePair<int, MyModelInstanceData> instancePart in m_instanceParts)
			{
				using (instancePart.Value.InstanceBufferLock.AcquireSharedUsing())
				{
					foreach (KeyValuePair<int, MyInstanceData> instanceDatum in instancePart.Value.InstanceData)
					{
						if (Vector3D.DistanceSquared(instanceDatum.Value.LocalMatrix.Translation, value) < (double)(radius * radius))
						{
							output.Add(Vector3D.Transform(instanceDatum.Value.LocalMatrix.Translation, m_sectorMatrix));
						}
					}
				}
			}
		}

		internal void GetItemsInRadius(Vector3 position, float radius, List<MyEnvironmentItems.ItemInfo> output)
		{
			double num = radius * radius;
			foreach (KeyValuePair<int, MyModelInstanceData> instancePart in m_instanceParts)
			{
				MyModelInstanceData value = instancePart.Value;
				using (value.InstanceBufferLock.AcquireSharedUsing())
				{
					foreach (KeyValuePair<int, MyInstanceData> instanceDatum in value.InstanceData)
					{
						MyInstanceData value2 = instanceDatum.Value;
						if (!value2.LocalMatrix.EqualsFast(ref Matrix.Zero))
						{
							Vector3D vector3D = Vector3.Transform(value2.LocalMatrix.Translation, m_sectorMatrix);
							if ((vector3D - position).LengthSquared() < num)
							{
								output.Add(new MyEnvironmentItems.ItemInfo
								{
									LocalId = value.InstanceIds[instanceDatum.Key],
									SubtypeId = instancePart.Value.SubtypeId,
									Transform = new MyTransformD(vector3D)
								});
							}
						}
					}
				}
			}
		}

		internal void GetItems(List<MyEnvironmentItems.ItemInfo> output)
		{
			foreach (KeyValuePair<int, MyModelInstanceData> instancePart in m_instanceParts)
			{
				MyModelInstanceData value = instancePart.Value;
				using (value.InstanceBufferLock.AcquireSharedUsing())
				{
					foreach (KeyValuePair<int, MyInstanceData> instanceDatum in value.InstanceData)
					{
						Matrix localMatrix = instanceDatum.Value.LocalMatrix;
						if (!localMatrix.EqualsFast(ref Matrix.Zero))
						{
							Vector3D position = Vector3.Transform(localMatrix.Translation, m_sectorMatrix);
							output.Add(new MyEnvironmentItems.ItemInfo
							{
								LocalId = value.InstanceIds[instanceDatum.Key],
								SubtypeId = instancePart.Value.SubtypeId,
								Transform = new MyTransformD(position)
							});
						}
					}
				}
			}
		}

		private BoundingBox GetSectorBoundingBox()
		{
			if (!IsValid)
			{
				return new BoundingBox(Vector3.Zero, Vector3.Zero);
			}
			BoundingBox result = BoundingBox.CreateInvalid();
			using (m_instancePartsLock.AcquireSharedUsing())
			{
				foreach (KeyValuePair<int, MyModelInstanceData> instancePart in m_instanceParts)
				{
					MyModelInstanceData value = instancePart.Value;
					using (value.InstanceBufferLock.AcquireSharedUsing())
					{
						BoundingBox modelBox = value.ModelBox;
						foreach (KeyValuePair<int, MyInstanceData> instanceDatum in value.InstanceData)
						{
							MyInstanceData value2 = instanceDatum.Value;
							if (!value2.LocalMatrix.EqualsFast(ref Matrix.Zero))
							{
								result.Include(modelBox.Transform(value2.LocalMatrix));
							}
						}
					}
				}
				return result;
			}
		}
	}
}
