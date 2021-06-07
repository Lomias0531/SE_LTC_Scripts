using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_Collector))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyCollector),
		typeof(Sandbox.ModAPI.Ingame.IMyCollector)
	})]
	public class MyCollector : MyFunctionalBlock, IMyConveyorEndpointBlock, Sandbox.ModAPI.IMyCollector, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyCollector, IMyInventoryOwner
	{
		protected sealed class PlayActionSoundAndParticle_003C_003EVRageMath_Vector3D : ICallSite<MyCollector, Vector3D, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCollector @this, in Vector3D position, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.PlayActionSoundAndParticle(position);
			}
		}

		protected class m_useConveyorSystem_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType useConveyorSystem;
				ISyncType result = useConveyorSystem = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyCollector)P_0).m_useConveyorSystem = (Sync<bool, SyncDirection.BothWays>)useConveyorSystem;
				return result;
			}
		}

		private class Sandbox_Game_Entities_Blocks_MyCollector_003C_003EActor : IActivator, IActivator<MyCollector>
		{
			private sealed override object CreateInstance()
			{
				return new MyCollector();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyCollector CreateInstance()
			{
				return new MyCollector();
			}

			MyCollector IActivator<MyCollector>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private HkConstraint m_phantomConstraint;

		private Sync<bool, SyncDirection.BothWays> m_useConveyorSystem;

		private MyMultilineConveyorEndpoint m_multilineConveyorEndpoint;

		private bool m_isCollecting;

		private readonly MyConcurrentHashSet<MyFloatingObject> m_entitiesToTake = new MyConcurrentHashSet<MyFloatingObject>();

		public new MyPoweredCargoContainerDefinition BlockDefinition => SlimBlock.BlockDefinition as MyPoweredCargoContainerDefinition;

		private bool ShouldHavePhantom
		{
			get
			{
				if (base.CubeGrid.CreatePhysics && !base.CubeGrid.IsPreview)
				{
					return Sync.IsServer;
				}
				return false;
			}
		}

		public IMyConveyorEndpoint ConveyorEndpoint => m_multilineConveyorEndpoint;

		private bool UseConveyorSystem
		{
			get
			{
				return m_useConveyorSystem;
			}
			set
			{
				m_useConveyorSystem.Value = value;
			}
		}

		bool Sandbox.ModAPI.Ingame.IMyCollector.UseConveyorSystem
		{
			get
			{
				return m_useConveyorSystem;
			}
			set
			{
				m_useConveyorSystem.Value = value;
			}
		}

		int IMyInventoryOwner.InventoryCount => base.InventoryCount;

		long IMyInventoryOwner.EntityId => base.EntityId;

		bool IMyInventoryOwner.HasInventory => base.HasInventory;

		bool IMyInventoryOwner.UseConveyorSystem
		{
			get
			{
				return UseConveyorSystem;
			}
			set
			{
				UseConveyorSystem = value;
			}
		}

		public MyCollector()
		{
			CreateTerminalControls();
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyCollector>())
			{
				base.CreateTerminalControls();
				MyTerminalControlOnOffSwitch<MyCollector> obj = new MyTerminalControlOnOffSwitch<MyCollector>("UseConveyor", MySpaceTexts.Terminal_UseConveyorSystem)
				{
					Getter = ((MyCollector x) => x.UseConveyorSystem),
					Setter = delegate(MyCollector x, bool v)
					{
						x.UseConveyorSystem = v;
					}
				};
				obj.EnableToggleAction();
				MyTerminalControlFactory.AddControl(obj);
			}
		}

		protected override bool CheckIsWorking()
		{
			if (base.ResourceSink == null)
			{
				return false;
			}
			if (base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				return base.CheckIsWorking();
			}
			return false;
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			MyObjectBuilder_Collector myObjectBuilder_Collector = objectBuilder as MyObjectBuilder_Collector;
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(MyStringHash.GetOrCompute(BlockDefinition.ResourceSinkGroup), BlockDefinition.RequiredPowerInput, ComputeRequiredPower);
			base.ResourceSink = myResourceSinkComponent;
			base.Init(objectBuilder, cubeGrid);
			if (MyFakes.ENABLE_INVENTORY_FIX)
			{
				FixSingleInventory();
			}
			MyInventory inventory = this.GetInventory();
			if (inventory == null)
			{
				inventory = new MyInventory(BlockDefinition.InventorySize.Volume, BlockDefinition.InventorySize, MyInventoryFlags.CanSend);
				base.Components.Add((MyInventoryBase)inventory);
				inventory.Init(myObjectBuilder_Collector.Inventory);
			}
			if (Sync.IsServer && base.CubeGrid.CreatePhysics)
			{
				LoadDummies();
			}
			base.ResourceSink.IsPoweredChanged += Receiver_IsPoweredChanged;
			SlimBlock.ComponentStack.IsFunctionalChanged += UpdateReceiver;
			base.EnabledChanged += UpdateReceiver;
			m_useConveyorSystem.SetLocalValue(myObjectBuilder_Collector.UseConveyorSystem);
			base.ResourceSink.Update();
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			if (m_phantomConstraint == null && ShouldHavePhantom)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			}
		}

		public override void OnRemovedFromScene(object source)
		{
			DisposePhantomContraint();
			base.OnRemovedFromScene(source);
		}

		protected float ComputeRequiredPower()
		{
			if (!base.Enabled || !base.IsFunctional)
			{
				return 0f;
			}
			return BlockDefinition.RequiredPowerInput;
		}

		private void UpdateReceiver(MyTerminalBlock block)
		{
			base.ResourceSink.Update();
		}

		private void UpdateReceiver()
		{
			base.ResourceSink.Update();
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_Collector obj = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_Collector;
			obj.UseConveyorSystem = m_useConveyorSystem;
			return obj;
		}

		public override void UpdateBeforeSimulation100()
		{
			base.UpdateBeforeSimulation100();
			if (Sync.IsServer && base.IsWorking && (bool)m_useConveyorSystem && this.GetInventory().GetItems().Count > 0)
			{
				MyGridConveyorSystem.PushAnyRequest(this, this.GetInventory(), base.OwnerId);
			}
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			if (!Sync.IsServer)
			{
				if (m_entitiesToTake.Count > 0)
				{
					m_entitiesToTake.Clear();
				}
				return;
			}
			if (m_phantomConstraint == null && ShouldHavePhantom)
			{
				CreatePhantomConstraint();
			}
			if (!base.Enabled || !base.IsWorking)
			{
				return;
			}
			bool flag = false;
			m_isCollecting = true;
			foreach (MyFloatingObject item in m_entitiesToTake)
			{
				this.GetInventory().TakeFloatingObject(item);
				flag = true;
			}
			m_isCollecting = false;
			if (flag)
			{
				Vector3D position = m_entitiesToTake.ElementAt(0).PositionComp.GetPosition();
				MyParticlesManager.TryCreateParticleEffect("Smoke_Collector", MatrixD.CreateWorld(position, base.WorldMatrix.Down, base.WorldMatrix.Forward), out MyParticleEffect _);
				if (m_soundEmitter != null)
				{
					m_soundEmitter.PlaySound(m_actionSound);
				}
				MyMultiplayer.RaiseEvent(this, (MyCollector x) => x.PlayActionSoundAndParticle, position);
			}
		}

		[Event(null, 217)]
		[Reliable]
		[Broadcast]
		private void PlayActionSoundAndParticle(Vector3D position)
		{
			MyParticlesManager.TryCreateParticleEffect("Smoke_Collector", MatrixD.CreateWorld(position, base.WorldMatrix.Down, base.WorldMatrix.Forward), out MyParticleEffect _);
			if (m_soundEmitter != null)
			{
				m_soundEmitter.PlaySound(m_actionSound);
			}
		}

		private void Receiver_IsPoweredChanged()
		{
			UpdateIsWorking();
		}

		protected override void OnStartWorking()
		{
			base.OnStartWorking();
			if (ShouldHavePhantom)
			{
				MyPhysicsBody physics = base.Physics;
				if (physics != null && !physics.Enabled)
				{
					physics.Enabled = true;
					CreatePhantomConstraint();
				}
			}
		}

		protected override void OnStopWorking()
		{
			base.OnStopWorking();
			if (base.Physics != null)
			{
				DisposePhantomContraint();
				base.Physics.Enabled = false;
			}
		}

		public override void OnDestroy()
		{
			ReleaseInventory(this.GetInventory());
			base.OnDestroy();
		}

		public override void OnRemovedByCubeBuilder()
		{
			ReleaseInventory(this.GetInventory());
			base.OnRemovedByCubeBuilder();
		}

		private void LoadDummies()
		{
			foreach (KeyValuePair<string, MyModelDummy> dummy in MyModels.GetModelOnlyDummies(BlockDefinition.Model).Dummies)
			{
				if (dummy.Key.ToLower().Contains("collector"))
				{
					Matrix matrix = dummy.Value.Matrix;
					GetBoxFromMatrix(matrix, out Vector3 halfExtents, out Vector3 _, out Quaternion _);
					HkBvShape shape = CreateFieldShape(halfExtents);
					base.Physics = new MyPhysicsBody(this, RigidBodyFlag.RBF_UNLOCKED_SPEEDS);
					base.Physics.IsPhantom = true;
					base.Physics.CreateFromCollisionObject(shape, matrix.Translation, base.WorldMatrix, null, 26);
					base.Physics.Enabled = true;
					base.Physics.RigidBody.ContactPointCallbackEnabled = false;
					shape.Base.RemoveReference();
					break;
				}
			}
		}

		private void Inventory_ContentChangedCallback(MyInventoryBase inventory)
		{
			if (Sync.IsServer)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			}
		}

		private HkBvShape CreateFieldShape(Vector3 extents)
		{
			return new HkBvShape(childShape: new HkPhantomCallbackShape(phantom_Enter, phantom_Leave), boundingVolumeShape: new HkBoxShape(extents), policy: HkReferencePolicy.TakeOwnership);
		}

		private void phantom_Leave(HkPhantomCallbackShape shape, HkRigidBody body)
		{
			if (Sync.IsServer && !m_isCollecting)
			{
				List<VRage.ModAPI.IMyEntity> allEntities = body.GetAllEntities();
				foreach (VRage.ModAPI.IMyEntity item in allEntities)
				{
					m_entitiesToTake.Remove(item as MyFloatingObject);
				}
				allEntities.Clear();
			}
		}

		private void phantom_Enter(HkPhantomCallbackShape shape, HkRigidBody body)
		{
			if (Sync.IsServer)
			{
				List<VRage.ModAPI.IMyEntity> allEntities = body.GetAllEntities();
				foreach (VRage.ModAPI.IMyEntity item in allEntities)
				{
					if (item is MyFloatingObject)
					{
						m_entitiesToTake.Add(item as MyFloatingObject);
						MySandboxGame.Static.Invoke(delegate
						{
							base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
						}, "MyCollector::NeedsUpdate");
					}
				}
				allEntities.Clear();
			}
		}

		private void RigidBody_ContactPointCallback(ref HkContactPointEvent value)
		{
			if (Sync.IsServer)
			{
				VRage.ModAPI.IMyEntity otherEntity = value.GetOtherEntity(this);
				if (otherEntity is MyFloatingObject)
				{
					m_entitiesToTake.Add(otherEntity as MyFloatingObject);
					MySandboxGame.Static.Invoke(delegate
					{
						base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
					}, "MyCollector::NeedsUpdate");
				}
			}
		}

		private void GetBoxFromMatrix(Matrix m, out Vector3 halfExtents, out Vector3 position, out Quaternion orientation)
		{
			MatrixD matrix = Matrix.Normalize(m) * base.WorldMatrix;
			orientation = Quaternion.CreateFromRotationMatrix(matrix);
			halfExtents = Vector3.Abs(m.Scale) / 2f;
			halfExtents = new Vector3(halfExtents.X, halfExtents.Y, halfExtents.Z);
			position = matrix.Translation;
		}

		public override void OnCubeGridChanged(MyCubeGrid oldGrid)
		{
			base.OnCubeGridChanged(oldGrid);
			DisposePhantomContraint(oldGrid);
			if (ShouldHavePhantom)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			}
		}

		private void CreatePhantomConstraint()
		{
			if (m_phantomConstraint != null)
			{
				DisposePhantomContraint();
			}
			MyGridPhysics physics = base.CubeGrid.Physics;
			MyPhysicsBody physics2 = base.Physics;
			if (physics != null && physics2 != null && physics2.Enabled)
			{
				Matrix matrix = Matrix.CreateTranslation(-physics2.Center);
				Matrix localMatrix = base.PositionComp.LocalMatrix;
				Matrix pivotB = matrix;
				HkFixedConstraintData hkFixedConstraintData = new HkFixedConstraintData();
				hkFixedConstraintData.SetInBodySpace(localMatrix, pivotB, physics, physics2);
				m_phantomConstraint = new HkConstraint(physics.RigidBody, physics2.RigidBody, hkFixedConstraintData);
				physics.AddConstraint(m_phantomConstraint);
			}
		}

		private void DisposePhantomContraint(MyCubeGrid oldGrid = null)
		{
			if (!(m_phantomConstraint == null))
			{
				if (oldGrid == null)
				{
					oldGrid = base.CubeGrid;
				}
				oldGrid.Physics.RemoveConstraint(m_phantomConstraint);
				m_phantomConstraint.Dispose();
				m_phantomConstraint = null;
			}
		}

		protected override void OnInventoryComponentAdded(MyInventoryBase inventory)
		{
			base.OnInventoryComponentAdded(inventory);
			if (this.GetInventory() != null)
			{
				this.GetInventory().ContentsChanged += Inventory_ContentChangedCallback;
			}
		}

		protected override void OnInventoryComponentRemoved(MyInventoryBase inventory)
		{
			base.OnInventoryComponentRemoved(inventory);
			MyInventory myInventory = inventory as MyInventory;
			if (myInventory != null)
			{
				myInventory.ContentsChanged -= Inventory_ContentChangedCallback;
			}
		}

		public void InitializeConveyorEndpoint()
		{
			m_multilineConveyorEndpoint = new MyMultilineConveyorEndpoint(this);
			AddDebugRenderComponent(new MyDebugRenderComponentDrawConveyorEndpoint(m_multilineConveyorEndpoint));
		}

		VRage.Game.ModAPI.Ingame.IMyInventory IMyInventoryOwner.GetInventory(int index)
		{
			return this.GetInventory(index);
		}

		public PullInformation GetPullInformation()
		{
			return null;
		}

		public PullInformation GetPushInformation()
		{
			return new PullInformation
			{
				Inventory = this.GetInventory(),
				OwnerID = base.OwnerId,
				Constraint = new MyInventoryConstraint("Empty constraint")
			};
		}

		public bool AllowSelfPulling()
		{
			return false;
		}
	}
}
