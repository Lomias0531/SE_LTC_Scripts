using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Audio;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment;
using Sandbox.Game.WorldEnvironment.Modules;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Import;

namespace Sandbox.Game.Weapons
{
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyShipToolBase),
		typeof(Sandbox.ModAPI.Ingame.IMyShipToolBase)
	})]
	public abstract class MyShipToolBase : MyFunctionalBlock, IMyGunObject<MyToolBase>, IMyInventoryOwner, IMyConveyorEndpointBlock, Sandbox.ModAPI.IMyShipToolBase, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyShipToolBase
	{
		protected class m_useConveyorSystem_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType useConveyorSystem;
				ISyncType result = useConveyorSystem = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyShipToolBase)P_0).m_useConveyorSystem = (Sync<bool, SyncDirection.BothWays>)useConveyorSystem;
				return result;
			}
		}

		protected float DEFAULT_REACH_DISTANCE = 4.5f;

		private MyMultilineConveyorEndpoint m_endpoint;

		private MyDefinitionId m_defId;

		private bool m_wantsToActivate;

		private bool m_isActivated;

		protected bool m_isActivatedOnSomething;

		protected int m_lastTimeActivate;

		private int m_shootHeatup;

		private int m_activateCounter;

		private HashSet<MyEntity> m_entitiesInContact;

		protected BoundingSphere m_detectorSphere;

		protected bool m_checkEnvironmentSector;

		private HashSet<MySlimBlock> m_blocksToActivateOn;

		private HashSet<MySlimBlock> m_tempBlocksBuffer;

		private Sync<bool, SyncDirection.BothWays> m_useConveyorSystem;

		protected MyCharacter m_controller;

		private bool m_effectActivated;

		private bool m_animationActivated;

		protected bool WantsToActivate
		{
			get
			{
				return m_wantsToActivate;
			}
			set
			{
				m_wantsToActivate = value;
				UpdateActivationState();
			}
		}

		public bool IsHeatingUp => m_shootHeatup > 0;

		public int HeatUpFrames
		{
			get;
			protected set;
		}

		public bool IsSkinnable => false;

		protected virtual bool CanInteractWithSelf => false;

		public float BackkickForcePerSecond => 0f;

		public float ShakeAmount
		{
			get;
			protected set;
		}

		public new MyDefinitionId DefinitionId => m_defId;

		public bool EnabledInWorldRules => true;

		public bool UseConveyorSystem
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

		public IMyConveyorEndpoint ConveyorEndpoint => m_endpoint;

		public bool IsShooting => m_isActivated;

		public int ShootDirectionUpdateTime => 0;

		public MyToolBase GunBase => null;

		bool Sandbox.ModAPI.Ingame.IMyShipToolBase.UseConveyorSystem
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

		public MyShipToolBase()
		{
			CreateTerminalControls();
		}

		protected override bool CheckIsWorking()
		{
			if (base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				return base.CheckIsWorking();
			}
			return false;
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyShipToolBase>())
			{
				base.CreateTerminalControls();
				MyTerminalControlOnOffSwitch<MyShipToolBase> obj = new MyTerminalControlOnOffSwitch<MyShipToolBase>("UseConveyor", MySpaceTexts.Terminal_UseConveyorSystem)
				{
					Getter = ((MyShipToolBase x) => x.UseConveyorSystem),
					Setter = delegate(MyShipToolBase x, bool v)
					{
						x.UseConveyorSystem = v;
					}
				};
				obj.EnableToggleAction();
				MyTerminalControlFactory.AddControl(obj);
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(MyStringHash.GetOrCompute("Defense"), 0.002f, ComputeRequiredPower);
			myResourceSinkComponent.IsPoweredChanged += Receiver_IsPoweredChanged;
			base.ResourceSink = myResourceSinkComponent;
			base.Init(objectBuilder, cubeGrid);
			m_entitiesInContact = new HashSet<MyEntity>();
			m_blocksToActivateOn = new HashSet<MySlimBlock>();
			m_tempBlocksBuffer = new HashSet<MySlimBlock>();
			m_isActivated = false;
			m_isActivatedOnSomething = false;
			m_wantsToActivate = false;
			m_shootHeatup = 0;
			m_activateCounter = 0;
			m_defId = objectBuilder.GetId();
			MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(m_defId);
			MyObjectBuilder_ShipToolBase myObjectBuilder_ShipToolBase = objectBuilder as MyObjectBuilder_ShipToolBase;
			float maxVolume = (float)cubeBlockDefinition.Size.X * cubeGrid.GridSize * (float)cubeBlockDefinition.Size.Y * cubeGrid.GridSize * (float)cubeBlockDefinition.Size.Z * cubeGrid.GridSize * 0.5f;
			Vector3 size = new Vector3(cubeBlockDefinition.Size.X, cubeBlockDefinition.Size.Y, (float)cubeBlockDefinition.Size.Z * 0.5f);
			MyInventory inventory = this.GetInventory();
			if (inventory == null)
			{
				inventory = new MyInventory(maxVolume, size, MyInventoryFlags.CanSend);
				base.Components.Add((MyInventoryBase)inventory);
				inventory.Init(myObjectBuilder_ShipToolBase.Inventory);
			}
			base.Enabled = myObjectBuilder_ShipToolBase.Enabled;
			UseConveyorSystem = myObjectBuilder_ShipToolBase.UseConveyorSystem;
			m_checkEnvironmentSector = myObjectBuilder_ShipToolBase.CheckEnvironmentSector;
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			LoadDummies();
			UpdateActivationState();
			base.IsWorkingChanged += MyShipToolBase_IsWorkingChanged;
			base.ResourceSink.Update();
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_ShipToolBase obj = (MyObjectBuilder_ShipToolBase)base.GetObjectBuilderCubeBlock(copy);
			obj.UseConveyorSystem = UseConveyorSystem;
			obj.CheckEnvironmentSector = m_checkEnvironmentSector;
			return obj;
		}

		public override void OnRemovedByCubeBuilder()
		{
			ReleaseInventory(this.GetInventory());
			base.OnRemovedByCubeBuilder();
		}

		public override void OnDestroy()
		{
			ReleaseInventory(this.GetInventory(), damageContent: true);
			base.OnDestroy();
		}

		private void LoadDummies()
		{
			MyModel modelOnlyDummies = MyModels.GetModelOnlyDummies(base.BlockDefinition.Model);
			MyShipToolDefinition myShipToolDefinition = (MyShipToolDefinition)base.BlockDefinition;
			foreach (KeyValuePair<string, MyModelDummy> dummy in modelOnlyDummies.Dummies)
			{
				if (dummy.Key.ToLower().Contains("detector_shiptool"))
				{
					Matrix matrix = dummy.Value.Matrix;
					matrix.Scale.AbsMin();
					Matrix localMatrix = base.PositionComp.LocalMatrix;
					Matrix matrix2 = matrix * localMatrix;
					Vector3 translation = matrix2.Translation;
					m_detectorSphere = new BoundingSphere(translation + matrix2.Forward * myShipToolDefinition.SensorOffset, myShipToolDefinition.SensorRadius);
					break;
				}
			}
		}

		protected void SetBuildingMusic(int amount)
		{
			if (MySession.Static != null && m_controller == MySession.Static.LocalCharacter && MyMusicController.Static != null)
			{
				MyMusicController.Static.Building(amount);
			}
		}

		private bool CanInteractWith(VRage.ModAPI.IMyEntity entity)
		{
			if (entity == null)
			{
				return false;
			}
			if (entity == base.CubeGrid && !CanInteractWithSelf)
			{
				return false;
			}
			if (!(entity is MyCubeGrid) && !(entity is MyCharacter))
			{
				return false;
			}
			return true;
		}

		protected override void OnEnabledChanged()
		{
			WantsToActivate = base.Enabled;
			base.OnEnabledChanged();
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			base.ResourceSink.Update();
		}

		private void Receiver_IsPoweredChanged()
		{
			UpdateIsWorking();
			UpdateActivationState();
		}

		private void MyShipToolBase_IsWorkingChanged(MyCubeBlock obj)
		{
			UpdateActivationState();
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			UpdateActivationState();
		}

		private void UpdateActivationState()
		{
			if (base.ResourceSink != null)
			{
				base.ResourceSink.Update();
			}
			if ((base.Enabled || WantsToActivate) && base.IsFunctional && base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				StartShooting();
			}
			else
			{
				StopShooting();
			}
		}

		private float ComputeRequiredPower()
		{
			if (!base.IsFunctional || (!base.Enabled && !WantsToActivate))
			{
				return 1E-06f;
			}
			return base.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId);
		}

		public override void UpdateAfterSimulation10()
		{
			base.UpdateAfterSimulation10();
			if (m_isActivated && CanShoot(MyShootActionEnum.PrimaryAction, base.OwnerId, out MyGunStatusEnum _))
			{
				if (!m_animationActivated)
				{
					m_animationActivated = true;
					StartAnimation();
				}
				ActivateCommon();
			}
			else if (m_animationActivated)
			{
				m_animationActivated = false;
				StopAnimation();
			}
			if (!m_isActivatedOnSomething && !m_effectActivated)
			{
				return;
			}
			bool flag = Vector3D.DistanceSquared(MySector.MainCamera.Position, base.PositionComp.GetPosition()) < 10000.0;
			if (!m_isActivatedOnSomething || !flag)
			{
				if (m_effectActivated)
				{
					StopEffects();
				}
				m_effectActivated = false;
			}
			else if (m_isActivatedOnSomething)
			{
				if (!m_effectActivated)
				{
					StartEffects();
				}
				m_effectActivated = true;
			}
		}

		protected abstract bool Activate(HashSet<MySlimBlock> targets);

		protected abstract void StartEffects();

		protected abstract void StopEffects();

		protected virtual void StartAnimation()
		{
		}

		protected virtual void StopAnimation()
		{
		}

		private void ActivateCommon()
		{
			BoundingSphereD boundingSphere = new BoundingSphereD(Vector3D.Transform(m_detectorSphere.Center, base.CubeGrid.WorldMatrix), m_detectorSphere.Radius);
			BoundingSphereD sphere = new BoundingSphereD(boundingSphere.Center, m_detectorSphere.Radius * 0.5f);
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
			{
				MyRenderProxy.DebugDrawSphere(boundingSphere.Center, (float)boundingSphere.Radius, Color.Red.ToVector3(), 1f, depthRead: false, smooth: false, cull: true, persistent: true);
				MyRenderProxy.DebugDrawSphere(sphere.Center, (float)sphere.Radius, Color.Blue.ToVector3(), 1f, depthRead: false, smooth: false, cull: true, persistent: true);
			}
			m_isActivatedOnSomething = false;
			List<MyEntity> topMostEntitiesInSphere = MyEntities.GetTopMostEntitiesInSphere(ref boundingSphere);
			bool flag = false;
			m_entitiesInContact.Clear();
			foreach (MyEntity item in topMostEntitiesInSphere)
			{
				if (item is MyEnvironmentSector)
				{
					flag = true;
				}
				MyEntity topMostParent = item.GetTopMostParent();
				if (CanInteractWith(topMostParent))
				{
					m_entitiesInContact.Add(topMostParent);
				}
			}
			if (m_checkEnvironmentSector && flag)
			{
				MyPhysics.HitInfo? hitInfo = MyPhysics.CastRay(boundingSphere.Center, boundingSphere.Center + boundingSphere.Radius * base.WorldMatrix.Forward, 24);
				if (hitInfo.HasValue && hitInfo.HasValue)
				{
					VRage.ModAPI.IMyEntity hitEntity = hitInfo.Value.HkHitInfo.GetHitEntity();
					if (hitEntity is MyEnvironmentSector)
					{
						MyEnvironmentSector myEnvironmentSector = hitEntity as MyEnvironmentSector;
						uint shapeKey = hitInfo.Value.HkHitInfo.GetShapeKey(0);
						int itemFromShapeKey = myEnvironmentSector.GetItemFromShapeKey(shapeKey);
						if (myEnvironmentSector.DataView.Items[itemFromShapeKey].ModelIndex >= 0)
						{
							MyBreakableEnvironmentProxy module = myEnvironmentSector.GetModule<MyBreakableEnvironmentProxy>();
							Vector3D hitnormal = base.CubeGrid.WorldMatrix.Right + base.CubeGrid.WorldMatrix.Forward;
							hitnormal.Normalize();
							float mass = base.CubeGrid.Physics.Mass;
							float num = 10f * 10f * mass;
							module.BreakAt(itemFromShapeKey, hitInfo.Value.HkHitInfo.Position, hitnormal, num);
						}
					}
				}
			}
			topMostEntitiesInSphere.Clear();
			foreach (MyEntity item2 in m_entitiesInContact)
			{
				MyCubeGrid myCubeGrid = item2 as MyCubeGrid;
				MyCharacter myCharacter = item2 as MyCharacter;
				if (myCubeGrid != null)
				{
					m_tempBlocksBuffer.Clear();
					myCubeGrid.GetBlocksInsideSphere(ref boundingSphere, m_tempBlocksBuffer, checkTriangles: true);
					m_blocksToActivateOn.UnionWith(m_tempBlocksBuffer);
				}
				if (myCharacter != null && Sync.IsServer)
				{
					MyStringHash damageType = MyDamageType.Drill;
					if (this is Sandbox.ModAPI.IMyShipGrinder)
					{
						damageType = MyDamageType.Grind;
					}
					else if (this is Sandbox.ModAPI.IMyShipWelder)
					{
						damageType = MyDamageType.Weld;
					}
					if (new MyOrientedBoundingBoxD(myCharacter.PositionComp.LocalAABB, myCharacter.PositionComp.WorldMatrix).Intersects(ref sphere))
					{
						myCharacter.DoDamage(20f, damageType, updateSync: true, base.EntityId);
					}
				}
			}
			m_isActivatedOnSomething |= Activate(m_blocksToActivateOn);
			m_activateCounter++;
			m_lastTimeActivate = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			PlayLoopSound(m_isActivatedOnSomething);
			m_blocksToActivateOn.Clear();
		}

		public override void OnRemovedFromScene(object source)
		{
			base.OnRemovedFromScene(source);
			StopEffects();
			StopLoopSound();
		}

		public override void OnAddedToScene(object source)
		{
			LoadDummies();
			base.OnAddedToScene(source);
			UpdateActivationState();
		}

		protected override void Closing()
		{
			base.Closing();
			StopEffects();
			StopLoopSound();
		}

		protected virtual void StartShooting()
		{
			m_isActivated = true;
		}

		protected virtual void StopShooting()
		{
			m_wantsToActivate = false;
			m_isActivated = false;
			m_isActivatedOnSomething = false;
			if (base.Physics != null)
			{
				base.Physics.Enabled = false;
			}
			if (base.ResourceSink != null)
			{
				base.ResourceSink.Update();
			}
			m_shootHeatup = 0;
			StopEffects();
			StopLoopSound();
		}

		public int GetAmmunitionAmount()
		{
			throw new NotImplementedException();
		}

		public virtual void OnControlAcquired(MyCharacter owner)
		{
		}

		public virtual void OnControlReleased()
		{
			if (!base.Enabled && !base.Closed)
			{
				StopShooting();
			}
		}

		public void DrawHud(IMyCameraController camera, long playerId, bool fullUpdate)
		{
			DrawHud(camera, playerId);
		}

		public void DrawHud(IMyCameraController camera, long playerId)
		{
		}

		public void SetInventory(MyInventory inventory, int index)
		{
			base.Components.Add((MyInventoryBase)inventory);
		}

		public void InitializeConveyorEndpoint()
		{
			m_endpoint = new MyMultilineConveyorEndpoint(this);
			AddDebugRenderComponent(new MyDebugRenderComponentDrawConveyorEndpoint(m_endpoint));
		}

		protected abstract void StopLoopSound();

		protected abstract void PlayLoopSound(bool activated);

		public Vector3 DirectionToTarget(Vector3D target)
		{
			throw new NotImplementedException();
		}

		public virtual bool CanShoot(MyShootActionEnum action, long shooter, out MyGunStatusEnum status)
		{
			status = MyGunStatusEnum.OK;
			if (action != 0)
			{
				status = MyGunStatusEnum.Failed;
				return false;
			}
			if (!base.IsFunctional)
			{
				status = MyGunStatusEnum.NotFunctional;
				return false;
			}
			if (!HasPlayerAccess(shooter))
			{
				status = MyGunStatusEnum.AccessDenied;
				return false;
			}
			if (MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastTimeActivate < 250)
			{
				status = MyGunStatusEnum.Cooldown;
				return false;
			}
			return true;
		}

		public void Shoot(MyShootActionEnum action, Vector3 direction, Vector3D? overrideWeaponPos, string gunAction)
		{
			if (action == MyShootActionEnum.PrimaryAction)
			{
				if (m_shootHeatup < HeatUpFrames)
				{
					m_shootHeatup++;
					return;
				}
				WantsToActivate = true;
				base.ResourceSink.Update();
			}
		}

		public virtual void BeginShoot(MyShootActionEnum action)
		{
		}

		public virtual void EndShoot(MyShootActionEnum action)
		{
			if (action == MyShootActionEnum.PrimaryAction && !base.Enabled)
			{
				StopShooting();
			}
		}

		public void BeginFailReaction(MyShootActionEnum action, MyGunStatusEnum status)
		{
		}

		public void BeginFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status)
		{
		}

		public void ShootFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status)
		{
		}

		public void UpdateSoundEmitter()
		{
			if (m_soundEmitter != null)
			{
				m_soundEmitter.Update();
			}
		}

		public bool SupressShootAnimation()
		{
			return false;
		}

		VRage.Game.ModAPI.Ingame.IMyInventory IMyInventoryOwner.GetInventory(int index)
		{
			return MyEntityExtensions.GetInventory(this, index);
		}

		public virtual PullInformation GetPullInformation()
		{
			return null;
		}

		public virtual PullInformation GetPushInformation()
		{
			return null;
		}

		public bool AllowSelfPulling()
		{
			return false;
		}
	}
}
