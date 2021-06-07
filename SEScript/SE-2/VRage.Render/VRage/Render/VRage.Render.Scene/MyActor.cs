using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using VRage.Collections;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Render.Scene.Components;
using VRage.Render11.Common;
using VRage.Utils;
using VRageMath;
using VRageRender.Messages;

namespace VRage.Render.Scene
{
	[GenerateActivator]
	public class MyActor
	{
		public bool EnableAabbUpdateBasedOnChildren;

		private int m_perFrameUpdateCounter;

		public readonly bool[] OccludedState = new bool[19];

		public readonly long[] FrameInView = new long[19];

		private BoundingBoxD m_aabb;

		private MyIDTracker<MyActor> m_ID;

		private bool m_visible;

		private readonly MyIndexedComponentContainer<MyActorComponent> m_components = new MyIndexedComponentContainer<MyActorComponent>();

		private bool m_relativeTransformValid;

		private Matrix m_relativeTransform;

		private BoundingBox? m_localAabb;

		public MatrixD LastWorldMatrix;

		private int m_worldMatrixIndex;

		private volatile bool m_worldMatrixDirty;

		private MatrixD m_worldMatrixInv;

		private bool m_worldMatrixInvDirty;

		private bool m_dirtyProxy;

		private bool m_root;

		private int m_cullTreeId;

		private MyManualCullTreeData m_cullTreeData;

		private int m_childTreeId;

		private int m_globalTreeId;

		private int m_globalFarTreeId;

		private List<MyActor> m_children;

		private bool m_visibilityUpdates;

		private float m_relativeForwardScale;

		public string DebugName
		{
			get;
			private set;
		}

		public uint ID => m_ID.ID;

		public bool IsVisible => m_visible;

		public bool IsDestroyed => m_ID == null;

		public bool VisibilityUpdates => m_visibilityUpdates;

		public MyScene Scene
		{
			get;
			private set;
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public MatrixD WorldMatrix
		{
			get
			{
				UpdateWorldMatrix();
				return LastWorldMatrix;
			}
			private set
			{
				LastWorldMatrix = value;
				m_worldMatrixDirty = false;
				m_worldMatrixInvDirty = true;
				m_worldMatrixIndex++;
				WorldMatrixForwardScale = (float)LastWorldMatrix.Forward.Length();
			}
		}

		public float WorldMatrixForwardScale
		{
			get;
			private set;
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public Matrix RelativeTransform => m_relativeTransform;

		public int WorldMatrixIndex => m_worldMatrixIndex;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public MatrixD WorldMatrixInv
		{
			get
			{
				if (m_worldMatrixInvDirty || m_worldMatrixDirty)
				{
					m_worldMatrixInvDirty = false;
					m_worldMatrixInv = MatrixD.Invert(WorldMatrix);
				}
				return m_worldMatrixInv;
			}
		}

		public bool DirtyProxy => m_dirtyProxy;

		public bool HasLocalAabb => m_localAabb.HasValue;

		public MyActor Parent
		{
			get;
			private set;
		}

		public ListReader<MyActor> Children => m_children;

		public BoundingBox LocalAabb => m_localAabb.Value;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public BoundingBoxD WorldAabb => m_aabb;

		public bool AlwaysUpdate
		{
			get
			{
				return m_perFrameUpdateCounter > 0;
			}
			set
			{
				if (value)
				{
					if (m_perFrameUpdateCounter == 0)
					{
						Scene.Updater.AddToAlwaysUpdate(this);
					}
					m_perFrameUpdateCounter++;
				}
				else
				{
					m_perFrameUpdateCounter--;
					if (m_perFrameUpdateCounter == 0)
					{
						Scene.Updater.RemoveFromAlwaysUpdate(this);
					}
				}
			}
		}

		public float VolumeExtent
		{
			get;
			private set;
		}

		public event Action OnMove;

		public event Action<MyActor> OnDestruct;

		public void SetWorldMatrixDirty()
		{
			if (Parent != null)
			{
				m_worldMatrixDirty = true;
			}
			m_worldMatrixIndex++;
		}

		public MyActor(MyScene scene)
		{
			Scene = scene;
		}

		public MyActor()
		{
			throw new Exception("Invalid constructor");
		}

		public void SetVisibility(bool visibility)
		{
			if (m_visible != visibility)
			{
				m_visible = visibility;
				for (int i = 0; i < m_components.Count; i++)
				{
					m_components[i].OnVisibilityChange();
				}
				InvalidateCullTreeData();
			}
		}

		public void SetParent(MyActor parent)
		{
			if (parent != Parent)
			{
				if (Parent != null)
				{
					Parent.Remove(this);
					m_worldMatrixDirty = false;
				}
				Parent = parent;
				if (Parent != null)
				{
					Parent.Add(this);
				}
			}
		}

		public void SetRoot(bool state)
		{
			if (!m_root)
			{
				m_root = state;
				m_cullTreeData = Scene.AllocateGroupData();
				m_cullTreeData.Actor = this;
			}
		}

		public bool IsRoot()
		{
			return m_root;
		}

		public Color GetDebugColor()
		{
			Color color = Color.Magenta;
			for (int i = 0; i < m_components.Count; i++)
			{
				color = m_components[i].DebugColor;
				if (color != Color.Magenta)
				{
					if (Parent == null)
					{
						return Vector4.Lerp(color.ToVector4().ToLinearRGB(), Color.Magenta, 0.5f).ToSRGB();
					}
					return color;
				}
			}
			return color;
		}

		public void SetTransforms(MyRenderObjectUpdateData data)
		{
			if (data.LocalMatrix.HasValue)
			{
				m_relativeTransform = data.LocalMatrix.Value;
				m_relativeForwardScale = m_relativeTransform.Forward.Length();
				m_relativeTransformValid = true;
			}
			else if (data.WorldMatrix.HasValue)
			{
				WorldMatrix = data.WorldMatrix.Value;
			}
			if (data.LocalAABB.HasValue)
			{
				m_localAabb = data.LocalAABB.Value;
				UpdateVolumeExtent();
			}
			OnMatrixChange();
		}

		public void SetMatrix(ref MatrixD matrix)
		{
			WorldMatrix = matrix;
			OnMatrixChange();
		}

		public void SetRelativeTransform(ref Matrix m)
		{
			m_relativeTransform = m;
			m_relativeForwardScale = m_relativeTransform.Forward.Length();
			m_relativeTransformValid = true;
			OnMatrixChange();
		}

		public void AddLocalAabb(BoundingBox localAabb)
		{
			if (m_localAabb.HasValue)
			{
				m_localAabb = m_localAabb.Value.Include(localAabb);
			}
			else
			{
				m_localAabb = localAabb;
			}
			UpdateVolumeExtent();
			OnMatrixChange();
		}

		public void SetLocalAabb(BoundingBox localAabb)
		{
			m_localAabb = localAabb;
			UpdateVolumeExtent();
			OnMatrixChange();
		}

		private void UpdateVolumeExtent()
		{
			Vector3 vector = (Parent == null) ? ((Vector3)m_aabb.Extents) : m_localAabb.Value.Extents;
			VolumeExtent = Math.Max(Math.Max(vector.X, vector.Y), vector.Z);
		}

		public float CalculateCameraDistanceSquaredFast()
		{
			return (float)(WorldMatrix.Translation - Scene.Environment.CameraPosition).LengthSquared();
		}

		public float CalculateCameraDistanceSquared()
		{
			if (Parent == null)
			{
				return (float)m_aabb.DistanceSquared(Scene.Environment.CameraPosition);
			}
			Vector3D v = Vector3D.Transform(Scene.Environment.CameraPosition, WorldMatrixInv);
			return m_localAabb.Value.DistanceSquared(v);
		}

		public void Construct(string debugName)
		{
			m_components.Clear();
			m_cullTreeId = -1;
			m_childTreeId = -1;
			m_children = new List<MyActor>();
			m_dirtyProxy = false;
			m_root = false;
			m_globalTreeId = -1;
			m_globalFarTreeId = -1;
			m_visible = true;
			m_visibilityUpdates = false;
			DebugName = debugName;
			MyUtils.Init(ref m_ID);
			m_ID.Clear();
			m_localAabb = null;
			WorldMatrix = MatrixD.Identity;
			m_relativeTransformValid = false;
		}

		public void Destruct()
		{
			if (m_ID != null)
			{
				SetParent(null);
				if (this.OnDestruct != null)
				{
					this.OnDestruct(this);
				}
				this.OnDestruct = null;
				while (m_children.Count > 0)
				{
					m_children[0].SetParent(null);
				}
				m_children = null;
				for (int i = 0; i < m_components.Count; i++)
				{
					m_components[i].OnRemove(this);
				}
				m_components.Clear();
				RemoveProxy();
				if (m_cullTreeData != null)
				{
					Scene.FreeGroupData(m_cullTreeData);
					m_cullTreeData = null;
				}
				if (m_cullTreeId != -1)
				{
					Scene.ManualCullTree.RemoveProxy(m_cullTreeId);
					m_cullTreeId = -1;
				}
				if (m_ID.Value != null)
				{
					m_ID.Deregister();
				}
				m_ID = null;
				this.OnMove = null;
				for (int j = 0; j < OccludedState.Length; j++)
				{
					OccludedState[j] = false;
				}
				Scene.Updater.RemoveFromUpdates(this);
			}
		}

		public void SetID(uint id)
		{
			m_ID.Register(id, this);
		}

		public BoundingBoxD CalculateAabb()
		{
			return m_localAabb.Value.Transform(WorldMatrix);
		}

		public void UpdateBeforeDraw()
		{
			OnUpdateBeforeDraw();
			UpdateProxy();
		}

		public void AddComponent<T>(MyActorComponent component) where T : MyActorComponent
		{
			AddComponent(typeof(T), component);
		}

		public void AddComponent(Type t, MyActorComponent component)
		{
			component.Assign(this);
			m_components.Add(t, component);
		}

		public void RemoveComponent<T>(MyActorComponent component) where T : MyActorComponent
		{
			RemoveComponent(typeof(T), component);
		}

		public void RemoveComponent(Type t, MyActorComponent component)
		{
			RemoveProxy();
			component.OnRemove(this);
			m_components.Remove(t);
			InvalidateCullTreeData();
		}

		public T GetComponent<T>() where T : MyActorComponent
		{
			return m_components.TryGetComponent<T>();
		}

		public void UpdateComponent(MyRenderMessageUpdateComponent message)
		{
			UpdateData data = message.Data;
			MyActorComponent myActorComponent = m_components.TryGetComponent(data.ComponentType);
			if (message.Type == MyRenderMessageUpdateComponent.UpdateType.Delete)
			{
				if (myActorComponent != null)
				{
					RemoveComponent(data.ComponentType, myActorComponent);
				}
				return;
			}
			if (myActorComponent == null)
			{
				myActorComponent = Scene.ComponentFactory.Create(data.ComponentType);
				AddComponent(data.ComponentType, myActorComponent);
			}
			((MyRenderDirectComponent)myActorComponent).Update(data);
		}

		private void Add(MyActor child)
		{
			child.Parent = this;
			m_children.Add(child);
			if (!child.m_relativeTransformValid)
			{
				Matrix m = child.WorldMatrix * WorldMatrixInv;
				child.SetRelativeTransform(ref m);
			}
			child.OnParentSet();
			child.InvalidateCullTreeData();
		}

		private void Remove(MyActor child)
		{
			child.OnParentRemoved();
			child.RemoveProxy();
			m_children.Remove(child);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateWorldMatrix()
		{
			if (m_worldMatrixDirty)
			{
				if (m_relativeTransformValid)
				{
					MatrixD.Multiply(ref m_relativeTransform, ref Parent.LastWorldMatrix, out LastWorldMatrix);
					WorldMatrixForwardScale = Parent.WorldMatrixForwardScale * m_relativeForwardScale;
				}
				else
				{
					LastWorldMatrix = Parent.WorldMatrix;
					WorldMatrixForwardScale = Parent.WorldMatrixForwardScale;
				}
				m_worldMatrixDirty = false;
				m_worldMatrixInvDirty = true;
				m_worldMatrixIndex++;
			}
		}

		private void OnParentSet()
		{
			for (int i = 0; i < m_components.Count; i++)
			{
				m_components[i].OnParentSet();
			}
		}

		private void OnParentRemoved()
		{
			for (int i = 0; i < m_components.Count; i++)
			{
				m_components[i].OnParentRemoved();
			}
		}

		private void OnUpdateBeforeDraw()
		{
			for (int i = 0; i < m_components.Count; i++)
			{
				MyActorComponent myActorComponent = m_components[i];
				if (!myActorComponent.NeedsPerFrameUpdate)
				{
					myActorComponent.OnUpdateBeforeDraw();
				}
			}
		}

		private void OnMatrixChange()
		{
			if (m_root)
			{
				foreach (MyActor child in m_children)
				{
					child.SetWorldMatrixDirty();
				}
			}
			else if (Parent != null)
			{
				SetWorldMatrixDirty();
			}
			this.OnMove.InvokeIfNotNull();
			MoveProxy();
		}

		private int AddProxy(ref BoundingBox aabb, MyChildCullTreeData userData, uint flags)
		{
			MyChildCullTreeData myChildCullTreeData = Scene.CompileCullData(userData);
			myChildCullTreeData.Add(m_cullTreeData.All, arg2: true);
			int num = m_cullTreeData.Children.AddProxy(ref aabb, myChildCullTreeData.Add, flags);
			MyBruteCullData myBruteCullData = default(MyBruteCullData);
			myBruteCullData.Aabb = new MyCullAABB(aabb);
			myBruteCullData.UserData = myChildCullTreeData;
			MyBruteCullData myBruteCullData2 = myBruteCullData;
			m_cullTreeData.BruteCull.Add(num, myBruteCullData2);
			for (int i = 0; i < m_cullTreeData.RenderCullData.Length; i++)
			{
				m_cullTreeData.RenderCullData[i].CulledActors.Add(myBruteCullData2);
			}
			MoveProxy();
			return num;
		}

		private void RemoveProxy(int id)
		{
			MyChildCullTreeData userData = m_cullTreeData.BruteCull[id].UserData;
			userData.Remove(m_cullTreeData.All);
			m_cullTreeData.Children.RemoveProxy(id);
			m_cullTreeData.BruteCull.Remove(id);
			Predicate<MyBruteCullData> match = (MyBruteCullData x) => x.UserData == userData;
			for (int i = 0; i < m_cullTreeData.RenderCullData.Length; i++)
			{
				int num = m_cullTreeData.RenderCullData[i].CulledActors.FindIndex(match);
				if (num >= 0)
				{
					m_cullTreeData.RenderCullData[i].CulledActors.RemoveAtFast(num);
					continue;
				}
				num = m_cullTreeData.RenderCullData[i].ActiveActors.FindIndex(match);
				m_cullTreeData.RenderCullData[i].ActiveActors.RemoveAtFast(num);
				userData.Remove(m_cullTreeData.RenderCullData[i].ActiveResults);
			}
			MoveProxy();
		}

		private void MoveProxy(int id, ref BoundingBox aabb)
		{
			m_cullTreeData.Children.MoveProxy(id, ref aabb, Vector3.Zero);
			MyBruteCullData value = m_cullTreeData.BruteCull[id];
			value.Aabb.Reset(ref aabb);
			m_cullTreeData.BruteCull[id] = value;
			Scene.Updater.AddToNextUpdate(this);
			m_dirtyProxy = true;
		}

		private void AddProxy(MyChildCullTreeData data)
		{
			bool flag = m_globalTreeId == -1 && m_globalFarTreeId == -1;
			if (Parent != null)
			{
				if (!flag)
				{
					RemoveProxy();
				}
				if (m_childTreeId == -1)
				{
					BoundingBox aabb = m_localAabb.Value.Transform(m_relativeTransform);
					m_aabb = aabb;
					m_childTreeId = Parent.AddProxy(ref aabb, data, 0u);
				}
			}
			else if (flag)
			{
				m_aabb = m_localAabb.Value.Transform(WorldMatrix);
				if (data.FarCull)
				{
					m_globalFarTreeId = Scene.DynamicRenderablesFarDBVH.AddProxy(ref m_aabb, data.Add, 0u);
				}
				else
				{
					m_globalTreeId = Scene.DynamicRenderablesDBVH.AddProxy(ref m_aabb, data.Add, 0u);
				}
				UpdateVolumeExtent();
			}
		}

		private void RemoveProxy()
		{
			if (Parent != null)
			{
				if (m_childTreeId != -1)
				{
					Parent.RemoveProxy(m_childTreeId);
				}
				m_childTreeId = -1;
			}
			if (m_globalTreeId != -1)
			{
				Scene.DynamicRenderablesDBVH.RemoveProxy(m_globalTreeId);
				m_globalTreeId = -1;
			}
			if (m_globalFarTreeId != -1)
			{
				Scene.DynamicRenderablesFarDBVH.RemoveProxy(m_globalFarTreeId);
				m_globalFarTreeId = -1;
			}
		}

		private void MoveProxy()
		{
			if (!IsVisible)
			{
				return;
			}
			if (m_root)
			{
				Scene.Updater.AddToNextUpdate(this);
				m_dirtyProxy = true;
			}
			else if (Parent != null)
			{
				if (m_childTreeId != -1)
				{
					BoundingBox aabb = m_localAabb.Value.Transform(m_relativeTransform);
					Parent.MoveProxy(m_childTreeId, ref aabb);
					m_aabb = aabb;
				}
			}
			else if (m_globalTreeId != -1)
			{
				m_aabb = m_localAabb.Value.Transform(WorldMatrix);
				Scene.DynamicRenderablesDBVH.MoveProxy(m_globalTreeId, ref m_aabb, Vector3.Zero);
			}
			else if (m_globalFarTreeId != -1)
			{
				m_aabb = m_localAabb.Value.Transform(WorldMatrix);
				Scene.DynamicRenderablesFarDBVH.MoveProxy(m_globalFarTreeId, ref m_aabb, Vector3.Zero);
			}
		}

		private void UpdateProxy()
		{
			if (!m_dirtyProxy || !IsVisible)
			{
				return;
			}
			m_dirtyProxy = false;
			if (m_root)
			{
				m_aabb = ((m_cullTreeData.Children.GetRoot() != -1) ? m_cullTreeData.Children.GetAabb(m_cullTreeData.Children.GetRoot()) : new BoundingBox(Vector3.Zero, Vector3.Zero)).Transform(WorldMatrix);
				if (m_cullTreeId == -1)
				{
					m_cullTreeId = Scene.ManualCullTree.AddProxy(ref m_aabb, m_cullTreeData, 0u);
				}
				else
				{
					Scene.ManualCullTree.MoveProxy(m_cullTreeId, ref m_aabb, Vector3.Zero);
				}
			}
		}

		public void InvalidateCullTreeData()
		{
			RemoveProxy();
			if (!m_visible)
			{
				return;
			}
			bool flag = false;
			MyChildCullTreeData myChildCullTreeData = null;
			for (int i = 0; i < m_components.Count; i++)
			{
				MyChildCullTreeData cullTreeData = m_components[i].GetCullTreeData();
				if (cullTreeData == null)
				{
					continue;
				}
				if (myChildCullTreeData == null)
				{
					myChildCullTreeData = cullTreeData;
					continue;
				}
				if (!flag)
				{
					flag = true;
					myChildCullTreeData = new MyChildCullTreeData
					{
						Add = myChildCullTreeData.Add,
						Remove = myChildCullTreeData.Remove,
						DebugColor = myChildCullTreeData.DebugColor
					};
				}
				myChildCullTreeData.FarCull |= cullTreeData.FarCull;
				MyChildCullTreeData myChildCullTreeData2 = myChildCullTreeData;
				myChildCullTreeData2.Add = (Action<MyCullResultsBase, bool>)Delegate.Combine(myChildCullTreeData2.Add, cullTreeData.Add);
				MyChildCullTreeData myChildCullTreeData3 = myChildCullTreeData;
				myChildCullTreeData3.Remove = (Action<MyCullResultsBase>)Delegate.Combine(myChildCullTreeData3.Remove, cullTreeData.Remove);
				MyChildCullTreeData myChildCullTreeData4 = myChildCullTreeData;
				myChildCullTreeData4.DebugColor = (Func<Color>)Delegate.Combine(myChildCullTreeData4.DebugColor, cullTreeData.DebugColor);
			}
			if (myChildCullTreeData != null)
			{
				AddProxy(myChildCullTreeData);
			}
		}

		public void SetVisibilityUpdates(bool state)
		{
			m_visibilityUpdates = state;
		}

		public void Destroy(bool fadeOut = false)
		{
			bool flag = true;
			if (fadeOut && m_visible)
			{
				for (int i = 0; i < m_components.Count; i++)
				{
					flag &= m_components[i].StartFadeOut();
				}
			}
			if (flag)
			{
				Destruct();
			}
			else
			{
				Scene.Updater.DestroyIn(this, MyTimeSpan.FromSeconds(Scene.FadeOutTime));
			}
		}

		public bool IsOccluded(int viewId)
		{
			if (FrameInView[viewId] == MyScene.FrameCounter - 1)
			{
				return OccludedState[viewId];
			}
			return false;
		}
	}
}
