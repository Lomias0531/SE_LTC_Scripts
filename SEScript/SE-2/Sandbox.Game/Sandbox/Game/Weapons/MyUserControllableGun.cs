using Sandbox.Common.ObjectBuilders;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Weapons
{
	[MyCubeBlockType(typeof(MyObjectBuilder_UserControllableGun))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyUserControllableGun),
		typeof(Sandbox.ModAPI.Ingame.IMyUserControllableGun)
	})]
	public abstract class MyUserControllableGun : MyFunctionalBlock, Sandbox.ModAPI.IMyUserControllableGun, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyUserControllableGun
	{
		protected sealed class ShootOncePressedEvent_003C_003E : ICallSite<MyUserControllableGun, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyUserControllableGun @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ShootOncePressedEvent();
			}
		}

		protected class m_isShooting_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType isShooting;
				ISyncType result = isShooting = new Sync<bool, SyncDirection.FromServer>(P_1, P_2);
				((MyUserControllableGun)P_0).m_isShooting = (Sync<bool, SyncDirection.FromServer>)isShooting;
				return result;
			}
		}

		protected class m_forceShoot_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType forceShoot;
				ISyncType result = forceShoot = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyUserControllableGun)P_0).m_forceShoot = (Sync<bool, SyncDirection.BothWays>)forceShoot;
				return result;
			}
		}

		protected Sync<bool, SyncDirection.FromServer> m_isShooting;

		protected static readonly MyStringId ID_RED_DOT = MyStringId.GetOrCompute("RedDot");

		protected readonly Sync<bool, SyncDirection.BothWays> m_forceShoot;

		protected bool m_shootingBegun;

		bool Sandbox.ModAPI.Ingame.IMyUserControllableGun.IsShooting => m_isShooting;

		public event Action<int> ReloadStarted;

		public MyUserControllableGun()
		{
			CreateTerminalControls();
			m_isShooting.ValueChanged += delegate
			{
				ShootingChanged();
			};
			base.NeedsWorldMatrix = true;
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyUserControllableGun>())
			{
				base.CreateTerminalControls();
				if (MyFakes.ENABLE_WEAPON_TERMINAL_CONTROL)
				{
					MyTerminalControlButton<MyUserControllableGun> myTerminalControlButton = new MyTerminalControlButton<MyUserControllableGun>("ShootOnce", MySpaceTexts.Terminal_ShootOnce, MySpaceTexts.Blank, delegate(MyUserControllableGun b)
					{
						b.OnShootOncePressed();
					});
					myTerminalControlButton.EnableAction();
					MyTerminalControlFactory.AddControl(myTerminalControlButton);
					MyTerminalControlOnOffSwitch<MyUserControllableGun> obj = new MyTerminalControlOnOffSwitch<MyUserControllableGun>("Shoot", MySpaceTexts.Terminal_Shoot)
					{
						Getter = ((MyUserControllableGun x) => x.m_forceShoot),
						Setter = delegate(MyUserControllableGun x, bool v)
						{
							x.OnShootPressed(v);
						}
					};
					obj.EnableToggleAction();
					obj.EnableOnOffActions();
					MyTerminalControlFactory.AddControl(obj);
					MyTerminalControlFactory.AddControl(new MyTerminalControlSeparator<MyUserControllableGun>());
				}
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.Init(objectBuilder, cubeGrid);
			MyObjectBuilder_UserControllableGun myObjectBuilder_UserControllableGun = objectBuilder as MyObjectBuilder_UserControllableGun;
			m_forceShoot.SetLocalValue(myObjectBuilder_UserControllableGun.IsShootingFromTerminal);
			m_isShooting.SetLocalValue(myObjectBuilder_UserControllableGun.IsShooting);
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			m_forceShoot.ValueChanged += OnForceShootChanged;
		}

		private void OnForceShootChanged(SyncBase obj)
		{
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_UserControllableGun obj = (MyObjectBuilder_UserControllableGun)base.GetObjectBuilderCubeBlock(copy);
			obj.IsShooting = m_isShooting;
			obj.IsShootingFromTerminal = m_forceShoot;
			return obj;
		}

		public virtual bool IsStationary()
		{
			return false;
		}

		public virtual Vector3D GetWeaponMuzzleWorldPosition()
		{
			return base.WorldMatrix.Translation;
		}

		private void OnShootOncePressed()
		{
			SyncRotationAndOrientation();
			MyMultiplayer.RaiseEvent(this, (MyUserControllableGun x) => x.ShootOncePressedEvent);
		}

		[Event(null, 110)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		public void ShootOncePressedEvent()
		{
			Shoot();
		}

		public void SetShooting(bool shooting)
		{
			OnShootPressed(shooting);
		}

		private void OnShootPressed(bool isShooting)
		{
			m_forceShoot.Value = isShooting;
			if (isShooting)
			{
				BeginShoot(MyShootActionEnum.PrimaryAction);
				SyncRotationAndOrientation();
			}
			else
			{
				EndShoot(MyShootActionEnum.PrimaryAction);
			}
		}

		private void Shoot()
		{
			if (CanShoot(MyShootActionEnum.PrimaryAction, base.OwnerId, out MyGunStatusEnum status) && CanShoot(out status) && CanOperate())
			{
				ShootFromTerminal(base.WorldMatrix.Forward);
			}
		}

		public virtual void BeginShoot(MyShootActionEnum action)
		{
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			m_shootingBegun = true;
			Shoot();
			RememberIdle();
			TakeControlFromTerminal();
			if (MyVisualScriptLogicProvider.WeaponBlockActivated != null)
			{
				WeaponBlockActivatedEvent weaponBlockActivated = MyVisualScriptLogicProvider.WeaponBlockActivated;
				long entityId = base.EntityId;
				long entityId2 = base.CubeGrid.EntityId;
				string name = Name;
				string name2 = base.CubeGrid.Name;
				MyObjectBuilderType typeId = base.BlockDefinition.Id.TypeId;
				string blockType = typeId.ToString();
				MyStringHash subtypeId = base.BlockDefinition.Id.SubtypeId;
				weaponBlockActivated(entityId, entityId2, name, name2, blockType, subtypeId.ToString());
			}
		}

		public virtual void EndShoot(MyShootActionEnum action)
		{
			m_shootingBegun = false;
			RestoreIdle();
			base.Render.NeedsDrawFromParent = false;
			StopShootFromTerminal();
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			base.Render.NeedsDrawFromParent = ((bool)m_isShooting || (bool)m_forceShoot);
			if ((bool)m_isShooting || (bool)m_forceShoot)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			}
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (base.IsWorking && ((bool)m_isShooting || (bool)m_forceShoot))
			{
				TakeControlFromTerminal();
				Shoot();
				RotateModels();
			}
		}

		public virtual void ShootFromTerminal(Vector3 direction)
		{
			if (MyVisualScriptLogicProvider.WeaponBlockActivated != null)
			{
				WeaponBlockActivatedEvent weaponBlockActivated = MyVisualScriptLogicProvider.WeaponBlockActivated;
				long entityId = base.EntityId;
				long entityId2 = base.CubeGrid.EntityId;
				string name = Name;
				string name2 = base.CubeGrid.Name;
				MyObjectBuilderType typeId = base.BlockDefinition.Id.TypeId;
				string blockType = typeId.ToString();
				MyStringHash subtypeId = base.BlockDefinition.Id.SubtypeId;
				weaponBlockActivated(entityId, entityId2, name, name2, blockType, subtypeId.ToString());
			}
		}

		public abstract void StopShootFromTerminal();

		public abstract bool CanShoot(MyShootActionEnum action, long shooter, out MyGunStatusEnum status);

		public virtual bool CanShoot(out MyGunStatusEnum status)
		{
			status = MyGunStatusEnum.OK;
			return true;
		}

		public abstract bool CanOperate();

		public virtual void TakeControlFromTerminal()
		{
		}

		public virtual void SyncRotationAndOrientation()
		{
		}

		protected virtual void RotateModels()
		{
		}

		protected virtual void RememberIdle()
		{
		}

		protected virtual void RestoreIdle()
		{
		}

		protected void ShootingChanged()
		{
			if ((bool)m_isShooting)
			{
				BeginShoot(MyShootActionEnum.PrimaryAction);
			}
			else
			{
				EndShoot(MyShootActionEnum.PrimaryAction);
			}
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
		}

		public override void OnRemovedByCubeBuilder()
		{
			MyInventory inventory = this.GetInventory();
			if (inventory != null)
			{
				ReleaseInventory(inventory);
			}
			base.OnRemovedByCubeBuilder();
		}

		public override void OnDestroy()
		{
			MyInventory inventory = this.GetInventory();
			if (inventory != null)
			{
				ReleaseInventory(inventory);
			}
			base.OnDestroy();
		}

		public void OnReloadStarted(int reloadTime)
		{
			this.ReloadStarted?.Invoke(reloadTime);
		}
	}
}
