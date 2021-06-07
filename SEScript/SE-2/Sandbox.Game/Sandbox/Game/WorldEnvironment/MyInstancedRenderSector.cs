using Sandbox.Engine.Utils;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game.Models;
using VRage.Library.Collections;
using VRageMath;
using VRageRender;
using VRageRender.Import;
using VRageRender.Messages;

namespace Sandbox.Game.WorldEnvironment
{
	public class MyInstancedRenderSector
	{
		private class InstancedModelBuffer
		{
			public MyInstanceData[] Instances = new MyInstanceData[4];

			public uint[] InstanceOIDs;

			public Queue<short> UnusedSlots = new Queue<short>();

			public uint InstanceBufferId = uint.MaxValue;

			public uint RenderObjectId = uint.MaxValue;

			public BoundingBox Bounds = BoundingBox.CreateInvalid();

			public int Model;

			private readonly MyInstancedRenderSector m_parent;

			public readonly BoundingBox ModelBb;

			public InstancedModelBuffer(MyInstancedRenderSector parent, int modelId)
			{
				m_parent = parent;
				Model = modelId;
				MyModel modelOnlyData = MyModels.GetModelOnlyData(MyModel.GetById(Model));
				ModelBb = modelOnlyData.BoundingBox;
			}

			private void UpdateRenderObjectsWithTheNew()
			{
				string byId = MyModel.GetById(Model);
				if (RenderObjectId != uint.MaxValue)
				{
					MyRenderProxy.RemoveRenderObject(RenderObjectId, MyRenderProxy.ObjectType.Entity);
				}
				Vector3D translation = m_parent.WorldMatrix.Translation;
				Matrix matrix = m_parent.WorldMatrix;
				matrix.Translation = Vector3.Zero;
				Matrix[] array = new Matrix[Instances.Length];
				for (int i = 0; i < Instances.Length; i++)
				{
					MyInstanceData myInstanceData = Instances[i];
					Matrix localMatrix = myInstanceData.LocalMatrix;
					array[i] = localMatrix * matrix;
				}
				RenderObjectId = MyRenderProxy.CreateStaticGroup(byId, translation, array);
			}

			private unsafe void UpdateRenderObjectsWithTheOld()
			{
				string byId = MyModel.GetById(Model);
				if (InstanceOIDs == null)
				{
					BoundingBox bounds = Bounds;
					if (RenderObjectId == uint.MaxValue)
					{
						RenderObjectId = MyRenderProxy.CreateRenderEntity($"RO::{m_parent.Name}: {byId}", byId, m_parent.WorldMatrix, MyMeshDrawTechnique.MESH, RenderFlags.CastShadows | RenderFlags.Visible | RenderFlags.ForceOldPipeline | RenderFlags.DistanceFade, CullingOptions.Default, Vector3.One, Vector3.Zero, 0f, 100000f, 0, 1f, fadeIn: true);
					}
					if (InstanceBufferId == uint.MaxValue)
					{
						InstanceBufferId = MyRenderProxy.CreateRenderInstanceBuffer($"IB::{m_parent.Name}: {byId}", MyRenderInstanceBufferType.Generic);
					}
					MyRenderProxy.UpdateRenderInstanceBufferRange(InstanceBufferId, Instances);
					MyRenderProxy.SetInstanceBuffer(RenderObjectId, InstanceBufferId, 0, Instances.Length, bounds);
					MyRenderProxy.UpdateRenderObject(RenderObjectId, m_parent.WorldMatrix);
				}
				else
				{
					if (InstanceOIDs.Length != Instances.Length)
					{
						ResizeActorBuffer();
					}
					fixed (MyInstanceData* ptr = Instances)
					{
						for (int i = 0; i < InstanceOIDs.Length; i++)
						{
							if (InstanceOIDs[i] == uint.MaxValue && ptr[i].m_row0.PackedValue != 0L)
							{
								MatrixD matrixD = ptr[i].LocalMatrix * m_parent.WorldMatrix;
								uint num = MyRenderProxy.CreateRenderEntity($"RO::{m_parent.Name}: {byId}", byId, matrixD, MyMeshDrawTechnique.MESH, RenderFlags.CastShadows | RenderFlags.Visible, CullingOptions.Default, Vector3.One, Vector3.Zero, 0f, 100000f, 0, 1f, fadeIn: true);
								MyRenderProxy.UpdateRenderObject(num, matrixD, ModelBb);
								InstanceOIDs[i] = num;
							}
						}
					}
				}
			}

			public void UpdateRenderObjects()
			{
				if (MyFakes.ENABLE_TREES_IN_THE_NEW_PIPE)
				{
					UpdateRenderObjectsWithTheNew();
				}
				else
				{
					UpdateRenderObjectsWithTheOld();
				}
			}

			private void ClearRenderObjectsWithTheNew()
			{
				if (RenderObjectId != uint.MaxValue)
				{
					MyRenderProxy.RemoveRenderObject(RenderObjectId, MyRenderProxy.ObjectType.Entity);
					RenderObjectId = uint.MaxValue;
				}
			}

			private void ClearRenderObjectsWithTheOld()
			{
				if (InstanceBufferId != uint.MaxValue)
				{
					MyRenderProxy.RemoveRenderObject(InstanceBufferId, MyRenderProxy.ObjectType.InstanceBuffer);
					InstanceBufferId = uint.MaxValue;
				}
				if (RenderObjectId != uint.MaxValue)
				{
					MyRenderProxy.RemoveRenderObject(RenderObjectId, MyRenderProxy.ObjectType.Entity, fadeOut: true);
					RenderObjectId = uint.MaxValue;
				}
				if (InstanceOIDs == null)
				{
					return;
				}
				for (int i = 0; i < InstanceOIDs.Length; i++)
				{
					if (InstanceOIDs[i] != uint.MaxValue)
					{
						MyRenderProxy.RemoveRenderObject(InstanceOIDs[i], MyRenderProxy.ObjectType.Entity);
						InstanceOIDs[i] = uint.MaxValue;
					}
				}
			}

			public void ClearRenderObjects()
			{
				if (MyFakes.ENABLE_TREES_IN_THE_NEW_PIPE)
				{
					ClearRenderObjectsWithTheNew();
				}
				else
				{
					ClearRenderObjectsWithTheOld();
				}
			}

			public void Close()
			{
				ClearRenderObjects();
				Bounds = BoundingBox.CreateInvalid();
				Instances = null;
				InstanceOIDs = null;
				UnusedSlots.Clear();
			}

			public void SetPerInstanceLod(bool value)
			{
				if (value != (InstanceOIDs != null))
				{
					_ = Instances;
				}
			}

			private void ResizeActorBuffer()
			{
				int num = (InstanceOIDs != null) ? InstanceOIDs.Length : 0;
				Array.Resize(ref InstanceOIDs, Instances.Length);
				for (int i = num; i < InstanceOIDs.Length; i++)
				{
					InstanceOIDs[i] = uint.MaxValue;
				}
			}
		}

		private const bool ENABLE_SEPARATE_INSTANCE_LOD = false;

		public MatrixD WorldMatrix;

		private readonly Dictionary<int, InstancedModelBuffer> m_instancedModels = new Dictionary<int, InstancedModelBuffer>();

		private readonly HashSet<int> m_changedBuffers = new HashSet<int>();

		private int m_lod;

		public string Name
		{
			get;
			private set;
		}

		public int Lod
		{
			get
			{
				return m_lod;
			}
			set
			{
				if (m_lod != value && value != -1)
				{
					foreach (InstancedModelBuffer value2 in m_instancedModels.Values)
					{
						value2.SetPerInstanceLod(value == 0);
					}
				}
				m_lod = value;
			}
		}

		public MyInstancedRenderSector(string name, MatrixD worldMatrix)
		{
			Name = name;
			WorldMatrix = worldMatrix;
		}

		private int GetExpandedSize(int size)
		{
			return size + 5;
		}

		public int AddInstances(int model, MyList<MyInstanceData> instances)
		{
			if (!m_instancedModels.TryGetValue(model, out InstancedModelBuffer value))
			{
				value = new InstancedModelBuffer(this, model);
				value.SetPerInstanceLod(Lod == 0);
				m_instancedModels[model] = value;
			}
			value.Instances = instances.GetInternalArray();
			int count = instances.Count;
			for (int i = 0; i < count; i++)
			{
				BoundingBox box = value.ModelBb.Transform(value.Instances[i].LocalMatrix);
				value.Bounds.Include(ref box);
			}
			value.UnusedSlots.Clear();
			for (int j = count; j < instances.Capacity; j++)
			{
				value.UnusedSlots.Enqueue((short)j);
			}
			m_changedBuffers.Add(model);
			return 0;
		}

		public void CommitChangesToRenderer()
		{
			foreach (int changedBuffer in m_changedBuffers)
			{
				m_instancedModels[changedBuffer].UpdateRenderObjects();
			}
			m_changedBuffers.Clear();
		}

		public void Close()
		{
			foreach (InstancedModelBuffer value in m_instancedModels.Values)
			{
				value.Close();
			}
		}

		public bool HasChanges()
		{
			return m_changedBuffers.Count != 0;
		}

		public void DetachEnvironment(MyEnvironmentSector myEnvironmentSector)
		{
			Close();
		}

		public void RemoveInstance(int modelId, short index)
		{
			InstancedModelBuffer instancedModelBuffer = m_instancedModels[modelId];
			instancedModelBuffer.Instances[index] = default(MyInstanceData);
			instancedModelBuffer.UnusedSlots.Enqueue(index);
			m_changedBuffers.Add(modelId);
		}

		public short AddInstance(int modelId, ref MyInstanceData data)
		{
			if (!m_instancedModels.TryGetValue(modelId, out InstancedModelBuffer value))
			{
				value = new InstancedModelBuffer(this, modelId);
				value.SetPerInstanceLod(Lod == 0);
				m_instancedModels[modelId] = value;
			}
			if (QueueExtensions.TryDequeue(value.UnusedSlots, out short result))
			{
				value.Instances[result] = data;
			}
			else
			{
				int num = (value.Instances != null) ? value.Instances.Length : 0;
				int expandedSize = GetExpandedSize(num);
				Array.Resize(ref value.Instances, expandedSize);
				int num2 = expandedSize - num;
				result = (short)num;
				value.Instances[result] = data;
				for (int i = 1; i < num2; i++)
				{
					value.UnusedSlots.Enqueue((short)(i + result));
				}
			}
			BoundingBox box = value.ModelBb.Transform(data.LocalMatrix);
			value.Bounds.Include(ref box);
			m_changedBuffers.Add(modelId);
			return result;
		}

		public uint GetRenderEntity(int modelId)
		{
			if (m_instancedModels.TryGetValue(modelId, out InstancedModelBuffer value))
			{
				return value.RenderObjectId;
			}
			return uint.MaxValue;
		}
	}
}
