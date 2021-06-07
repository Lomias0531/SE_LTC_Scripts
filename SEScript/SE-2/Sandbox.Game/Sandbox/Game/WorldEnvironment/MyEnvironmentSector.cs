using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Platform;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment.Definitions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.Library.Collections;
using VRage.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.WorldEnvironment
{
	public class MyEnvironmentSector : MyEntity
	{
		private struct LodHEntry
		{
			public int Lod;

			public bool Set;

			public StackTrace Trace;

			public override string ToString()
			{
				return string.Format("{0} {1} @ {2}", Set ? "Set" : "Requested", Lod, Trace.GetFrame(1));
			}
		}

		private class CompoundInstancedShape
		{
			public HkStaticCompoundShape Shape = new HkStaticCompoundShape(HkReferencePolicy.TakeOwnership);

			private readonly Dictionary<int, int> m_itemToShapeInstance = new Dictionary<int, int>();

			private readonly Dictionary<int, int> m_shapeInstanceToItem = new Dictionary<int, int>();

			private bool m_baked;

			public void AddInstance(int itemId, ref ItemInfo item, HkShape shape)
			{
				if (!shape.IsZero)
				{
					Matrix.CreateFromQuaternion(ref item.Rotation, out Matrix result);
					result.Translation = item.Position;
					int num = Shape.AddInstance(shape, result);
					m_itemToShapeInstance[itemId] = num;
					m_shapeInstanceToItem[num] = itemId;
				}
			}

			public void Bake()
			{
				Shape.Bake();
				m_baked = true;
			}

			public bool TryGetInstance(int itemId, out int shapeInstance)
			{
				return m_itemToShapeInstance.TryGetValue(itemId, out shapeInstance);
			}

			public bool TryGetItemId(int shapeInstance, out int itemId)
			{
				return m_shapeInstanceToItem.TryGetValue(shapeInstance, out itemId);
			}

			public int GetItemId(int shapeInstance)
			{
				return m_shapeInstanceToItem[shapeInstance];
			}
		}

		private class Module
		{
			public readonly IMyEnvironmentModuleProxy Proxy;

			public List<int> Items = new List<int>();

			public MyDefinitionId Definition;

			public Module(IMyEnvironmentModuleProxy proxy)
			{
				Proxy = proxy;
			}
		}

		private class Sandbox_Game_WorldEnvironment_MyEnvironmentSector_003C_003EActor : IActivator, IActivator<MyEnvironmentSector>
		{
			private sealed override object CreateInstance()
			{
				return new MyEnvironmentSector();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyEnvironmentSector CreateInstance()
			{
				return new MyEnvironmentSector();
			}

			MyEnvironmentSector IActivator<MyEnvironmentSector>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private int m_parallelWorksInProgress;

		private MyProceduralEnvironmentDefinition m_environment;

		private MyInstancedRenderSector m_render;

		private IMyEnvironmentOwner m_owner;

		private IMyEnvironmentDataProvider m_provider;

		private MyConcurrentQueue<LodHEntry> m_lodHistory = new MyConcurrentQueue<LodHEntry>();

		private Vector3D m_sectorCenter;

		private BoundingBox2I m_dataRange;

		private Dictionary<int, HkShape> m_modelsToShapes;

		private CompoundInstancedShape m_activeShape;

		private CompoundInstancedShape m_newShape;

		private bool m_togglePhysics;

		private bool m_recalculateShape;

		private int m_lodSwitchedFrom = -1;

		private volatile int m_currentLod = -1;

		private volatile int m_lodToSwitch = -1;

		private List<short> m_modelToItem;

		private readonly Dictionary<Type, Module> m_modules = new Dictionary<Type, Module>();

		private bool m_modulesPendingUpdate;

		private HashSet<MySectorContactEvent> m_contactListeners;

		private int m_hasParallelWorkPending;

		public Vector3D SectorCenter
		{
			get
			{
				return m_sectorCenter;
			}
			private set
			{
				m_sectorCenter = value;
			}
		}

		public Vector3D[] Bounds
		{
			get;
			private set;
		}

		public MyWorldEnvironmentDefinition EnvironmentDefinition
		{
			get;
			private set;
		}

		public bool IsLoaded => true;

		public bool IsClosed
		{
			get;
			private set;
		}

		public int LodLevel => m_currentLod;

		public bool HasPhysics
		{
			get;
			private set;
		}

		public bool IsPinned
		{
			get;
			internal set;
		}

		public bool IsPendingLodSwitch => m_currentLod != m_lodToSwitch;

		public bool IsPendingPhysicsToggle => m_togglePhysics;

		public bool HasSerialWorkPending
		{
			get;
			private set;
		}

		public bool HasParallelWorkPending
		{
			get
			{
				return Interlocked.CompareExchange(ref m_hasParallelWorkPending, 0, 0) == 1;
			}
			private set
			{
				Interlocked.Exchange(ref m_hasParallelWorkPending, value ? 1 : 0);
			}
		}

		public bool HasParallelWorkInProgress => Volatile.Read(ref m_parallelWorksInProgress) > 0;

		public long SectorId
		{
			get;
			private set;
		}

		public MyEnvironmentDataView DataView
		{
			get;
			private set;
		}

		public IMyEnvironmentOwner Owner => m_owner;

		public event Action OnPhysicsClose;

		public event MySectorContactEvent OnContactPoint
		{
			add
			{
				if (m_contactListeners == null)
				{
					m_contactListeners = new HashSet<MySectorContactEvent>();
				}
				if (m_contactListeners.Count == 0 && base.Physics != null && base.Physics.RigidBody != null)
				{
					base.Physics.RigidBody.ContactPointCallbackEnabled = true;
				}
				m_contactListeners.Add(value);
			}
			remove
			{
				if (m_contactListeners != null)
				{
					m_contactListeners.Remove(value);
					if (m_contactListeners.Count == 0 && base.Physics != null && base.Physics.RigidBody != null)
					{
						base.Physics.RigidBody.ContactPointCallbackEnabled = false;
					}
				}
			}
		}

		public event Action<MyEnvironmentSector, int> OnLodCommit;

		public event Action<MyEnvironmentSector, bool> OnPhysicsCommit;

		public void Init(IMyEnvironmentOwner owner, ref MyEnvironmentSectorParameters parameters)
		{
			SectorCenter = parameters.Center;
			Bounds = parameters.Bounds;
			m_dataRange = parameters.DataRange;
			m_environment = (MyProceduralEnvironmentDefinition)parameters.Environment;
			EnvironmentDefinition = parameters.Environment;
			m_owner = owner;
			m_provider = parameters.Provider;
			Vector3D center = parameters.Center;
			owner.ProjectPointToSurface(ref center);
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				m_render = new MyInstancedRenderSector($"{owner}:Sector(0x{parameters.SectorId:X})", MatrixD.CreateTranslation(center));
			}
			SectorId = parameters.SectorId;
			BoundingBoxD worldAABB = BoundingBoxD.CreateInvalid();
			for (int i = 0; i < 8; i++)
			{
				worldAABB.Include(Bounds[i]);
			}
			base.PositionComp.SetPosition(parameters.Center);
			base.PositionComp.WorldAABB = worldAABB;
			AddDebugRenderComponent(new MyDebugRenderComponentEnvironmentSector(this));
			base.GameLogic = new MyNullGameLogicComponent();
			base.Save = false;
			IsClosed = false;
		}

		protected override void Closing()
		{
			CloseInternal(entityClosing: true);
		}

		public new void Close()
		{
			CloseInternal(entityClosing: false);
		}

		private void CloseInternal(bool entityClosing)
		{
			if (m_render != null)
			{
				m_render.DetachEnvironment(this);
			}
			if (DataView != null)
			{
				DataView.Close();
				DataView = null;
			}
			foreach (Module value in m_modules.Values)
			{
				value.Proxy.Close();
			}
			HasPhysics = false;
			m_currentLod = -1;
			base.Close();
			IsClosed = true;
		}

		public void SetLod(int lod)
		{
			if (!base.Closed && (lod != m_currentLod || lod != m_lodToSwitch))
			{
				if (Interlocked.Exchange(ref m_hasParallelWorkPending, 1) == 0)
				{
					Owner.ScheduleWork(this, parallel: true);
				}
				m_lodToSwitch = lod;
				if (m_render != null)
				{
					m_render.Lod = m_lodToSwitch;
				}
			}
		}

		public void EnablePhysics(bool physics)
		{
			if (base.Closed)
			{
				return;
			}
			bool flag = HasPhysics != physics;
			if (flag != m_togglePhysics && flag)
			{
				if (m_activeShape == null || m_recalculateShape)
				{
					if (Interlocked.Exchange(ref m_hasParallelWorkPending, 1) == 0)
					{
						Owner.ScheduleWork(this, parallel: true);
					}
				}
				else
				{
					if (base.Physics != null)
					{
						base.Physics.Enabled = physics;
					}
					flag = false;
					HasPhysics = physics;
					if (!physics)
					{
						this.OnPhysicsClose?.Invoke();
					}
				}
			}
			m_togglePhysics = flag;
		}

		public void CancelParallel()
		{
		}

		public void DoParallelWork()
		{
			try
			{
				if (Interlocked.Increment(ref m_parallelWorksInProgress) <= 1 && Interlocked.Exchange(ref m_hasParallelWorkPending, 0) == 1)
				{
					HasParallelWorkPending = false;
					if (base.Closed)
					{
						m_lodToSwitch = m_currentLod;
						m_togglePhysics = false;
					}
					else
					{
						bool flag = false;
						if (m_lodToSwitch != m_currentLod)
						{
							flag = true;
							if (m_lodToSwitch == -1)
							{
								m_render.Close();
							}
							else
							{
								FetchData(m_lodToSwitch);
								BuildInstanceBuffers(m_lodToSwitch);
							}
							m_lodSwitchedFrom = m_currentLod;
						}
						if ((m_togglePhysics && !HasPhysics) || (HasPhysics && m_recalculateShape))
						{
							flag = true;
							BuildShape();
						}
						HasSerialWorkPending = true;
						if (flag)
						{
							Owner.ScheduleWork(this, parallel: false);
						}
					}
				}
			}
			finally
			{
				Interlocked.Decrement(ref m_parallelWorksInProgress);
			}
		}

		public bool DoSerialWork()
		{
			if (base.Closed)
			{
				return false;
			}
			if (HasParallelWorkPending)
			{
				return false;
			}
			bool result = false;
			if (m_togglePhysics || m_lodSwitchedFrom != m_lodToSwitch)
			{
				foreach (KeyValuePair<Type, Module> module in m_modules)
				{
					if (m_lodSwitchedFrom != m_lodToSwitch)
					{
						module.Value.Proxy.CommitLodChange(m_lodSwitchedFrom, m_lodToSwitch);
					}
					if (m_togglePhysics)
					{
						module.Value.Proxy.CommitPhysicsChange(!HasPhysics);
					}
				}
				result = true;
			}
			m_currentLod = m_lodToSwitch;
			if (m_lodSwitchedFrom != m_currentLod && m_lodToSwitch == m_currentLod)
			{
				RaiseOnLodCommitEvent(m_currentLod);
			}
			if (m_togglePhysics)
			{
				RaiseOnPhysicsCommitEvent(HasPhysics);
			}
			if (m_render != null && m_render.HasChanges() && m_lodToSwitch == m_currentLod)
			{
				m_render.CommitChangesToRenderer();
				result = true;
				m_lodSwitchedFrom = m_currentLod;
			}
			if (m_togglePhysics)
			{
				if (HasPhysics)
				{
					base.Physics.Enabled = false;
					HasPhysics = false;
					m_togglePhysics = false;
				}
				else if (m_newShape != null)
				{
					PreparePhysicsBody();
					result = true;
					HasPhysics = true;
					m_togglePhysics = false;
				}
			}
			if (m_recalculateShape)
			{
				m_recalculateShape = false;
				if (HasPhysics && m_newShape != null)
				{
					PreparePhysicsBody();
				}
			}
			HasSerialWorkPending = false;
			return result;
		}

		public void OnItemChange(int index, short newModelIndex)
		{
			if (m_currentLod != -1 || HasPhysics)
			{
				foreach (Module value in m_modules.Values)
				{
					value.Proxy.OnItemChange(index, newModelIndex);
				}
				if (m_currentLod != -1)
				{
					UpdateItemModel(index, newModelIndex);
					m_render.CommitChangesToRenderer();
				}
				if (HasPhysics)
				{
					UpdateItemShape(index, newModelIndex);
				}
				else if (newModelIndex >= 0)
				{
					m_recalculateShape = true;
				}
			}
		}

		public void OnItemsChange(int sector, List<int> indices, short newModelIndex)
		{
			if (m_currentLod == -1 && !HasPhysics)
			{
				return;
			}
			int num = DataView.SectorOffsets[sector];
			int num2 = ((sector < DataView.SectorOffsets.Count - 1) ? DataView.SectorOffsets[sector + 1] : DataView.Items.Count) - num;
			for (int i = 0; i < indices.Count; i++)
			{
				if (indices[i] >= num2)
				{
					indices.RemoveAtFast(i);
					i--;
				}
			}
			foreach (Module value in m_modules.Values)
			{
				value.Proxy.OnItemChangeBatch(indices, num, newModelIndex);
			}
			if (m_currentLod != -1)
			{
				foreach (int index3 in indices)
				{
					int index = index3 + num;
					UpdateItemModel(index, newModelIndex);
				}
				m_render.CommitChangesToRenderer();
			}
			if (HasPhysics)
			{
				foreach (int index4 in indices)
				{
					int index2 = index4 + num;
					UpdateItemShape(index2, newModelIndex);
				}
			}
			else if (newModelIndex > 0)
			{
				m_recalculateShape = true;
			}
		}

		[Conditional("DEBUG")]
		private void RecordHistory(int lod, bool set)
		{
			if (m_lodHistory.Count > 10)
			{
				m_lodHistory.Dequeue();
			}
			m_lodHistory.Enqueue(new LodHEntry
			{
				Lod = lod,
				Set = set,
				Trace = new StackTrace()
			});
		}

		private unsafe void FetchData(int lodToSwitch)
		{
			MyEnvironmentDataView dataView = DataView;
			if (dataView == null || dataView.Lod != lodToSwitch)
			{
				DataView = m_provider.GetItemView(lodToSwitch, ref m_dataRange.Min, ref m_dataRange.Max, ref m_sectorCenter);
				DataView.Listener = this;
				dataView?.Close();
				foreach (Module value2 in m_modules.Values)
				{
					value2.Proxy.Close();
				}
				m_modules.Clear();
				int count = DataView.Items.Count;
				fixed (ItemInfo* ptr = DataView.Items.GetInternalArray())
				{
					for (int i = 0; i < count; i++)
					{
						if (ptr[i].DefinitionIndex != -1)
						{
							MyItemTypeDefinition.Module[] proxyModules = m_environment.Items[ptr[i].DefinitionIndex].Type.ProxyModules;
							if (proxyModules != null)
							{
								MyItemTypeDefinition.Module[] array = proxyModules;
								for (int j = 0; j < array.Length; j++)
								{
									MyItemTypeDefinition.Module module = array[j];
									if (!m_modules.TryGetValue(module.Type, out Module value))
									{
										value = new Module((IMyEnvironmentModuleProxy)Activator.CreateInstance(module.Type));
										value.Definition = module.Definition;
										m_modules[module.Type] = value;
									}
									value.Items.Add(i);
								}
							}
						}
					}
				}
				foreach (KeyValuePair<Type, Module> module2 in m_modules)
				{
					module2.Value.Proxy.Init(this, module2.Value.Items);
					module2.Value.Items = null;
				}
			}
		}

		private unsafe void BuildShape()
		{
			FetchData(0);
			CompoundInstancedShape compoundInstancedShape = new CompoundInstancedShape();
			if (m_modelsToShapes == null)
			{
				m_modelsToShapes = new Dictionary<int, HkShape>();
			}
			int count = DataView.Items.Count;
			fixed (ItemInfo* ptr = DataView.Items.GetInternalArray())
			{
				for (int i = 0; i < count; i++)
				{
					short modelIndex = ptr[i].ModelIndex;
					if (modelIndex >= 0 && Owner.GetModelForId(modelIndex) != null)
					{
						if (!m_modelsToShapes.TryGetValue(modelIndex, out HkShape value))
						{
							MyModel modelOnlyData = MyModels.GetModelOnlyData(Owner.GetModelForId(modelIndex).Model);
							HkShape[] havokCollisionShapes = modelOnlyData.HavokCollisionShapes;
							if (havokCollisionShapes != null)
							{
								if (havokCollisionShapes.Length != 0)
								{
									value = ((havokCollisionShapes.Length != 1) ? ((HkShape)new HkListShape(havokCollisionShapes, HkReferencePolicy.TakeOwnership)) : havokCollisionShapes[0]);
								}
								else
								{
									MyLog.Default.Warning("Model {0} has an empty list of shapes, something wrong with export?", modelOnlyData.AssetName);
								}
							}
							m_modelsToShapes[modelIndex] = value;
						}
						compoundInstancedShape.AddInstance(i, ref ptr[i], value);
					}
				}
			}
			compoundInstancedShape.Bake();
			m_newShape = compoundInstancedShape;
		}

		private void UpdateItemShape(int index, short newModelIndex)
		{
			if (m_activeShape != null && m_activeShape.TryGetInstance(index, out int shapeInstance))
			{
				m_activeShape.Shape.EnableInstance(shapeInstance, newModelIndex >= 0);
			}
			else if (!m_recalculateShape)
			{
				m_recalculateShape = true;
				if (Interlocked.Exchange(ref m_hasParallelWorkPending, 1) == 0)
				{
					Owner.ScheduleWork(this, parallel: true);
				}
			}
		}

		private void PreparePhysicsBody()
		{
			m_activeShape = m_newShape;
			m_newShape = null;
			if (base.Physics != null)
			{
				base.Physics.Close();
			}
			base.Physics = new MyPhysicsBody(this, RigidBodyFlag.RBF_STATIC);
			MyPhysicsBody obj = (MyPhysicsBody)base.Physics;
			obj.CreateFromCollisionObject(m_activeShape.Shape, Vector3.Zero, base.PositionComp.WorldMatrix, default(HkMassProperties));
			obj.ContactPointCallback += Physics_onContactPoint;
			obj.IsStaticForCluster = true;
			if (m_contactListeners != null && m_contactListeners.Count != 0)
			{
				base.Physics.RigidBody.ContactPointCallbackEnabled = true;
			}
			base.Physics.Enabled = true;
		}

		private void Physics_onContactPoint(ref MyPhysics.MyContactPointEvent evt)
		{
			MyPhysicsBody physicsBody = evt.ContactPointEvent.GetPhysicsBody(0);
			if (physicsBody == null)
			{
				return;
			}
			int num = (physicsBody.Entity != this) ? 1 : 0;
			uint shapeKey = evt.ContactPointEvent.GetShapeKey(num);
			if (shapeKey != uint.MaxValue)
			{
				MyPhysicsBody physicsBody2 = evt.ContactPointEvent.GetPhysicsBody(1 ^ num);
				if (physicsBody2 != null)
				{
					IMyEntity entity = physicsBody2.Entity;
					int itemFromShapeKey = GetItemFromShapeKey(shapeKey);
					foreach (MySectorContactEvent contactListener in m_contactListeners)
					{
						contactListener(itemFromShapeKey, (MyEntity)entity, ref evt);
					}
				}
			}
		}

		public void BuildInstanceBuffers(int lod)
		{
			Dictionary<short, MyList<MyInstanceData>> dictionary = new Dictionary<short, MyList<MyInstanceData>>();
			m_modelToItem = new List<short>(DataView.Items.Count);
			Vector3D value = SectorCenter - m_render.WorldMatrix.Translation;
			int count = DataView.Items.Count;
			ItemInfo[] internalArray = DataView.Items.GetInternalArray();
			for (int i = 0; i < count; i++)
			{
				if (internalArray[i].ModelIndex < 0)
				{
					m_modelToItem.Add(-1);
					continue;
				}
				if (!dictionary.TryGetValue(internalArray[i].ModelIndex, out MyList<MyInstanceData> value2))
				{
					value2 = new MyList<MyInstanceData>();
					dictionary[internalArray[i].ModelIndex] = value2;
				}
				Matrix.CreateFromQuaternion(ref internalArray[i].Rotation, out Matrix result);
				result.Translation = internalArray[i].Position + value;
				m_modelToItem.Add((short)value2.Count);
				value2.Add(new MyInstanceData(result));
			}
			foreach (KeyValuePair<short, MyList<MyInstanceData>> item in dictionary)
			{
				MyPhysicalModelDefinition modelForId = m_owner.GetModelForId(item.Key);
				if (modelForId != null)
				{
					int id = MyModel.GetId(modelForId.Model);
					m_render.AddInstances(id, item.Value);
				}
			}
		}

		private void UpdateItemModel(int index, short newModelIndex)
		{
			ItemInfo value = DataView.Items[index];
			if (value.ModelIndex == newModelIndex)
			{
				return;
			}
			if (m_currentLod == m_lodToSwitch)
			{
				if (value.ModelIndex >= 0 && m_owner.GetModelForId(value.ModelIndex) != null)
				{
					int id = MyModel.GetId(m_owner.GetModelForId(value.ModelIndex).Model);
					m_render.RemoveInstance(id, m_modelToItem[index]);
					m_modelToItem[index] = -1;
				}
				if (newModelIndex >= 0 && m_owner.GetModelForId(newModelIndex) != null)
				{
					int id2 = MyModel.GetId(m_owner.GetModelForId(newModelIndex).Model);
					Vector3D value2 = SectorCenter - m_render.WorldMatrix.Translation;
					Matrix.CreateFromQuaternion(ref value.Rotation, out Matrix result);
					result.Translation = value.Position + value2;
					MyInstanceData data = new MyInstanceData(result);
					m_modelToItem[index] = m_render.AddInstance(id2, ref data);
				}
			}
			value.ModelIndex = newModelIndex;
			DataView.Items[index] = value;
		}

		public void GetItemInfo(int itemId, out uint renderObjectId, out int instanceIndex)
		{
			ItemInfo itemInfo = DataView.Items[itemId];
			int id = MyModel.GetId(m_owner.GetModelForId(itemInfo.ModelIndex).Model);
			renderObjectId = m_render.GetRenderEntity(id);
			instanceIndex = m_modelToItem[itemId];
		}

		public void EnableItem(int itemId, bool enabled)
		{
			DataView.GetLogicalSector(itemId, out int logicalItem, out MyLogicalEnvironmentSectorBase sector);
			sector.EnableItem(logicalItem, enabled);
		}

		public int GetItemFromShapeKey(uint shapekey)
		{
			m_activeShape.Shape.DecomposeShapeKey(shapekey, out int instanceId, out uint _);
			return m_activeShape.GetItemId(instanceId);
		}

		public T GetModuleForDefinition<T>(MyRuntimeEnvironmentItemInfo itemEnvDefinition) where T : class, IMyEnvironmentModuleProxy
		{
			MyItemTypeDefinition.Module[] proxyModules = itemEnvDefinition.Type.ProxyModules;
			if (proxyModules == null || !proxyModules.Any((MyItemTypeDefinition.Module x) => typeof(T).IsAssignableFrom(x.Type)))
			{
				return null;
			}
			m_modules.TryGetValue(typeof(T), out Module value);
			return (T)(value?.Proxy);
		}

		public T GetModule<T>() where T : class, IMyEnvironmentModuleProxy
		{
			m_modules.TryGetValue(typeof(T), out Module value);
			return (T)(value?.Proxy);
		}

		public IMyEnvironmentModuleProxy GetModule(Type moduleType)
		{
			m_modules.TryGetValue(moduleType, out Module value);
			return value?.Proxy;
		}

		public void RaiseItemEvent<TModule>(TModule module, int item, bool fromClient = false) where TModule : IMyEnvironmentModuleProxy
		{
			RaiseItemEvent<TModule, object>(module, item, null, fromClient);
		}

		public void RaiseItemEvent<TModule, TArgument>(TModule module, int item, TArgument eventData, bool fromClient = false) where TModule : IMyEnvironmentModuleProxy
		{
			MyDefinitionId modDef = m_modules[typeof(TModule)].Definition;
			DataView.GetLogicalSector(item, out int logicalItem, out MyLogicalEnvironmentSectorBase sector);
			sector.RaiseItemEvent(logicalItem, ref modDef, eventData, fromClient);
		}

		public new void DebugDraw()
		{
			if (LodLevel < 0 && !HasPhysics)
			{
				return;
			}
			Color color = Color.Red;
			if (MyPlanetEnvironmentSessionComponent.ActiveSector == this)
			{
				color = Color.LimeGreen;
				if (DataView != null)
				{
					if (MyPlanetEnvironmentSessionComponent.DebugDrawActiveSectorItems)
					{
						for (int i = 0; i < DataView.Items.Count; i++)
						{
							ItemInfo itemInfo = DataView.Items[i];
							Vector3D worldCoord = itemInfo.Position + SectorCenter;
							Owner.GetDefinition((ushort)itemInfo.DefinitionIndex, out MyRuntimeEnvironmentItemInfo def);
							MyRenderProxy.DebugDrawText3D(worldCoord, $"{def.Type.Name} i{i} m{itemInfo.ModelIndex} d{itemInfo.DefinitionIndex}", color, 0.7f, depthRead: true);
						}
					}
					if (MyPlanetEnvironmentSessionComponent.DebugDrawActiveSectorProvider)
					{
						foreach (MyLogicalEnvironmentSectorBase logicalSector in DataView.LogicalSectors)
						{
							logicalSector.DebugDraw(DataView.Lod);
						}
					}
				}
			}
			else if (HasPhysics && LodLevel == -1)
			{
				color = Color.RoyalBlue;
			}
			Vector3D vector3D = (Bounds[4] + Bounds[7]) / 2.0;
			if (MyPlanetEnvironmentSessionComponent.ActiveSector == this || Vector3D.DistanceSquared(vector3D, MySector.MainCamera.Position) < (double)(MyPlanetEnvironmentSessionComponent.DebugDrawDistance * MyPlanetEnvironmentSessionComponent.DebugDrawDistance))
			{
				string text = ToString();
				MyRenderProxy.DebugDrawText3D(vector3D, text, color, 1f, depthRead: true);
			}
			MyRenderProxy.DebugDraw6FaceConvex(Bounds, color, 1f, depthRead: true, fill: false);
		}

		public override string ToString()
		{
			long sectorId = SectorId;
			int num = (int)(sectorId & 0xFFFFFF);
			long num2 = sectorId >> 24;
			int num3 = (int)(num2 & 0xFFFFFF);
			long num4 = num2 >> 24;
			int num5 = (int)(num4 & 7);
			int num6 = (int)((num4 >> 3) & 0xFF);
			return string.Format("S(x{0} y{1} f{2} l{3}({4}) c{6} {5})", num, num3, num5, num6, LodLevel, HasPhysics ? " p" : "", (DataView != null) ? DataView.Items.Count : 0);
		}

		public override int GetHashCode()
		{
			return SectorId.GetHashCode();
		}

		public void RaiseOnLodCommitEvent(int lod)
		{
			if (this.OnLodCommit != null)
			{
				this.OnLodCommit(this, lod);
			}
		}

		public void RaiseOnPhysicsCommitEvent(bool enabled)
		{
			if (this.OnPhysicsCommit != null)
			{
				this.OnPhysicsCommit(this, enabled);
			}
		}

		public short GetModelIndex(int itemId)
		{
			DataView.GetLogicalSector(itemId, out int logicalItem, out MyLogicalEnvironmentSectorBase sector);
			sector.GetItem(logicalItem, out ItemInfo item);
			return item.ModelIndex;
		}
	}
}
